namespace NxAutoAio.WinUI.Models;

public sealed class HawbModel
{
    public string Number { get; init; } = string.Empty;
    public string Destination { get; init; } = string.Empty;
    public int Packages { get; init; }
    public string Weight { get; init; } = string.Empty;
    public string Shipper { get; init; } = string.Empty;
    public string Consignee { get; init; } = string.Empty;
}
