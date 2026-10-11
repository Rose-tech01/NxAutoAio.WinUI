using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace NxAutoAio.WinUI.Helpers;

/// <summary>
/// Các thao tác bố cục Grid dùng chung cho những trang có giao diện responsive.
/// </summary>
internal static class GridLayoutHelper
{
    public static void SetLayout(Grid grid, int columns, int rows)
    {
        grid.ColumnDefinitions.Clear();
        for (var i = 0; i < columns; i++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
        }

        grid.RowDefinitions.Clear();
        for (var i = 0; i < rows; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });
        }
    }

    public static void SetPosition(FrameworkElement element, int row, int column)
    {
        Grid.SetRow(element, row);
        Grid.SetColumn(element, column);
        Grid.SetColumnSpan(element, 1);
    }

    public static void StretchButton(Button button, int row, int column)
    {
        SetPosition(button, row, column);
        button.HorizontalAlignment = HorizontalAlignment.Stretch;
    }
}
