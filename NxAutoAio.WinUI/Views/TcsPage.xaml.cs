using System;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NxAutoAio.WinUI.Models;
using System.Linq;

namespace NxAutoAio.WinUI.Views;

public sealed partial class TcsPage : Page
{
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
        LoadRows();
    }

    private void Load_Click(object sender, RoutedEventArgs e)
    {
        FileStatus.Text = $"{_allItems.Count} dòng dữ liệu mẫu đã nạp";
    }

    private void LoadRows()
    {
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
        SelectionStatus.Text = MawbList.SelectedItems.Count > 0
            ? $"Đã chọn {MawbList.SelectedItems.Count} dòng"
            : $"{MawbList.Items.Count} bản ghi";
    }

    private void Action_Click(object sender, RoutedEventArgs e)
    {
        var selected = MawbList.SelectedItems.Count;
        FileStatus.Text = selected > 0
            ? $"Đang mô phỏng thao tác cho {selected} MAWB"
            : "Hãy chọn ít nhất một MAWB để xử lý hàng loạt";
    }
}
