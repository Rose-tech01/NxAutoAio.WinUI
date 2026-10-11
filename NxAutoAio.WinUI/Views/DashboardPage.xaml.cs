using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NxAutoAio.WinUI.Constants;
using NxAutoAio.WinUI.Helpers;

namespace NxAutoAio.WinUI.Views;

/// <summary>
/// Hiển thị số liệu tổng quan mẫu và tự sắp xếp các thẻ theo chiều rộng cửa sổ.
/// </summary>
public sealed partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
        // Tính bố cục ban đầu sau khi giao diện biết được kích thước thực tế.
        Loaded += (_, _) => UpdateLayout(PageRoot.ActualWidth);
    }

    private void PageRoot_SizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdateLayout(e.NewSize.Width);

    private void UpdateLayout(double width)
    {
        // Đây chỉ là logic bố cục; dữ liệu dashboard nên được nạp qua service backend riêng.
        // Các ngưỡng chiều rộng quyết định số cột để thẻ không bị chật trên cửa sổ nhỏ.
        var narrow = width < ResponsiveBreakpoints.DashboardNarrow;
        var compact = width < ResponsiveBreakpoints.DashboardCompact;
        UpdateHeaderLayout(width);

        SetStatColumns(narrow ? 2 : compact ? 3 : 6);
        GridLayoutHelper.SetLayout(WorkGrid, narrow ? 1 : 2, narrow ? 2 : 1);
        GridLayoutHelper.SetLayout(StatusGrid, narrow ? 1 : compact ? 2 : 4, narrow ? 4 : compact ? 2 : 1);
        GridLayoutHelper.SetLayout(BottomGrid, narrow ? 1 : 2, narrow ? 2 : 1);

        GridLayoutHelper.SetPosition(ShipmentCard, 0, 0);
        GridLayoutHelper.SetPosition(AlertCard, narrow ? 1 : 0, narrow ? 0 : 1);
        GridLayoutHelper.SetPosition(ConnectionCard, 0, 0);
        GridLayoutHelper.SetPosition(ActivityCard, narrow ? 1 : 0, narrow ? 0 : 1);

        var statusCards = new FrameworkElement[]
        {
            (FrameworkElement)StatusGrid.Children[0],
            (FrameworkElement)StatusGrid.Children[1],
            (FrameworkElement)StatusGrid.Children[2],
            (FrameworkElement)StatusGrid.Children[3]
        };
        for (var i = 0; i < statusCards.Length; i++)
            GridLayoutHelper.SetPosition(statusCards[i], narrow ? i : i / (compact ? 2 : 4), narrow ? 0 : i % (compact ? 2 : 4));
    }

    private void UpdateHeaderLayout(double width)
    {
        // Trên màn hình hẹp, đưa bộ lọc và nút làm mới xuống hàng dưới tiêu đề.
        var narrow = width < ResponsiveBreakpoints.DashboardHeaderNarrow;
        HeaderGrid.RowDefinitions.Clear();
        HeaderGrid.ColumnDefinitions.Clear();

        if (narrow)
        {
            HeaderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            HeaderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            HeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetRow(HeaderActions, 1);
            Grid.SetColumn(HeaderActions, 0);
            Grid.SetColumnSpan(HeaderActions, 1);
            HeaderActions.HorizontalAlignment = HorizontalAlignment.Stretch;
            HeaderActions.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            HeaderActions.ColumnDefinitions[1].Width = GridLength.Auto;
        }
        else
        {
            HeaderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            HeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            HeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            Grid.SetRow(HeaderActions, 0);
            Grid.SetColumn(HeaderActions, 1);
            Grid.SetColumnSpan(HeaderActions, 1);
            HeaderActions.HorizontalAlignment = HorizontalAlignment.Right;
            HeaderActions.ColumnDefinitions[0].Width = new GridLength(150);
            HeaderActions.ColumnDefinitions[1].Width = GridLength.Auto;
        }
    }

    private void SetStatColumns(int count)
    {
        // Chia sáu thẻ thống kê thành số cột phù hợp và tính số hàng cần thiết.
        var cards = new[] { StatCard1, StatCard2, StatCard3, StatCard4, StatCard5, StatCard6 };
        var rows = (cards.Length + count - 1) / count;
        GridLayoutHelper.SetLayout(DashboardRowGrid(), count, rows);
        for (var i = 0; i < cards.Length; i++)
            GridLayoutHelper.SetPosition(cards[i], i / count, i % count);
    }

    private Grid DashboardRowGrid() => (Grid)StatCard1.Parent;

    private void PeriodFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PeriodFilter.SelectedItem is ComboBoxItem item)
        {
            // Lưu khoảng thời gian đã chọn để backend có thể dùng khi truy vấn số liệu.
            PageRoot.Tag = item.Content?.ToString();
        }
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        // Hiện chỉ cập nhật dấu thời gian; cần thay bằng lệnh đồng bộ dashboard từ C++.
        PageRoot.Tag = "Đã làm mới lúc " + System.DateTime.Now.ToString("HH:mm:ss");
    }
}
