namespace NxAutoAio.WinUI.Models;

/// <summary>
/// Thông tin vận đơn chính MAWB dùng chung cho màn hình TCS và AS400.
/// </summary>
public sealed class MawbModel
{
    // Model dùng chung cho danh sách MAWB ở màn hình TCS và AS400.
    // Khi nối backend, nên giữ tên thuộc tính này ổn định để hạn chế sửa XAML.
    public int Index { get; init; }
    public string Number { get; init; } = string.Empty;
    // Thông tin chuyến bay và lô hàng thường lấy từ nguồn AS400.
    public string Flight { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public string Destination { get; init; } = string.Empty;
    public int Packages { get; init; }
    public string Weight { get; init; } = string.Empty;
    // Các trường trạng thái/nghiệp vụ được dùng ở màn hình TCS.
    public string TcsStatus { get; init; } = string.Empty;
    public string PrintStatus { get; init; } = string.Empty;
    public int HawbCount { get; init; }
    public string Fee { get; init; } = "Chưa tra";
}
