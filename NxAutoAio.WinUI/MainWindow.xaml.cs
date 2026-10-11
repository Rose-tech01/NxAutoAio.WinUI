using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace NxAutoAio.WinUI;

public sealed partial class MainWindow : Window
{
    // Lưu trạng thái giao diện hiện tại để đồng bộ biểu tượng và nhãn nút.
    private bool _darkTheme;

    public MainWindow()
    {
        InitializeComponent();
        // Khởi tạo trang tổng quan mặc định và đăng ký cập nhật bố cục/giao diện.
        ContentFrame.ContentTransitions = null;
        RootGrid.Loaded += RootGrid_Loaded;
        RootGrid.ActualThemeChanged += RootGrid_ActualThemeChanged;
        MainNavigation.SelectedItem = MainNavigation.MenuItems[0];
        ContentFrame.Navigate(typeof(Views.DashboardPage));
    }

    private void RootGrid_Loaded(object sender, RoutedEventArgs e)
    {
        // Đọc theme thực tế sau khi giao diện đã được dựng xong.
        SyncThemeState();
        UpdateThemeButtonLayout();
    }

    private void RootGrid_ActualThemeChanged(FrameworkElement sender, object args)
    {
        // Đồng bộ nút chuyển theme nếu theme bị đổi từ bên ngoài cửa sổ.
        SyncThemeState();
    }

    private void MainNavigation_DisplayModeChanged(
        NavigationView sender,
        NavigationViewDisplayModeChangedEventArgs args)
    {
        UpdateThemeButtonLayout();
    }

    private void UpdateThemeButtonLayout()
    {
        // Mục theme ở chân NavigationView luôn có thể tương tác ở mọi chế độ hiển thị.
        var compact = MainNavigation.DisplayMode != NavigationViewDisplayMode.Expanded;
        ThemeFooterItem.IsHitTestVisible = true;
    }

    private void SyncThemeState()
    {
        // Cập nhật biểu tượng, nội dung và gợi ý theo theme đang được áp dụng.
        _darkTheme = RootGrid.ActualTheme == ElementTheme.Dark;
        ThemeIcon.Glyph = _darkTheme ? "\uE706" : "\uE708";
        ThemeIcon.FontSize = 14;
        ThemeIcon.Margin = new Thickness(-4, 0, 0, 0);
        ThemeFooterItem.Content = _darkTheme ? "Giao diện sáng" : "Giao diện tối";
        ToolTipService.SetToolTip(
            ThemeFooterItem,
            _darkTheme ? "Chuyển sang giao diện sáng" : "Chuyển sang giao diện tối");
    }

    private void MainNavigation_SelectionChanged(
        NavigationView sender,
        NavigationViewSelectionChangedEventArgs args)
    {
        // Ánh xạ Tag của mục điều hướng sang trang tương ứng trong Frame.
        if (args.SelectedItem is not NavigationViewItem item)
            return;

        var pageType = item.Tag?.ToString() switch
        {
            "Dashboard" => typeof(Views.DashboardPage),
            "Tcs" => typeof(Views.TcsPage),
            "As400" => typeof(Views.As400Page),
            "Scsc" => typeof(Views.ScscPage),
            "Logs" => typeof(Views.LogPage),
            "About" => typeof(Views.AboutPage),
            _ => typeof(Views.DeclarationPage)
        };

        if (ContentFrame.CurrentSourcePageType != pageType)
            ContentFrame.Navigate(pageType);
    }

    private void ThemeButton_Click(object sender, RoutedEventArgs e)
    {
        ToggleTheme();
    }

    private void ThemeFooterItem_Tapped(object sender, TappedRoutedEventArgs e)
    {
        ToggleTheme();
        e.Handled = true;
    }

    private void ToggleTheme()
    {
        // Đổi theme của toàn bộ vùng nội dung rồi cập nhật lại điều khiển chuyển theme.
        _darkTheme = !_darkTheme;
        RootGrid.RequestedTheme = _darkTheme ? ElementTheme.Dark : ElementTheme.Light;
        ThemeIcon.Glyph = _darkTheme ? "\uE706" : "\uE708";
        ThemeIcon.FontSize = 14;
        ThemeIcon.Margin = new Thickness(-4, 0, 0, 0);
        ThemeFooterItem.Content = _darkTheme ? "Giao diện sáng" : "Giao diện tối";
        ToolTipService.SetToolTip(
            ThemeFooterItem,
            _darkTheme ? "Chuyển sang giao diện sáng" : "Chuyển sang giao diện tối");
    }
}
