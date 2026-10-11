namespace NxAutoAio.WinUI.Models;

public sealed class HawbModel
{
    // Một MAWB có thể có nhiều HAWB; backend trả về danh sách này sau khi người dùng chọn MAWB.
    public string Number { get; init; } = string.Empty;
    public string Destination { get; init; } = string.Empty;
    public int Packages { get; init; }
    public string Weight { get; init; } = string.Empty;
    public string Shipper { get; init; } = string.Empty;
    public string Consignee { get; init; } = string.Empty;
}
