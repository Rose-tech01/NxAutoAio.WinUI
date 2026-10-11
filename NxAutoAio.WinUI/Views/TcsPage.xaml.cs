using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NxAutoAio.WinUI.Models;

namespace NxAutoAio.WinUI.Views;

public sealed partial class TcsPage : Page
{
    private readonly ObservableCollection<MawbModel> _items = new()
    {
        new() { Index = 1, Number = "738-12345670", TcsStatus = "Đã thông quan", PrintStatus = "Sẵn sàng" },
        new() { Index = 2, Number = "738-12345681", TcsStatus = "Đang xử lý", PrintStatus = "Chờ in" },
        new() { Index = 3, Number = "738-12345692", TcsStatus = "Đã thông quan", PrintStatus = "Sẵn sàng" },
        new() { Index = 4, Number = "738-12345703", TcsStatus = "Cần kiểm tra", PrintStatus = "Chưa in" },
        new() { Index = 5, Number = "738-12345714", TcsStatus = "Đang xử lý", PrintStatus = "Chờ in" }
    };

    public TcsPage()
    {
        InitializeComponent();
        LoadRows();
    }

    private void Load_Click(object sender, RoutedEventArgs e)
    {
        FileStatus.Text = $"{_items.Count} dòng dữ liệu mẫu";
    }

    private void LoadRows()
    {
        MawbList.ItemsSource = null;
        MawbList.ItemsSource = _items;
    }
}
