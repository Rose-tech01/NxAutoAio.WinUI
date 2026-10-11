using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NxAutoAio.WinUI.Models;

public sealed class PrintDocumentModel : INotifyPropertyChanged
{
    private bool _isSelected;

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

    public int Index { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string Mawb { get; init; } = string.Empty;
    public string Hawb { get; init; } = string.Empty;
    public string FileFormat { get; init; } = string.Empty;
    public int PageCount { get; init; }
    public string BarcodeStatus { get; init; } = string.Empty;
    public string PrintStatus { get; init; } = string.Empty;
    public string ErrorMessage { get; init; } = string.Empty;
    public string LastUpdated { get; init; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;
}
