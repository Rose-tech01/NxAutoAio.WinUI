using System.Collections.Generic;
using System.Linq;
using NxAutoAio.WinUI.Models;

namespace NxAutoAio.WinUI.Helpers;

internal static class PrintWorkflowHelper
{
    public static void SelectAll(IEnumerable<PrintDocumentModel> documents, bool selected)
    {
        foreach (var document in documents)
            document.IsSelected = selected;
    }

    public static int CountSelected(IEnumerable<PrintDocumentModel> documents) =>
        documents.Count(document => document.IsSelected);
}
