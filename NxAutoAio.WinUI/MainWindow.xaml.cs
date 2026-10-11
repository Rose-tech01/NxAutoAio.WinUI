using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace NxAutoAio.WinUI;

public sealed partial class MainWindow : Window
{
    private bool _darkTheme;

    public MainWindow()
    {
        InitializeComponent();
        ContentFrame.ContentTransitions = null;
        RootGrid.Loaded += RootGrid_Loaded;
        RootGrid.ActualThemeChanged += RootGrid_ActualThemeChanged;
        MainNavigation.SelectedItem = MainNavigation.MenuItems[0];
        ContentFrame.Navigate(typeof(Views.DashboardPage));
    }

    private void RootGrid_Loaded(object sender, RoutedEventArgs e)
    {
        SyncThemeState();
    }

    private void RootGrid_ActualThemeChanged(FrameworkElement sender, object args)
    {
        SyncThemeState();
    }

    private void SyncThemeState()
    {
        _darkTheme = RootGrid.ActualTheme == ElementTheme.Dark;
        ThemeIcon.Glyph = _darkTheme ? "\uE708" : "\uE706";
        ToolTipService.SetToolTip(
            ThemeButton,
            _darkTheme ? "Chuyển sang giao diện sáng" : "Chuyển sang giao diện tối");
    }

    private void MainNavigation_SelectionChanged(
        NavigationView sender,
        NavigationViewSelectionChangedEventArgs args)
    {
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
        _darkTheme = !_darkTheme;
        RootGrid.RequestedTheme = _darkTheme ? ElementTheme.Dark : ElementTheme.Light;
        ThemeIcon.Glyph = _darkTheme ? "\uE708" : "\uE706";
        ToolTipService.SetToolTip(
            ThemeButton,
            _darkTheme ? "Chuyển sang giao diện sáng" : "Chuyển sang giao diện tối");
    }
}
