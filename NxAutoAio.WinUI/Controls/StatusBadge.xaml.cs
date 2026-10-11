using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace NxAutoAio.WinUI.Controls;

public sealed partial class StatusBadge : UserControl
{
    public StatusBadge(string status)
    {
        InitializeComponent();
        StatusText.Text = status;
        Badge.Background = new SolidColorBrush(status.Contains("Hoàn") || status.Contains("Đã")
            ? Color.FromArgb(50, 39, 190, 142)
            : status.Contains("Lỗi")
                ? Color.FromArgb(50, 232, 80, 80)
                : Color.FromArgb(50, 240, 180, 70));
    }
}
