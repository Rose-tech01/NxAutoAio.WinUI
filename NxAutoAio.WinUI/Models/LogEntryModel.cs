namespace NxAutoAio.WinUI.Models;

/// <summary>
/// Một dòng nhật ký vận hành gồm thời điểm, mức độ và nội dung sự kiện.
/// </summary>
public sealed class LogEntryModel
{
    public string Time { get; init; } = string.Empty;
    public string Level { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
