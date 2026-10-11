using Microsoft.UI.Xaml.Controls;

namespace NxAutoAio.WinUI.Controls;

/// <summary>
/// Điều khiển trạng thái rỗng với tiêu đề và mô tả do trang chủ cung cấp.
/// </summary>
public sealed partial class EmptyState : UserControl
{
    public EmptyState(string title, string message)
    {
        InitializeComponent();
        // Đưa nội dung truyền vào code-behind lên các vùng chữ trong XAML.
        TitleText.Text = title;
        MessageText.Text = message;
    }
}
