using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace AxisAvaloniaApp.UserControls.Extensions
{
    public partial class AxisDataGridTextColumn : DataGridTextColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisDataGridTextColumn"/> class.
        /// </summary>
        public AxisDataGridTextColumn()
        {
            InitializeComponent();
        }

        public static readonly StyledProperty<string> CellTextProperty =
            AvaloniaProperty.Register<AxisDataGridTextColumn, string>(nameof(CellText), string.Empty);

        /// <summary>
        /// Gets or sets text of a cell.
        /// </summary>
        /// <date>21.06.2022.</date>
        public string CellText
        {
            get => GetValue(CellTextProperty);
            set => SetValue(CellTextProperty, value);
        }

        /// <summary>
        /// Occurs when cell began to edit.
        /// </summary>
        /// <date>21.06.2022.</date>
        public event EventHandler<DataGridBeginningEditEventArgs> CellEditBegan;

        /// <summary>
        /// Occurs when key down into the cell.
        /// </summary>
        /// <date>21.06.2022.</date>
        public event EventHandler<KeyEventArgs> CellKeyDown;

        /// <summary>
        /// Occurs when text in the cell is changed.
        /// </summary>
        /// <date>21.06.2022.</date>
        public event EventHandler<TextInputEventArgs> CellTextChanged;

        /// <summary>
        /// Called when the cell in the column enters editing mode.
        /// </summary>
        /// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
        /// <param name="editingEventArgs">Information about the user gesture that is causing a cell to enter editing mode.</param>
        /// <returns>The unedited value. </returns>
        /// <date>21.06.2022.</date>
        protected override object PrepareCellForEdit(IControl editingElement, RoutedEventArgs editingEventArgs)
        {
            DataGridRow gridRow = null;
            if (editingEventArgs != null && editingEventArgs.Source is DataGridCell gridCell && gridCell.Parent != null)
            {
                if (gridCell.Parent is DataGridCellsPresenter cellsPresenter && cellsPresenter.Parent != null)
                {
                    if (cellsPresenter.Parent is DataGridFrozenGrid frozenGrid && frozenGrid.Parent != null)
                    {
                        if (frozenGrid.Parent is DataGridRow row)
                        {
                            gridRow = row;
                        }
                    }
                }
            }

            if (editingElement is TextBox textBox)
            {
                textBox.KeyUp += CellTextBox_KeyUp;
                textBox.KeyDown += CellTextBox_KeyDown;
                textBox.DetachedFromVisualTree += CellTextBox_DetachedFromVisualTree;
            }

            CellEditBegan?.Invoke(editingElement, new DataGridBeginningEditEventArgs(this, gridRow, editingEventArgs));

            System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(100);
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    editingElement.Focus();
                });
            });

            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }

        /// <summary>
        /// Invokes CellKeyDown event when key down into TextBox.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">KeyEventArgs</param>
        /// <date>21.06.2022.</date>
        private void CellTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            CellKeyDown?.Invoke(sender, e);
        }

        /// <summary>
        /// Unsubscribes from events when TextBox is detached from visual tree.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>21.06.2022.</date>
        private void CellTextBox_DetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                CellText = string.Empty;
                textBox.KeyDown -= CellTextBox_KeyDown;
                textBox.KeyUp -= CellTextBox_KeyUp;
                textBox.DetachedFromVisualTree -= CellTextBox_DetachedFromVisualTree;
            }
        }

        /// <summary>
        /// Sets CellText property and invoke CellTextChanged event when key up into TextBox.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">KeyEventArgs</param>
        /// <date>21.06.2022.</date>
        private void CellTextBox_KeyUp(object? sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                CellText = textBox.Text;

                CellTextChanged?.Invoke(
                    sender, 
                    new TextInputEventArgs()
                    {
                        Text = textBox.Text,
                    });
            }
        }

        /// <summary>
        /// Initialize component.
        /// </summary>
        /// <date>21.06.2022.</date>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
