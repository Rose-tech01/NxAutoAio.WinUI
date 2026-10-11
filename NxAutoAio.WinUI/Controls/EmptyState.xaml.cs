using Microsoft.UI.Xaml.Controls;

namespace NxAutoAio.WinUI.Controls;

public sealed partial class EmptyState : UserControl
{
    public EmptyState(string title, string message)
    {
        InitializeComponent();
        TitleText.Text = title;
        MessageText.Text = message;
    }
}
