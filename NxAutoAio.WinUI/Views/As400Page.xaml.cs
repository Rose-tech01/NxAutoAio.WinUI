using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NxAutoAio.WinUI.Models;

namespace NxAutoAio.WinUI.Views;

public sealed partial class As400Page : Page
{
    private readonly ObservableCollection<MawbModel> _mawbs = new()
    {
        new() { Number = "738-12345670", Destination = "HAN - Hà Nội", Packages = 12 },
        new() { Number = "738-12345681", Destination = "SGN - Hồ Chí Minh", Packages = 8 },
        new() { Number = "738-12345692", Destination = "DAD - Đà Nẵng", Packages = 21 },
        new() { Number = "738-12345703", Destination = "HAN - Hà Nội", Packages = 5 },
        new() { Number = "738-12345714", Destination = "SGN - Hồ Chí Minh", Packages = 16 }
    };

    private readonly ObservableCollection<HawbModel> _hawbs = new()
    {
        new() { Number = "HWB-001", Consignee = "Công ty Minh Long", Packages = 4 },
        new() { Number = "HWB-002", Consignee = "Công ty Á Châu", Packages = 8 },
        new() { Number = "HWB-003", Consignee = "Công ty Tân Tiến", Packages = 3 }
    };

    public As400Page()
    {
        InitializeComponent();
        MawbList.ItemsSource = _mawbs;
        UpdateMawbState();
        Loaded += (_, _) => UpdateLayout(PageRoot.ActualWidth);
    }

    private void PageRoot_SizeChanged(object sender, SizeChangedEventArgs e) =>
        UpdateLayout(e.NewSize.Width);

    private void UpdateLayout(double width)
    {
        var narrow = width < 820;
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
        var query = MawbSearchBox.Text.Trim();
        MawbList.ItemsSource = new ObservableCollection<MawbModel>(
            _mawbs.Where(item => item.Number.Contains(query, StringComparison.OrdinalIgnoreCase)));
        UpdateMawbState();
    }

    private void MawbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
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
        MawbList.ItemsSource = _mawbs;
        UpdateMawbState();
    }

    private void LoadHawb_Click(object sender, RoutedEventArgs e)
    {
        if (MawbList.SelectedItem is MawbModel)
            MawbList_SelectionChanged(MawbList, null!);
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
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
