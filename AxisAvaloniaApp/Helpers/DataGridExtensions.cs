using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxisAvaloniaApp.Helpers
{
    public static class DataGridExtensions
    {
        /// <summary>
        /// Gets current row.
        /// </summary>
        /// <param name="dataGrid">DataGrid to get row.</param>
        /// <returns>Returns current row if it exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        public static DataGridRow? GetCurrentRow(this DataGrid dataGrid)
        {
            DataGridRowsPresenter rowsPresenter = dataGrid.FindDescendantOfType<DataGridRowsPresenter>();
            if (rowsPresenter != null)
            {
                IEnumerable<DataGridRow> rows = rowsPresenter.Children.OfType<DataGridRow>();

                if (rows != null)
                {
                    return rows.FirstOrDefault(r => r.FindDescendantOfType<DataGridCellsPresenter>().Children.Any(p => p.Classes.Contains(":current")));
                }
            }

            return null;
        }

        /// <summary>
        /// Gets row by index.
        /// </summary>
        /// <param name="dataGrid">DataGrid to get row.</param>
        /// <param name="rowIndex">Index to get row.</param>
        /// <returns>Returns row if it exists; otherwise throw IndexOutOfRangeException.</returns>
        /// <date>20.06.2022.</date>
        public static DataGridRow GetRow(this DataGrid dataGrid, int rowIndex)
        {
            DataGridRowsPresenter rowsPresenter = dataGrid.FindDescendantOfType<DataGridRowsPresenter>();

            if (rowIndex > -1 && rowIndex < rowsPresenter.Children.Count)
            {
                return (DataGridRow)rowsPresenter.Children[rowIndex];
            }

            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Gets last row of DataGrid.
        /// </summary>
        /// <param name="dataGrid">DataGrid to get row.</param>
        /// <returns>Returns last row if it exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        public static DataGridRow? GetLastRow(this DataGrid dataGrid)
        {
            DataGridRowsPresenter rowsPresenter = dataGrid.FindDescendantOfType<DataGridRowsPresenter>();

            if (rowsPresenter != null && rowsPresenter.Children.Count > 0)
            {
                return (DataGridRow)rowsPresenter.Children[rowsPresenter.Children.Count - 1];
            }

            return null;
        }

        /// <summary>
        /// Gets rows of DataGrid.
        /// </summary>
        /// <param name="dataGrid">DataGrid to get rows.</param>
        /// <returns>Returns list with rows.</returns>
        /// <date>21.06.2022.</date>
        public static IList<DataGridRow> GetRows(this DataGrid dataGrid)
        {
            IList<DataGridRow> rows = new List<DataGridRow>();
            DataGridRowsPresenter rowsPresenter = dataGrid.FindDescendantOfType<DataGridRowsPresenter>();

            if (rowsPresenter != null && rowsPresenter.Children.Count > 0)
            {
                foreach (DataGridRow row in rowsPresenter.Children)
                {
                    rows.Add(row);
                }
            }

            return rows;
        }

        /// <summary>
        /// Gets current cell of the DataGrid.
        /// </summary>
        /// <param name="dataGrid">DataGrid to get cell.</param>
        /// <returns>Returns current cell if it exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        public static DataGridCell? GetCurrentCell(this DataGrid dataGrid)
        {
            DataGridRow? row = dataGrid.GetCurrentRow();

            if (row != null)
            {
                DataGridCellsPresenter cellsPresenter = row.FindDescendantOfType<DataGridCellsPresenter>();

                if (cellsPresenter != null)
                {
                    return cellsPresenter.Children.OfType<DataGridCell>().FirstOrDefault(p => p.Classes.Contains(":current"));
                }
            }

            return null;
        }

        /// <summary>
        /// Gets cell of DataGrid by row and index of column.
        /// </summary>
        /// <param name="dataGrid">DataGrid to get cell.</param>
        /// <param name="row">Row to get cell.</param>
        /// <param name="columnIndex">Index of column to get cell.</param>
        /// <returns>Returns cell if it exists; otherwise returns null.</returns>
        /// <exception cref="IndexOutOfRangeException">Throws exception if ColumnIndex is out of range.</exception>
        /// <date>20.06.2022.</date>
        public static DataGridCell? GetCell(this DataGrid dataGrid, DataGridRow row, int columnIndex)
        {
            if (columnIndex > -1 && columnIndex < dataGrid.Columns.Count)
            {
                DataGridCellsPresenter cellsPresenter = row.FindDescendantOfType<DataGridCellsPresenter>();

                if (cellsPresenter != null && cellsPresenter.Children.Count > 0)
                {
                    IEnumerable<DataGridCell> cellsList = cellsPresenter.Children.OfType<DataGridCell>();
                    return cellsList.ElementAt(columnIndex);
                }

                return null;
            }

            throw new IndexOutOfRangeException("Column index is out of range!");
        }

        /// <summary>
        /// Gets cell of DataGrid by indexes of column and row.
        /// </summary>
        /// <param name="dataGrid">DataGrid to get cell.</param>
        /// <param name="columnIndex">Index of column to get cell.</param>
        /// <param name="rowIndex">Index of row to get cell.</param>
        /// <returns>Returns cell if it exists; otherwise throw IndexOutOfRangeException.</returns>
        /// <date>20.06.2022.</date>
        public static DataGridCell GetCell(this DataGrid dataGrid, int columnIndex, int rowIndex)
        {
            if (columnIndex > -1 && columnIndex < dataGrid.Columns.Count)
            {
                IEnumerable<DataGridCell> cellsList = dataGrid.
                    GetRow(rowIndex).
                    FindDescendantOfType<DataGridCellsPresenter>().
                    Children.
                    OfType<DataGridCell>();

                return cellsList.ElementAt(columnIndex);
            }

            throw new IndexOutOfRangeException("Column index is out of range!");
        }

        /// <summary>
        /// Activates cell to edit.
        /// </summary>
        /// <param name="gridCell">DataGridCell to activate.</param>
        /// <date>20.06.2022.</date>
        public static void BeginCellEdit(this DataGridCell gridCell)
        {
            var cellMethod = gridCell.
                GetType().
                GetMethod("DataGridCell_PointerPressed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (cellMethod != null && gridCell.GetVisualRoot() is Window root)
            {
                double x = 0;
                double y = 0;
                if (gridCell.TransformedBounds != null)
                {
                    System.Reflection.PropertyInfo res = gridCell.TransformedBounds.GetType().GetProperty("Clip");
                    Rect clip = (Rect)res.GetValue(gridCell.TransformedBounds);
                    x = root.Position.X + (clip.Position.X * gridCell.Bounds.Left);
                    y = root.Position.Y + (clip.Position.Y * gridCell.Bounds.Top);
                }
                else
                {
                    x = root.Position.X;
                    y = root.Position.Y;
                }
                
                PointerPressedEventArgs pointerPressed = new PointerPressedEventArgs(
                    gridCell,
                    new Pointer(0, PointerType.Mouse, true),
                    gridCell.GetVisualRoot(),
                    new Point(x, y),
                    (ulong)DateTime.Now.Ticks,
                    new PointerPointProperties(RawInputModifiers.LeftMouseButton, PointerUpdateKind.LeftButtonPressed),
                    KeyModifiers.Control);

                cellMethod.Invoke(gridCell, new object[] { pointerPressed });
            }
        }
    }
}
