using Microsoft.UI.Xaml.Controls;

namespace NxAutoAio.WinUI.Controls;

public sealed partial class PageHeader : UserControl
{
    public PageHeader(string title, string subtitle)
    {
        InitializeComponent();
        TitleText.Text = title;
        SubtitleText.Text = subtitle;
    }
}
