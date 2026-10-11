namespace NxAutoAio.WinUI.Models;

/// <summary>
/// Nội dung một thẻ thống kê trên dashboard; giá trị được chuẩn bị để hiển thị trong giao diện.
/// </summary>
public sealed class DashboardStatisticModel
{
    // Nhãn, số liệu, mô tả bổ sung và ký tự biểu tượng của thẻ.
    public string Title { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string IconGlyph { get; init; } = "\uE10F";
}
