namespace NxAutoAio.WinUI.Models;

/// <summary>
/// Thông tin một vận đơn thứ cấp HAWB thuộc danh sách của một lô hàng.
/// </summary>
public sealed class HawbModel
{
    // Một MAWB có thể có nhiều HAWB; backend trả về danh sách này sau khi người dùng chọn MAWB.
    public string Number { get; init; } = string.Empty;
    // Thông tin tuyến/kiện và các bên gửi nhận do nguồn dữ liệu cung cấp.
    public string Destination { get; init; } = string.Empty;
    public int Packages { get; init; }
    public string Weight { get; init; } = string.Empty;
    public string Shipper { get; init; } = string.Empty;
    public string Consignee { get; init; } = string.Empty;
}
