namespace NxAutoAio.WinUI.Models;

public sealed class MawbModel
{
    // Model dùng chung cho danh sách MAWB ở màn hình TCS và AS400.
    // Khi nối backend, nên giữ tên thuộc tính này ổn định để hạn chế sửa XAML.
    public int Index { get; init; }
    public string Number { get; init; } = string.Empty;
    public string Flight { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public string Destination { get; init; } = string.Empty;
    public int Packages { get; init; }
    public string Weight { get; init; } = string.Empty;
    public string TcsStatus { get; init; } = string.Empty;
    public string PrintStatus { get; init; } = string.Empty;
    public int HawbCount { get; init; }
    public string Fee { get; init; } = "Chưa tra";
}
