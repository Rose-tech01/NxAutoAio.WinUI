namespace NxAutoAio.WinUI.Models;

public sealed class MawbModel
{
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
