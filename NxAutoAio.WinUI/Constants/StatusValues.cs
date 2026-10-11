namespace NxAutoAio.WinUI.Constants;

/// <summary>
/// Mã trạng thái dùng trong logic; chuỗi hiển thị vẫn giữ nguyên như giao diện hiện tại.
/// </summary>
internal static class StatusValues
{
    public const string All = "All";
    public const string Cleared = "Đã thông quan";
    public const string Processing = "Đang xử lý";
    public const string NeedsReview = "Cần kiểm tra";
    public const string NotProcessed = "Chưa xử lý";
    public const string Printed = "Đã in";
    public const string PrintError = "Lỗi";
    public const string MissingBarcode = "Thiếu mã vạch";
}
