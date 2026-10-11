# Tài liệu tích hợp backend C++ cho NX Auto AIO

Tài liệu này mô tả cách thay dữ liệu mẫu trong giao diện WinUI 3 bằng logic backend C++.
Mục tiêu là giữ nguyên giao diện và thay từng điểm `TODO(backend)` bằng service thật, tránh
đưa logic kết nối AS400/TCS/máy in trực tiếp vào code-behind của XAML.

## 1. Kiến trúc đề xuất

```text
WinUI Page
    -> C# service/interface
        -> lớp interop C++/CLI hoặc DLL C ABI
            -> AS400 / TCS / SCSC / máy in / file system
```

Nên tạo các lớp C# trung gian:

```text
Services/
  IDashboardService.cs
  ITcsService.cs
  IAs400Service.cs
  IDeclarationService.cs
  IPrintService.cs
Interop/
  BackendNative.cs
```

Page chỉ nên thực hiện ba việc:

1. Đọc giá trị điều khiển và tạo request.
2. Gọi service bất đồng bộ.
3. Cập nhật model, trạng thái tải và thông báo lỗi.

Không đặt mã gọi AS400, TCS hoặc máy in trực tiếp trong `DashboardPage.xaml.cs`,
`TcsPage.xaml.cs`, `As400Page.xaml.cs` hay `DeclarationPage.xaml.cs`.

## 2. Các điểm đang dùng dữ liệu mẫu

| Màn hình | Vị trí | Dữ liệu cần thay |
|---|---|---|
| Dashboard | `DashboardPage.xaml.cs` | Số MAWB, HAWB, trạng thái, cảnh báo, tiến trình |
| TCS | `_allItems` trong `TcsPage.xaml.cs` | Danh sách MAWB, trạng thái thông quan, phí, trạng thái in |
| AS400 | `_mawbs`, `_hawbs` trong `As400Page.xaml.cs` | MAWB và HAWB đọc từ AS400 |
| In tờ khai | `_allDocuments` trong `DeclarationPage.xaml.cs` | File tờ khai, mã vạch, trạng thái in |

Các handler đã đánh dấu `TODO(backend)` là điểm bắt đầu tích hợp. Không cần sửa XAML
nếu service trả dữ liệu đúng model hiện tại.

## 3. Contract dữ liệu C#

Backend nên map dữ liệu về các model sau:

### MAWB

```csharp
public sealed record MawbDto(
    string Number,
    string Flight,
    string Date,
    string Destination,
    int Packages,
    string Weight,
    string TcsStatus,
    string PrintStatus,
    int HawbCount,
    string Fee);
```

`Number` là khóa nghiệp vụ chính. Không dùng `Index` làm khóa vì Index chỉ phục vụ
hiển thị và có thể thay đổi sau khi lọc.

### HAWB

```csharp
public sealed record HawbDto(
    string Number,
    string Destination,
    int Packages,
    string Weight,
    string Shipper,
    string Consignee);
```

### Tài liệu tờ khai

```csharp
public sealed record PrintDocumentDto(
    string FileName,
    string Mawb,
    string Hawb,
    string FileFormat,
    int PageCount,
    string BarcodeStatus,
    string PrintStatus,
    string ErrorMessage,
    DateTimeOffset LastUpdated);
```

Có thể map DTO sang `MawbModel`, `HawbModel` và `PrintDocumentModel`. Các chuỗi trạng
thái cần thống nhất với giá trị đang dùng trong ComboBox và bộ lọc:

- `Đã thông quan`
- `Đang xử lý`
- `Cần kiểm tra`
- `Chưa xử lý`
- `Đang xử lý`
- `Đã in`
- `Lỗi`

Nếu backend dùng mã trạng thái, nên map mã sang chuỗi hiển thị ở service, không để
XAML biết mã nội bộ.

## 4. Giao tiếp C++ với C#

### Khuyến nghị: DLL C ABI

Nếu backend là DLL C++ native, nên cung cấp API C ABI ổn định, ví dụ:

```cpp
extern "C" __declspec(dllexport)
int nx_tcs_query_mawb(
    const char* request_json,
    char* response_json,
    int response_capacity);

extern "C" __declspec(dllexport)
void nx_free_buffer(char* buffer);
```

Quy tắc:

- Không truyền `std::string`, `std::vector` hoặc class C++ trực tiếp qua ABI.
- Dùng UTF-8 cho JSON và chuỗi trả về.
- Có mã lỗi riêng cho timeout, mất kết nối, dữ liệu không hợp lệ và lỗi thiết bị.
- C++ cấp hàm giải phóng bộ nhớ nếu C++ cấp phát response.
- Không gọi hàm native trên UI thread.

C# có thể khai báo lớp interop:

```csharp
internal static partial class BackendNative
{
    [LibraryImport("NxAutoBackend.dll", StringMarshalling = StringMarshalling.Utf8)]
    internal static partial int nx_tcs_query_mawb(
        string requestJson,
        StringBuilder responseJson,
        int responseCapacity);
}
```

