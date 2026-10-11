using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace NxAutoAio.WinUI.Views;

public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
        Loaded += (_, _) => UpdateLayout(PageRoot.ActualWidth);
    }

    private void PageRoot_SizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdateLayout(e.NewSize.Width);

    private void UpdateLayout(double width)
    {
        var compact = width < 980;
        var narrow = width < 620;
        var columnCount = narrow ? 1 : compact ? 2 : 4;

        DashboardGrid.ColumnDefinitions.Clear();
        for (var i = 0; i < columnCount; i++)
            DashboardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        DashboardGrid.RowDefinitions.Clear();
        var statRows = narrow ? 4 : 2;
        var totalRows = statRows + 1 + (narrow || compact ? 1 : 0);
        for (var i = 0; i < totalRows; i++)
            DashboardGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        SetPosition(StatCard1, 0, 0, 1);
        SetPosition(StatCard2, narrow ? 1 : 0, narrow ? 0 : 1, 1);
        SetPosition(StatCard3, narrow ? 2 : 1, narrow ? 0 : 0, 1);
        SetPosition(StatCard4, narrow ? 3 : 1, narrow ? 0 : 1, 1);

        var bottomRow = statRows;
        SetPosition(ShipmentCard, bottomRow, 0, columnCount);
        SetPosition(ProgressCard, narrow ? bottomRow + 1 : bottomRow, narrow ? 0 : compact ? 0 : 2,
            narrow ? 1 : compact ? columnCount : 2);
    }

    private static void SetPosition(FrameworkElement element, int row, int column, int columnSpan)
    {
        Grid.SetRow(element, row);
        Grid.SetColumn(element, column);
        Grid.SetColumnSpan(element, columnSpan);
    }
}
