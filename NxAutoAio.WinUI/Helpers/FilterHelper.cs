using System;
using System.Collections.Generic;
using System.Linq;
using NxAutoAio.WinUI.Constants;
using NxAutoAio.WinUI.Models;

namespace NxAutoAio.WinUI.Helpers;

internal static class FilterHelper
{
    public static IEnumerable<MawbModel> FilterMawbs(
        IEnumerable<MawbModel> items,
        string? query,
        string? status)
    {
        var normalizedQuery = query?.Trim() ?? string.Empty;
        var normalizedStatus = string.IsNullOrWhiteSpace(status) ? StatusValues.All : status;

        return items.Where(item =>
            (string.IsNullOrWhiteSpace(normalizedQuery) ||
             item.Number.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)) &&
            (normalizedStatus == StatusValues.All || item.TcsStatus == normalizedStatus));
    }

    public static IEnumerable<PrintDocumentModel> FilterDocuments(
        IEnumerable<PrintDocumentModel> items,
        string? query,
        string? status)
    {
        var normalizedQuery = query?.Trim() ?? string.Empty;
        var normalizedStatus = string.IsNullOrWhiteSpace(status) ? StatusValues.All : status;

        return items.Where(item =>
            (string.IsNullOrWhiteSpace(normalizedQuery) ||
             item.FileName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) ||
             item.Mawb.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) ||
             item.Hawb.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)) &&
            (normalizedStatus == StatusValues.All ||
             item.PrintStatus == normalizedStatus ||
             (normalizedStatus == StatusValues.PrintError && item.PrintStatus == StatusValues.PrintError)));
    }
}
