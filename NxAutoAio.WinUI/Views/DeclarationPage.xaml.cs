using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace NxAutoAio.WinUI.Views;

public sealed partial class DeclarationPage : Page
{
    public DeclarationPage() => InitializeComponent();

    private void ChooseFolder_Click(object sender, RoutedEventArgs e) =>
        FolderStatus.Text = "Đã chọn thư mục";

    private void StartPrint_Click(object sender, RoutedEventArgs e)
    {
        PrintProgress.Value = 100;
        StatusText.Text = "Trạng thái: Đã hoàn tất.";
    }
}
