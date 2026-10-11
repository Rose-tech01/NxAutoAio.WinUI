using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NxAutoAio.WinUI.Models;

namespace NxAutoAio.WinUI.Views;

public sealed partial class DeclarationPage : Page
{
    private readonly ObservableCollection<PrintDocumentModel> _allDocuments = new()
    {
        new() { Index = 1, FileName = "declaration_738_12345670.xlsx", Mawb = "738-12345670", Hawb = "HWB-001", FileFormat = "Excel", PageCount = 4, BarcodeStatus = "Có", PrintStatus = "Chưa xử lý", LastUpdated = "Vừa xong" },
        new() { Index = 2, FileName = "declaration_738_12345681.pdf", Mawb = "738-12345681", Hawb = "HWB-002", FileFormat = "PDF", PageCount = 6, BarcodeStatus = "Có", PrintStatus = "Đã in", LastUpdated = "2 phút trước" },
        new() { Index = 3, FileName = "declaration_738_12345692.xlsx", Mawb = "738-12345692", Hawb = "HWB-003", FileFormat = "Excel", PageCount = 3, BarcodeStatus = "Thiếu", PrintStatus = "Thiếu mã vạch", LastUpdated = "5 phút trước" },
        new() { Index = 4, FileName = "declaration_738_12345703.pdf", Mawb = "738-12345703", Hawb = "HWB-004", FileFormat = "PDF", PageCount = 8, BarcodeStatus = "Có", PrintStatus = "Đang xử lý", LastUpdated = "8 phút trước" },
        new() { Index = 5, FileName = "declaration_738_12345714.xlsx", Mawb = "738-12345714", Hawb = "HWB-005", FileFormat = "Excel", PageCount = 5, BarcodeStatus = "Có", PrintStatus = "Chưa xử lý", LastUpdated = "10 phút trước" },
        new() { Index = 6, FileName = "declaration_738_12345725.pdf", Mawb = "738-12345725", Hawb = "HWB-006", FileFormat = "PDF", PageCount = 2, BarcodeStatus = "Có", PrintStatus = "Đã in", LastUpdated = "12 phút trước" },
        new() { Index = 7, FileName = "declaration_738_12345736.xls", Mawb = "738-12345736", Hawb = "HWB-007", FileFormat = "Excel", PageCount = 4, BarcodeStatus = "Lỗi", PrintStatus = "Lỗi", LastUpdated = "15 phút trước", ErrorMessage = "Sai định dạng file" },
        new() { Index = 8, FileName = "declaration_738_12345747.pdf", Mawb = "738-12345747", Hawb = "HWB-008", FileFormat = "PDF", PageCount = 7, BarcodeStatus = "Có", PrintStatus = "Chưa xử lý", LastUpdated = "18 phút trước" },
        new() { Index = 9, FileName = "declaration_738_12345758.xlsx", Mawb = "738-12345758", Hawb = "HWB-009", FileFormat = "Excel", PageCount = 3, BarcodeStatus = "Có", PrintStatus = "Chưa xử lý", LastUpdated = "20 phút trước" },
        new() { Index = 10, FileName = "declaration_738_12345769.pdf", Mawb = "738-12345769", Hawb = "HWB-010", FileFormat = "PDF", PageCount = 5, BarcodeStatus = "Không tìm thấy", PrintStatus = "Lỗi", LastUpdated = "25 phút trước", ErrorMessage = "Không tìm thấy file" }
    };

    public DeclarationPage()
    {
        InitializeComponent();
        ApplyFilter();
        Loaded += (_, _) => UpdateLayout(PageRoot.ActualWidth);
    }

    private void PageRoot_SizeChanged(object sender, SizeChangedEventArgs e) => UpdateLayout(e.NewSize.Width);

    private void UpdateLayout(double width)
    {
        var narrow = width < 900;
        ContentGrid.ColumnDefinitions.Clear();
        ContentGrid.RowDefinitions.Clear();
        FilterGrid.RowDefinitions.Clear();
        FilterGrid.ColumnDefinitions.Clear();

        if (narrow)
        {
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ContentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            ContentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetColumn(DocumentCard, 0); Grid.SetRow(DocumentCard, 0);
            Grid.SetColumn(OptionsCard, 0); Grid.SetRow(OptionsCard, 1);
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(125) });
            Grid.SetRow(SearchBox, 0); Grid.SetColumn(SearchBox, 0); Grid.SetColumnSpan(SearchBox, 2);
            Grid.SetRow(StatusFilter, 1); Grid.SetColumn(StatusFilter, 0);
            Grid.SetRow(ResultStatus, 1); Grid.SetColumn(ResultStatus, 1);
        }
        else
        {
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            ContentGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(DocumentCard, 0); Grid.SetRow(DocumentCard, 0);
            Grid.SetColumn(OptionsCard, 1); Grid.SetRow(OptionsCard, 0);
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(190) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(125) });
            Grid.SetRow(SearchBox, 0); Grid.SetColumn(SearchBox, 0);
            Grid.SetRow(StatusFilter, 0); Grid.SetColumn(StatusFilter, 1);
            Grid.SetRow(ResultStatus, 0); Grid.SetColumn(ResultStatus, 2);
        }
    }

    private void ChooseFolder_Click(object sender, RoutedEventArgs e)
    {
        FolderStatus.Text = @"C:\Logistics\ToKhai\2026-10-11";
        ToolTipService.SetToolTip(FolderStatus, FolderStatus.Text);
        ResultStatus.Text = $"{_allDocuments.Count} file";
    }

    private void OpenFolder_Click(object sender, RoutedEventArgs e) => StatusText.Text = "Đã mô phỏng mở thư mục";
    private void Refresh_Click(object sender, RoutedEventArgs e) { FolderStatus.Text = "Đã quét lại dữ liệu mẫu"; ApplyFilter(); }
    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();
    private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();

    private void ApplyFilter()
    {
        if (DocumentList is null) return;
        var query = SearchBox?.Text.Trim() ?? string.Empty;
        var status = (StatusFilter?.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "All";
        var items = _allDocuments.Where(item =>
            (string.IsNullOrWhiteSpace(query) || item.FileName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
             item.Mawb.Contains(query, StringComparison.OrdinalIgnoreCase) || item.Hawb.Contains(query, StringComparison.OrdinalIgnoreCase)) &&
            (status == "All" || item.PrintStatus == status || (status == "Lỗi" && item.PrintStatus == "Lỗi")));
        var result = new ObservableCollection<PrintDocumentModel>(items);
        DocumentList.ItemsSource = result;
        EmptyState.Visibility = result.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        ResultStatus.Text = $"{result.Count} tài liệu";
        UpdateSummary();
    }

    private void SelectAll_Click(object sender, RoutedEventArgs e) { foreach (var item in _allDocuments) item.IsSelected = true; UpdateSummary(); }
    private void ClearSelection_Click(object sender, RoutedEventArgs e) { foreach (var item in _allDocuments) item.IsSelected = false; UpdateSummary(); }
    private void StartPrint_Click(object sender, RoutedEventArgs e)
    {
        var selected = _allDocuments.Count(item => item.IsSelected);
        if (selected == 0) { StatusText.Text = "Hãy chọn ít nhất một tài liệu"; return; }
        StatusText.Text = $"Đang mô phỏng in {selected} tài liệu";
        PrintProgress.Value = 35;
        StartButton.IsEnabled = false;
        SuccessText.Text = "Thành công: 0";
        ErrorText.Text = "Lỗi: 0";
    }
    private void UpdateSummary() => SummaryText.Text = $"Đã chọn: {_allDocuments.Count(item => item.IsSelected)}";
}
