using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NxAutoAio.WinUI.Constants;
using NxAutoAio.WinUI.Data;
using NxAutoAio.WinUI.Helpers;
using NxAutoAio.WinUI.Models;

namespace NxAutoAio.WinUI.Views;

/// <summary>
/// Tìm, lọc và chọn các tệp tờ khai để chuẩn bị xử lý/in.
/// Dữ liệu và thao tác thư mục/in hiện đang được mô phỏng.
/// </summary>
public sealed partial class DeclarationPage : Page
{
    // Danh sách mẫu thay cho kết quả quét thư mục; trạng thái dùng để thử bộ lọc và giao diện.
    private readonly ObservableCollection<PrintDocumentModel> _allDocuments =
        SampleDataFactory.CreateDeclarationDocuments();

    public DeclarationPage()
    {
        InitializeComponent();
        // Áp dụng bộ lọc mặc định và bố cục thích ứng sau khi trang được tải.
        ApplyFilter();
        Loaded += (_, _) => UpdateLayout(PageRoot.ActualWidth);
    }

    private void PageRoot_SizeChanged(object sender, SizeChangedEventArgs e) => UpdateLayout(e.NewSize.Width);

    private void UpdateLayout(double width)
    {
        // Chuyển bảng tài liệu và tùy chọn sang hai hàng khi vùng hiển thị hẹp.
        var narrow = width < ResponsiveBreakpoints.DeclarationNarrow;
        ContentGrid.ColumnDefinitions.Clear();
        ContentGrid.RowDefinitions.Clear();
        FilterGrid.RowDefinitions.Clear();
        FilterGrid.ColumnDefinitions.Clear();

        if (narrow)
        {
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ContentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            ContentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            GridLayoutHelper.SetPosition(DocumentCard, 0, 0);
            GridLayoutHelper.SetPosition(OptionsCard, 1, 0);
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(125) });
            GridLayoutHelper.SetPosition(SearchBox, 0, 0, 2);
            GridLayoutHelper.SetPosition(StatusFilter, 1, 0);
            GridLayoutHelper.SetPosition(ResultStatus, 1, 1);
        }
        else
        {
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });
            ContentGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            GridLayoutHelper.SetPosition(DocumentCard, 0, 0);
            GridLayoutHelper.SetPosition(OptionsCard, 0, 1);
            FilterGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(190) });
            FilterGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(125) });
            GridLayoutHelper.SetPosition(SearchBox, 0, 0);
            GridLayoutHelper.SetPosition(StatusFilter, 0, 1);
            GridLayoutHelper.SetPosition(ResultStatus, 0, 2);
        }
    }

    private void ChooseFolder_Click(object sender, RoutedEventArgs e)
    {
        // Đang hiển thị đường dẫn giả; tích hợp thật cần chọn thư mục, quét tệp và tạo model.
        FolderStatus.Text = @"C:\Logistics\ToKhai\2026-10-11";
        ToolTipService.SetToolTip(FolderStatus, FolderStatus.Text);
        ResultStatus.Text = $"{_allDocuments.Count} file";
    }

    private void OpenFolder_Click(object sender, RoutedEventArgs e)
    {
        // Chưa mở thư mục trên máy; thay phần mô phỏng bằng launcher hoặc native C++.
        StatusText.Text = "Đã mô phỏng mở thư mục";
    }

    // Làm mới dữ liệu mẫu rồi áp dụng lại truy vấn và trạng thái đang chọn.
    private void Refresh_Click(object sender, RoutedEventArgs e) { FolderStatus.Text = "Đã quét lại dữ liệu mẫu"; ApplyFilter(); }
    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();
    private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilter();

    private void ApplyFilter()
    {
        if (DocumentList is null) return;
        // Tìm không phân biệt hoa/thường trên tên tệp, MAWB và HAWB; trạng thái được lọc riêng.
        var query = SearchBox?.Text.Trim() ?? string.Empty;
        var status = (StatusFilter?.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? StatusValues.All;
        var items = FilterHelper.FilterDocuments(_allDocuments, query, status);
        var result = new ObservableCollection<PrintDocumentModel>(items);
        DocumentList.ItemsSource = result;
        EmptyState.Visibility = result.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        ResultStatus.Text = $"{result.Count} tài liệu";
        UpdateSummary();
    }

    // Chọn/bỏ chọn toàn bộ danh sách gốc, kể cả các dòng hiện không khớp bộ lọc.
    private void SelectAll_Click(object sender, RoutedEventArgs e) { PrintWorkflowHelper.SelectAll(_allDocuments, true); UpdateSummary(); }
    private void ClearSelection_Click(object sender, RoutedEventArgs e) { PrintWorkflowHelper.SelectAll(_allDocuments, false); UpdateSummary(); }
    private void StartPrint_Click(object sender, RoutedEventArgs e)
    {
        var selected = PrintWorkflowHelper.CountSelected(_allDocuments);
        if (selected == 0) { StatusText.Text = "Hãy chọn ít nhất một tài liệu"; return; }
        // Hiện chỉ cập nhật trạng thái minh họa; tích hợp thật cần gửi danh sách vào hàng đợi in.
        StatusText.Text = $"Đang mô phỏng in {selected} tài liệu";
        PrintProgress.Value = 35;
        StartButton.IsEnabled = false;
        SuccessText.Text = "Thành công: 0";
        ErrorText.Text = "Lỗi: 0";
    }
    private void UpdateSummary() => SummaryText.Text = $"Đã chọn: {PrintWorkflowHelper.CountSelected(_allDocuments)}";
}
