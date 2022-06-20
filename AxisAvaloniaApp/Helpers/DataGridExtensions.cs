using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Helpers
{
    public static class DataGridExtensions
    {
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

    }
}