Nếu dự án đã sử dụng C++/CLI, có thể bọc DLL native bằng lớp managed. Dù dùng cách
nào, UI vẫn nên chỉ phụ thuộc `ITcsService`, `IAs400Service` và các interface tương tự.

## 5. Mẫu service bất đồng bộ

```csharp
public interface IAs400Service
{
    Task<IReadOnlyList<MawbModel>> GetMawbAsync(
        string? query,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<HawbModel>> GetHawbAsync(
        string mawb,
        CancellationToken cancellationToken);
}
```

Service native có thể chạy lệnh blocking trong `Task.Run`, nhưng phải giới hạn số
luồng và hỗ trợ `CancellationToken` ở lớp C++ nếu thao tác có thể kéo dài.

Code-behind cần bảo vệ vòng đời trang:

```csharp
private CancellationTokenSource? _loadCancellation;

private async Task LoadMawbAsync()
{
    _loadCancellation?.Cancel();
    _loadCancellation = new CancellationTokenSource();

    try
    {
        var rows = await _as400Service.GetMawbAsync(
            MawbSearchBox.Text,
            _loadCancellation.Token);

        MawbList.ItemsSource = new ObservableCollection<MawbModel>(rows);
        UpdateMawbState();
    }
    catch (OperationCanceledException)
    {
        // Bỏ qua yêu cầu cũ khi người dùng tìm kiếm lại.
    }
    catch (Exception ex)
    {
        // Ghi log nội bộ và hiển thị thông báo lỗi thân thiện.
        StatusText.Text = $"Không thể đọc AS400: {ex.Message}";
    }
}
```

Sau `await`, WinUI thường quay lại UI context. Nếu service cập nhật từ background
thread, phải dùng `DispatcherQueue.TryEnqueue` trước khi thay `ItemsSource`,
`TextBlock.Text` hoặc `ProgressBar.Value`.

## 6. Quy tắc lỗi và trạng thái tải

Mỗi thao tác backend nên có:

- Trạng thái đang tải: disable nút liên quan và hiển thị progress.
- Timeout rõ ràng, không chờ vô hạn.
- Mã lỗi kỹ thuật được ghi vào log.
- Thông báo ngắn gọn cho người vận hành.
- Nút thử lại khi lỗi kết nối.

Không được trả về danh sách rỗng như thể thao tác thành công khi C++ bị lỗi. Danh sách
rỗng hợp lệ và lỗi kết nối phải được phân biệt bằng `Result` hoặc exception.

## 7. Tích hợp theo từng module

### TCS

1. Tạo `ITcsService.LoadAsync`.
2. Thay `_allItems` bằng kết quả service.
3. Với các nút tra thông quan, tra phí và in, truyền danh sách MAWB đã chọn.
4. Cập nhật từng dòng nếu backend trả tiến độ theo sự kiện.

### AS400

1. Tạo `IAs400Service.GetMawbAsync`.
2. Khi chọn MAWB, gọi `GetHawbAsync(mawbNumber)`.
3. Không tải HAWB mẫu dùng chung cho mọi MAWB.
4. Hiển thị trạng thái mất kết nối AS400 và cho phép thử lại.

### In tờ khai

1. Quét thư mục bằng service file system.
2. Kiểm tra mã vạch trước khi đưa vào hàng đợi.
3. Gửi danh sách file đã chọn tới `IPrintService`.
4. Nhận sự kiện tiến độ gồm tên file, số trang, thành công/lỗi.
5. Cập nhật `PrintDocumentModel` trên UI thread.

### Dashboard

1. Tạo `IDashboardService.GetSummaryAsync(period)`.
2. Map số liệu vào các card hiện có.
3. Nút `Làm mới` gọi lại service, không chỉ cập nhật thời gian.
4. Bộ lọc thời gian truyền giá trị enum hoặc mã cố định, không truyền chuỗi hiển thị
   tiếng Việt xuống backend.

## 8. Kiểm thử trước khi bàn giao

- Kiểm thử UTF-8 với tên công ty và địa chỉ tiếng Việt.
- Kiểm thử timeout và rút mạng AS400/TCS.
- Kiểm thử 0 dòng, nhiều dòng và dữ liệu trùng MAWB.
- Kiểm thử đóng trang trong lúc request đang chạy.
- Kiểm thử in thất bại một phần.
- Kiểm thử backend trả dữ liệu thiếu trường bắt buộc.
- Build x64 bằng:

```powershell
dotnet build .\NxAutoAio.WinUI\NxAutoAio.WinUI.csproj -p:Platform=x64 --nologo
```

Backend C++ cần cung cấp DLL đúng kiến trúc với ứng dụng (`x64` hiện là kiến trúc
đang dùng để build và kiểm thử chính).
