using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Serialization;
using System;
using System.Collections.Generic;

namespace AxisAvaloniaApp.Views
{
    public partial class SaleView : UserControl
    {
        private readonly ISerializationService serializationSale;

        public SaleView()
        {
            InitializeComponent();

            IsPaymentPanelVisible = false;
            IsPayInCashPanelVisible = false;
            IsEditPanelVisible = false;

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.SaleViewModel>();
            serializationSale = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            serializationSale.InitSerializationData(Enums.ESerializationGroups.Sale);
            DataGrid dataGrid = this.FindControl<DataGrid>("SaleGrid");
            dataGrid.LoadingRow += DataGrid_LoadingRow;
            foreach (var column in dataGrid.Columns)
            {
                switch (column.DisplayIndex)
                {
                    case 0:
                        column.Width = new DataGridLength((double)serializationSale[Enums.ESerializationKeys.ColCodeWidth]);
                        break;
                    case 1:
                        column.Width = new DataGridLength((double)serializationSale[Enums.ESerializationKeys.ColBarcodeWidth]);
                        break;
                    case 3:
                        column.Width = new DataGridLength((double)serializationSale[Enums.ESerializationKeys.ColMeasureWidth]);
                        break;
                    case 4:
                        column.Width = new DataGridLength((double)serializationSale[Enums.ESerializationKeys.ColQuantityWidth]);
                        break;
                    case 5:
                        column.Width = new DataGridLength((double)serializationSale[Enums.ESerializationKeys.ColPriceWidth]);
                        break;
                    case 6:
                        column.Width = new DataGridLength((double)serializationSale[Enums.ESerializationKeys.ColDiscountWidth]);
                        break;
                    case 7:
                        column.Width = new DataGridLength((double)serializationSale[Enums.ESerializationKeys.ColTotalSumWidth]);
                        break;
                    case 8:
                        column.Width = new DataGridLength((double)serializationSale[Enums.ESerializationKeys.ColNoteWidth]);
                        break;
                }
            }

            ContextMenu contextMenu = this.FindResource("SaleContextMenu") as ContextMenu;

            ComboBox comboBox = this.FindControl<ComboBox>("TestBox");
        }

        private List<DataGridRow> rows = new List<DataGridRow>();

        private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            rows.Add(e.Row);
        }

        public static readonly StyledProperty<bool> IsPaymentPanelVisibleProperty =
           AvaloniaProperty.Register<SaleView, bool>(nameof(IsPaymentPanelVisible));

        /// <summary>
        /// Gets or sets a value indicating whether panel to pay is visible.
        /// </summary>
        /// <date>30.05.2022.</date>
        public bool IsPaymentPanelVisible
        {
            get => GetValue(IsPaymentPanelVisibleProperty);
            set => SetValue(IsPaymentPanelVisibleProperty, value);
        }

        public static readonly StyledProperty<bool> IsPayInCashPanelVisibleProperty =
           AvaloniaProperty.Register<SaleView, bool>(nameof(IsPayInCashPanelVisible));

        /// <summary>
        /// Gets or sets a value indicating whether panel to pay in cash is visible.
        /// </summary>
        /// <date>30.05.2022.</date>
        public bool IsPayInCashPanelVisible
        {
            get => GetValue(IsPayInCashPanelVisibleProperty);
            set => SetValue(IsPayInCashPanelVisibleProperty, value);
        }

        public static readonly StyledProperty<string> SearchDataGridRowsProperty =
           AvaloniaProperty.Register<SaleView, string>(nameof(SearchDataGridRows));

        /// <summary>
        /// Gets or sets a value indicating whether panel to pay in cash is visible.
        /// </summary>
        /// <date>30.05.2022.</date>
        public string SearchDataGridRows
        {
            get => GetValue(SearchDataGridRowsProperty);
            set => SetValue(SearchDataGridRowsProperty, value);
        }

        public static readonly StyledProperty<bool> IsEditPanelVisibleProperty =
           AvaloniaProperty.Register<SaleView, bool>(nameof(IsEditPanelVisible));

        /// <summary>
        /// Gets or sets a value indicating whether panel to edit nomenclaturs is visible.
        /// </summary>
        /// <date>31.05.2022.</date>
        public bool IsEditPanelVisible
        {
            get => GetValue(IsEditPanelVisibleProperty);
            set => SetValue(IsEditPanelVisibleProperty, value);
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            switch (change.Property.Name)
            {
                case nameof(SearchDataGridRows):
                    DataGrid dataGrid = this.FindControl<DataGrid>("SaleGrid");
                    if (dataGrid != null)
                    {
                        dataGrid.SelectAll();
                        //dataGrid.SelectedItems.Clear();
                        //dataGrid.InvalidateMeasure();
                        foreach (var item in dataGrid.Items)
                        {
                            if (!(item as OperationItemModel).Name.Contains(SearchDataGridRows))
                            {
                                dataGrid.SelectedItems.Remove(item);
                            }
                        }
                        dataGrid.InvalidateMeasure();

                        foreach (var row in rows)
                        {
                            row.Background = Brushes.Red;
                        }

                    }
                    break;
            }
            base.OnPropertyChanged(change);
        }

        private void ChangeIsPaymentPanelVisible()
        {
            IsPaymentPanelVisible = !IsPaymentPanelVisible;
        }

        private void ChangeIsPayInCashPanelVisible()
        {
            IsPayInCashPanelVisible = !IsPayInCashPanelVisible;
        }

        private void ChangeIsEditPanelVisible()
        {
            IsEditPanelVisible = !IsEditPanelVisible;
        }

        //private void TextBoxPartner_GotFocus(object? sender, GotFocusEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        private void TextBoxTitle_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.Focus();
                    break;
            }
        }

        private void TextBoxTitle_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case nameof(TextBox.IsVisible):
                    if (sender is TextBox textBox && textBox.IsVisible)
                    {
                        textBox.Focus();
                        textBox.CaretIndex = textBox.Text.Length;
                    }
                    break;
                case nameof(TextBox.IsFocused):
                    if (sender is TextBox tb && !tb.IsFocused)
                    {
                        (DataContext as ViewModels.SaleViewModel).IsSaleTitleReadOnly = true;
                    }
                    break;
            }
        }

        private void TextBoxPartner_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case (nameof(TextBox.IsFocused)):
                    break;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
