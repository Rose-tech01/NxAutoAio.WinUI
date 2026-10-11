using System;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NxAutoAio.WinUI.Helpers;
using NxAutoAio.WinUI.Models;
using System.Linq;

namespace NxAutoAio.WinUI.Views;

/// <summary>
/// Hiển thị MAWB phục vụ tra cứu TCS; dữ liệu và thao tác dịch vụ hiện là mô phỏng.
/// </summary>
public sealed partial class TcsPage : Page
{
    // Bộ dữ liệu mẫu để thử tìm kiếm, lọc trạng thái và chọn nhiều MAWB.
    private readonly ObservableCollection<MawbModel> _allItems = new()
    {
        new() { Index = 1, Number = "738-12345670", HawbCount = 4, TcsStatus = "Đã thông quan", Fee = "850.000 đ", PrintStatus = "Sẵn sàng" },
        new() { Index = 2, Number = "738-12345681", HawbCount = 8, TcsStatus = "Đang xử lý", Fee = "Chưa tra", PrintStatus = "Chờ in" },
        new() { Index = 3, Number = "738-12345692", HawbCount = 2, TcsStatus = "Đã thông quan", Fee = "420.000 đ", PrintStatus = "Sẵn sàng" },
        new() { Index = 4, Number = "738-12345703", HawbCount = 0, TcsStatus = "Cần kiểm tra", Fee = "Lỗi dữ liệu", PrintStatus = "Chưa in" },
        new() { Index = 5, Number = "738-12345714", HawbCount = 6, TcsStatus = "Đang xử lý", Fee = "Chưa tra", PrintStatus = "Chờ in" },
        new() { Index = 6, Number = "738-12345725", HawbCount = 3, TcsStatus = "Đã thông quan", Fee = "610.000 đ", PrintStatus = "Đã in" }
    };

    public TcsPage()
    {
        InitializeComponent();
        // Hiển thị dữ liệu ban đầu và bố trí thanh lọc/công cụ theo kích thước trang.
        LoadRows();
        Loaded += (_, _) => UpdateLayout(PageRoot.ActualWidth);
        Loaded += (_, _) => UpdateFilterLayout(PageRoot.ActualWidth);
    }

    private void PageRoot_SizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdatePageSize(e.NewSize.Width);

    private void UpdatePageSize(double width)
    {
        UpdateLayout(width);
        UpdateFilterLayout(width);
    }

    private void UpdateFilterLayout(double width)
    {
        // Trên màn hình hẹp, chuyển ô tìm kiếm lên hàng riêng để giữ các bộ lọc dễ dùng.
        var narrow = width < 680;
        FilterGrid.RowDefinitions.Clear();
        FilterGrid.ColumnDefinitions.Clear();

        if (narrow)
        {
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

            Grid.SetRow(SearchBox, 0);
            Grid.SetColumn(SearchBox, 0);
            Grid.SetColumnSpan(SearchBox, 2);
            Grid.SetRow(StatusFilter, 1);
            Grid.SetColumn(StatusFilter, 0);
            Grid.SetColumnSpan(StatusFilter, 1);
            Grid.SetRow(SelectionStatus, 1);
            Grid.SetColumn(SelectionStatus, 1);
        }
        else
        {
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(220) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

            Grid.SetRow(SearchBox, 0);
            Grid.SetColumn(SearchBox, 0);
            Grid.SetColumnSpan(SearchBox, 1);
            Grid.SetRow(StatusFilter, 0);
            Grid.SetColumn(StatusFilter, 1);
            Grid.SetColumnSpan(StatusFilter, 1);
            Grid.SetRow(SelectionStatus, 0);
            Grid.SetColumn(SelectionStatus, 2);
        }
    }

    private void UpdateLayout(double width)
    {
        // Gom các nút thao tác thành nhiều hàng khi chiều rộng không đủ cho một thanh ngang.
        var narrow = width < 760;
        ToolbarGrid.RowDefinitions.Clear();
        ToolbarGrid.ColumnDefinitions.Clear();

        if (narrow)
        {
            for (var i = 0; i < 3; i++)
                ToolbarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            ToolbarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ToolbarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            GridLayoutHelper.StretchButton(LoadButton, 0, 0);
            GridLayoutHelper.StretchButton(ClearanceButton, 0, 1);
            GridLayoutHelper.StretchButton(FeeButton, 1, 0);
            GridLayoutHelper.StretchButton(PrintButton, 1, 1);
            Grid.SetRow(FileStatus, 2);
            Grid.SetColumn(FileStatus, 0);
            Grid.SetColumnSpan(FileStatus, 2);
        }
        else
        {
            ToolbarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            for (var i = 0; i < 5; i++)
                ToolbarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = i == 4 ? new GridLength(1, GridUnitType.Star) : GridLength.Auto });

            GridLayoutHelper.StretchButton(LoadButton, 0, 0);
            GridLayoutHelper.StretchButton(ClearanceButton, 0, 1);
            GridLayoutHelper.StretchButton(FeeButton, 0, 2);
            GridLayoutHelper.StretchButton(PrintButton, 0, 3);
            Grid.SetRow(FileStatus, 0);
            Grid.SetColumn(FileStatus, 4);
            Grid.SetColumnSpan(FileStatus, 1);
        }
    }

    private void Load_Click(object sender, RoutedEventArgs e)
    {
        // Chỉ xác nhận số dòng mẫu; thay bằng lời gọi API/C++ khi kết nối nguồn TCS.
        FileStatus.Text = $"{_allItems.Count} dòng dữ liệu mẫu đã nạp";
    }

    private void LoadRows()
    {
        // Gán lại danh sách gốc và đặt bộ đếm ban đầu khi trang được mở.
        MawbList.ItemsSource = null;
        MawbList.ItemsSource = _allItems;
        SelectionStatus.Text = $"{_allItems.Count} bản ghi";
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

    private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();

    private void ApplyFilter()
    {
        if (MawbList is null || SearchBox is null || StatusFilter is null)
            return;

        // Kết hợp tìm số MAWB với trạng thái TCS; danh sách kết quả không thay đổi dữ liệu gốc.
        var query = SearchBox.Text.Trim();
        var status = (StatusFilter.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "All";
        var filtered = _allItems.Where(item =>
            (string.IsNullOrWhiteSpace(query) || item.Number.Contains(query, StringComparison.OrdinalIgnoreCase)) &&
            (status == "All" || item.TcsStatus == status));

        MawbList.ItemsSource = new ObservableCollection<MawbModel>(filtered);
        SelectionStatus.Text = $"{MawbList.Items.Count} bản ghi";
    }

    private void MawbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Bộ đếm hiển thị số dòng được chọn, hoặc tổng số dòng khi chưa chọn dòng nào.
        SelectionStatus.Text = MawbList.SelectedItems.Count > 0
            ? $"Đã chọn {MawbList.SelectedItems.Count} dòng"
            : $"{MawbList.Items.Count} bản ghi";
    }

    private void Action_Click(object sender, RoutedEventArgs e)
    {
        var selected = MawbList.SelectedItems.Count;
        // Chưa gọi dịch vụ; đây là điểm nối để truyền MAWB đã chọn tới thao tác tương ứng.
        FileStatus.Text = selected > 0
            ? $"Đang mô phỏng thao tác cho {selected} MAWB"
            : "Hãy chọn ít nhất một MAWB để xử lý hàng loạt";
    }
}
