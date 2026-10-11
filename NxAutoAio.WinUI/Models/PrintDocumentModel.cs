using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NxAutoAio.WinUI.Models;

/// <summary>
/// Đại diện cho một tệp tờ khai trong danh sách kiểm tra và xử lý in.
/// </summary>
public sealed class PrintDocumentModel : INotifyPropertyChanged
{
    private bool _isSelected;

    // IsSelected được Binding với CheckBox trong bảng.
    // Không nên cập nhật trực tiếp UI từ luồng C++; chỉ thay đổi model trên UI thread.
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
        }
    }

    // Thông tin nhận diện tài liệu, vận đơn và định dạng tệp.
    public int Index { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string Mawb { get; init; } = string.Empty;
    public string Hawb { get; init; } = string.Empty;
    public string FileFormat { get; init; } = string.Empty;
    public int PageCount { get; init; }
    // Kết quả kiểm tra mã vạch, trạng thái in, lỗi gần nhất và thời điểm cập nhật.
    public string BarcodeStatus { get; init; } = string.Empty;
    public string PrintStatus { get; init; } = string.Empty;
    public string ErrorMessage { get; init; } = string.Empty;
    public string LastUpdated { get; init; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;
}
