using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NxAutoAio.WinUI.Constants;
using NxAutoAio.WinUI.Data;
using NxAutoAio.WinUI.Models;

namespace NxAutoAio.WinUI.Views;

/// <summary>
/// Hiển thị quan hệ MAWB–HAWB và các thao tác tra cứu AS400 đang được mô phỏng.
/// </summary>
public sealed partial class As400Page : Page
{
    // Danh sách MAWB mẫu; khi tích hợp, danh sách này sẽ đến từ truy vấn AS400.
    private readonly ObservableCollection<MawbModel> _mawbs = SampleDataFactory.CreateAs400Mawbs();

    // Danh sách HAWB mẫu hiện dùng chung để minh họa nội dung sau khi chọn một MAWB.
    private readonly ObservableCollection<HawbModel> _hawbs = SampleDataFactory.CreateAs400Hawbs();

    public As400Page()
    {
        InitializeComponent();
        // Nạp danh sách mẫu ban đầu và cập nhật số dòng/trạng thái rỗng.
        MawbList.ItemsSource = _mawbs;
        UpdateMawbState();
        Loaded += (_, _) => UpdateLayout(PageRoot.ActualWidth);
    }

    private void PageRoot_SizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdateLayout(e.NewSize.Width);

    private void UpdateLayout(double width)
    {
        // Hiển thị hai thẻ cạnh nhau trên màn hình rộng, xếp chồng khi màn hình hẹp.
        var narrow = width < ResponsiveBreakpoints.As400Narrow;
        DataGrid.ColumnDefinitions.Clear();
        DataGrid.RowDefinitions.Clear();

        if (narrow)
        {
            DataGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            DataGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            DataGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(MawbCard, 0);
            Grid.SetRow(MawbCard, 0);
            Grid.SetColumn(HawbCard, 0);
            Grid.SetRow(HawbCard, 1);
        }
        else
        {
            DataGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            DataGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            DataGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(MawbCard, 0);
            Grid.SetRow(MawbCard, 0);
            Grid.SetColumn(HawbCard, 1);
            Grid.SetRow(HawbCard, 0);
        }
    }

    private void MawbSearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Khi chuyển sang backend, nên lọc ở service nếu dữ liệu lớn thay vì tải toàn bộ về UI.
        // Tạm thời lọc danh sách mẫu theo số MAWB và cập nhật trạng thái danh sách.
        var query = MawbSearchBox.Text.Trim();
        MawbList.ItemsSource = new ObservableCollection<MawbModel>(
            _mawbs.Where(item => item.Number.Contains(query, StringComparison.OrdinalIgnoreCase)));
        UpdateMawbState();
    }

    private void MawbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Chỉ hiển thị danh sách HAWB khi có MAWB được chọn.
        if (MawbList.SelectedItem is MawbModel)
        {
            HawbEmptyState.Visibility = Visibility.Collapsed;
            HawbList.Visibility = Visibility.Visible;
            HawbList.ItemsSource = _hawbs;
        }
        else
        {
            HawbEmptyState.Visibility = Visibility.Visible;
            HawbList.Visibility = Visibility.Collapsed;
        }
    }

    private void LoadMawb_Click(object sender, RoutedEventArgs e)
    {
        // Hiện nạp lại danh sách mẫu; thay bằng lệnh đọc MAWB từ AS400 khi tích hợp.
        MawbList.ItemsSource = _mawbs;
        UpdateMawbState();
    }

    private void LoadHawb_Click(object sender, RoutedEventArgs e)
    {
        // Hiện dùng xử lý chọn để hiện danh sách HAWB mẫu; backend cần truy vấn theo MAWB.
        if (MawbList.SelectedItem is MawbModel)
            MawbList_SelectionChanged(MawbList, null!);
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        // Đặt lại tìm kiếm, lựa chọn và dữ liệu mẫu; tích hợp thật cần xử lý lỗi kết nối AS400.
        MawbSearchBox.Text = string.Empty;
        MawbList.SelectedItem = null;
        MawbList.ItemsSource = _mawbs;
        UpdateMawbState();
    }

    private void UpdateMawbState()
    {
        MawbCountText.Text = $"Danh sách MAWB ({MawbList.Items.Count} dòng)";
        MawbEmptyState.Visibility = MawbList.Items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    }
}
