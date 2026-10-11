using Microsoft.UI.Xaml.Controls;

namespace NxAutoAio.WinUI.Controls;

/// <summary>
/// Tiêu đề dùng lại cho trang, gồm tên trang và dòng mô tả bên dưới.
/// </summary>
public sealed partial class PageHeader : UserControl
{
    public PageHeader(string title, string subtitle)
    {
        InitializeComponent();
        // Gán nội dung tiêu đề/mô tả vào các phần tử được đặt tên trong XAML.
        TitleText.Text = title;
        SubtitleText.Text = subtitle;
    }
}
