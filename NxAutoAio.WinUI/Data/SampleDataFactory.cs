using System.Collections.ObjectModel;
using NxAutoAio.WinUI.Constants;
using NxAutoAio.WinUI.Models;

namespace NxAutoAio.WinUI.Data;

/// <summary>
/// Tạo dữ liệu mẫu cho giao diện trong giai đoạn chưa kết nối backend.
/// </summary>
internal static class SampleDataFactory
{
    public static ObservableCollection<MawbModel> CreateTcsMawbs() =>
        new()
        {
            new() { Index = 1, Number = "738-12345670", HawbCount = 4, TcsStatus = StatusValues.Cleared, Fee = "850.000 đ", PrintStatus = "Sẵn sàng" },
            new() { Index = 2, Number = "738-12345681", HawbCount = 8, TcsStatus = StatusValues.Processing, Fee = "Chưa tra", PrintStatus = "Chờ in" },
            new() { Index = 3, Number = "738-12345692", HawbCount = 2, TcsStatus = StatusValues.Cleared, Fee = "420.000 đ", PrintStatus = "Sẵn sàng" },
            new() { Index = 4, Number = "738-12345703", HawbCount = 0, TcsStatus = StatusValues.NeedsReview, Fee = "Lỗi dữ liệu", PrintStatus = "Chưa in" },
            new() { Index = 5, Number = "738-12345714", HawbCount = 6, TcsStatus = StatusValues.Processing, Fee = "Chưa tra", PrintStatus = "Chờ in" },
            new() { Index = 6, Number = "738-12345725", HawbCount = 3, TcsStatus = StatusValues.Cleared, Fee = "610.000 đ", PrintStatus = StatusValues.Printed }
        };

    public static ObservableCollection<MawbModel> CreateAs400Mawbs() =>
        new()
        {
            new() { Number = "738-12345670", Destination = "HAN - Hà Nội", Packages = 12 },
            new() { Number = "738-12345681", Destination = "SGN - Hồ Chí Minh", Packages = 8 },
            new() { Number = "738-12345692", Destination = "DAD - Đà Nẵng", Packages = 21 },
            new() { Number = "738-12345703", Destination = "HAN - Hà Nội", Packages = 5 },
            new() { Number = "738-12345714", Destination = "SGN - Hồ Chí Minh", Packages = 16 }
        };

    public static ObservableCollection<HawbModel> CreateAs400Hawbs() =>
        new()
        {
            new() { Number = "HWB-001", Consignee = "Công ty Minh Long", Packages = 4 },
            new() { Number = "HWB-002", Consignee = "Công ty Á Châu", Packages = 8 },
            new() { Number = "HWB-003", Consignee = "Công ty Tân Tiến", Packages = 3 }
        };

    public static ObservableCollection<PrintDocumentModel> CreateDeclarationDocuments() =>
        new()
        {
            new() { Index = 1, FileName = "declaration_738_12345670.xlsx", Mawb = "738-12345670", Hawb = "HWB-001", FileFormat = "Excel", PageCount = 4, BarcodeStatus = "Có", PrintStatus = StatusValues.NotProcessed, LastUpdated = "Vừa xong" },
            new() { Index = 2, FileName = "declaration_738_12345681.pdf", Mawb = "738-12345681", Hawb = "HWB-002", FileFormat = "PDF", PageCount = 6, BarcodeStatus = "Có", PrintStatus = StatusValues.Printed, LastUpdated = "2 phút trước" },
            new() { Index = 3, FileName = "declaration_738_12345692.xlsx", Mawb = "738-12345692", Hawb = "HWB-003", FileFormat = "Excel", PageCount = 3, BarcodeStatus = "Thiếu", PrintStatus = StatusValues.MissingBarcode, LastUpdated = "5 phút trước" },
            new() { Index = 4, FileName = "declaration_738_12345703.pdf", Mawb = "738-12345703", Hawb = "HWB-004", FileFormat = "PDF", PageCount = 8, BarcodeStatus = "Có", PrintStatus = StatusValues.Processing, LastUpdated = "8 phút trước" },
            new() { Index = 5, FileName = "declaration_738_12345714.xlsx", Mawb = "738-12345714", Hawb = "HWB-005", FileFormat = "Excel", PageCount = 5, BarcodeStatus = "Có", PrintStatus = StatusValues.NotProcessed, LastUpdated = "10 phút trước" },
            new() { Index = 6, FileName = "declaration_738_12345725.pdf", Mawb = "738-12345725", Hawb = "HWB-006", FileFormat = "PDF", PageCount = 2, BarcodeStatus = "Có", PrintStatus = StatusValues.Printed, LastUpdated = "12 phút trước" },
            new() { Index = 7, FileName = "declaration_738_12345736.xls", Mawb = "738-12345736", Hawb = "HWB-007", FileFormat = "Excel", PageCount = 4, BarcodeStatus = "Lỗi", PrintStatus = StatusValues.PrintError, LastUpdated = "15 phút trước", ErrorMessage = "Sai định dạng file" },
            new() { Index = 8, FileName = "declaration_738_12345747.pdf", Mawb = "738-12345747", Hawb = "HWB-008", FileFormat = "PDF", PageCount = 7, BarcodeStatus = "Có", PrintStatus = StatusValues.NotProcessed, LastUpdated = "18 phút trước" },
            new() { Index = 9, FileName = "declaration_738_12345758.xlsx", Mawb = "738-12345758", Hawb = "HWB-009", FileFormat = "Excel", PageCount = 3, BarcodeStatus = "Có", PrintStatus = StatusValues.NotProcessed, LastUpdated = "20 phút trước" },
            new() { Index = 10, FileName = "declaration_738_12345769.pdf", Mawb = "738-12345769", Hawb = "HWB-010", FileFormat = "PDF", PageCount = 5, BarcodeStatus = "Không tìm thấy", PrintStatus = StatusValues.PrintError, LastUpdated = "25 phút trước", ErrorMessage = "Không tìm thấy file" }
        };
}
