using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Serialization;
using AxisAvaloniaApp.UserControls.Extensions;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;

namespace AxisAvaloniaApp.Views
{
    public partial class SaleView : UserControl
    {
        DataGrid saleGrid;
        ContextMenu saleContextMenu;
        TreeView itemsGroupsTreeView;
        TreeView partnersGroupsTreeView;
        private readonly ISerializationService serializationSale;

        public SaleView()
        {
            InitializeComponent();

            IsPaymentPanelVisible = false;
            IsPayInCashPanelVisible = false;
            IsEditPanelVisible = false;

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.SaleViewModel>();
            (this.DataContext as ViewModels.ViewModelBase).ViewClosing += SerializeVisualData;

            serializationSale = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            serializationSale.InitSerializationData(Enums.ESerializationGroups.Sale);
            saleGrid = this.FindControl<DataGrid>("SaleGrid");
            foreach (var column in saleGrid.Columns)
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

            itemsGroupsTreeView = this.FindControl<TreeView>("ItemsGroupsTreeView");
            partnersGroupsTreeView = this.FindControl<TreeView>("PartnersGroupsTreeView");

            saleContextMenu = this.FindControl<ContextMenu>("SaleContextMenu");
            if (saleContextMenu != null)
            {
                foreach(CheckedMenuItem item in saleContextMenu.Items)
                {
                    if (item.Tag != null && item.Tag is EAdditionalSaleTableColumns column)
                    {
                        item.IsChecked = ((EAdditionalSaleTableColumns)serializationSale[ESerializationKeys.AddColumns] & column) > 0;
                    }                    
                }
            }

            TextBox textBox = this.FindControl<TextBox>("TextBoxPartner");
            if (textBox != null)
            {
                textBox.AttachedToVisualTree += (s, e) => textBox.Focus();
            }

            TextBox textBoxTaxNumber = this.FindControl<TextBox>("TextBoxTaxNumber");
            if (textBoxTaxNumber != null)
            {
                textBoxTaxNumber.KeyDown += TextBoxFindPartner_KeyDown;
            }

            TextBox textBoxVATNumber = this.FindControl<TextBox>("TextBoxVATNumber");
            if (textBoxVATNumber != null)
            {
                textBoxVATNumber.KeyDown += TextBoxFindPartner_KeyDown;
            }
        }

        private void SerializeVisualData(string viewId)
        {
            (this.DataContext as ViewModels.ViewModelBase).ViewClosing -= SerializeVisualData;

            EAdditionalSaleTableColumns tableColumns = 0;

            if (saleContextMenu != null)
            {
                foreach (CheckedMenuItem item in saleContextMenu.Items)
                {
                    if (item.IsChecked && item.Tag != null && item.Tag is EAdditionalSaleTableColumns column)
                    {
                        tableColumns = tableColumns | column;
                    }
                }
            }
            serializationSale[ESerializationKeys.AddColumns].Value = ((int)tableColumns).ToString();

            serializationSale.Update();

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
        /// Gets or sets a value indicating whether panel with buttons to edit nomenclaturs is visible.
        /// </summary>
        /// <date>31.05.2022.</date>
        public bool IsEditPanelVisible
        {
            get => GetValue(IsEditPanelVisibleProperty);
            set => SetValue(IsEditPanelVisibleProperty, value);
        }

        public static readonly StyledProperty<ENomenclatures> ActiveNomenclatureProperty =
           AvaloniaProperty.Register<SaleView, ENomenclatures>(nameof(ActiveNomenclature));

        /// <summary>
        /// Gets or sets an active nomanclature.
        /// </summary>
        /// <date>31.05.2022.</date>
        public ENomenclatures ActiveNomenclature
        {
            get => GetValue(ActiveNomenclatureProperty);
            set => SetValue(ActiveNomenclatureProperty, value);
        }

        public static readonly StyledProperty<bool> IsEditNomenclaturePanelVisibleProperty =
           AvaloniaProperty.Register<SaleView, bool>(nameof(IsEditNomenclaturePanelVisible));

        /// <summary>
        /// Gets or sets a value indicating whether panel to edit nomenclaturs is visible.
        /// </summary>
        /// <date>31.05.2022.</date>
        public bool IsEditNomenclaturePanelVisible
        {
            get => GetValue(IsEditNomenclaturePanelVisibleProperty);
            set => SetValue(IsEditNomenclaturePanelVisibleProperty, value);
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
                        foreach (OperationItemModel item in dataGrid.Items)
                        {
                            if (!item.Name.Contains(SearchDataGridRows))
                            {
                                dataGrid.SelectedItems.Remove(item);
                            }
                        }
                    }
                    break;
            }
            base.OnPropertyChanged(change);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            TextBox textBox = this.FindControl<TextBox>("TextBoxPartner");
            if (textBox != null)
            {
                textBox.Focus();
            }
        }

        public override void EndInit()
        {
            base.EndInit();

            //TextBox textBox = this.FindControl<TextBox>("TextBoxPartner");
            //if (textBox != null)
            //{
            //    textBox.Focus();
            //}
        }

        private void ChangeIsPaymentPanelVisible()
        {
            IsPaymentPanelVisible = !IsPaymentPanelVisible;
        }

        private void ChangeIsPayInCashPanelVisible()
        {
            IsPayInCashPanelVisible = !IsPayInCashPanelVisible;
        }

        private void ButtonEditNomenclature_Click(object? sender, RoutedEventArgs e)
        {
            IsEditPanelVisible = !IsEditPanelVisible;
        }
        //private void ChangeIsEditPanelVisible()
        //{
        //    IsEditPanelVisible = !IsEditPanelVisible;
        //}

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
                    if (sender is TextBox textBox && textBox.IsFocused)
                    {
                        ActiveNomenclature = ENomenclatures.Partners;
                    }
                    break;
            }
        }

        private void DataGridSale_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case (nameof(DataGrid.IsFocused)):
                    if (sender is DataGrid dataGrid && dataGrid.IsFocused)
                    {
                        ActiveNomenclature = ENomenclatures.Items;
                    }
                    break;
            }
        }

        private void ItemsGroupsTreeView_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            { 
                case nameof(TreeView.SelectedItem):
                    ActiveNomenclature = ENomenclatures.ItemsGroups;
                    break;
            }
        }

        private void PartnersGroupsTreeView_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case nameof(TreeView.SelectedItem):
                    ActiveNomenclature = ENomenclatures.PartnersGroups;
                    break;
            }
        }

        private void EditNomenclatureButton_Click(object? sender, RoutedEventArgs e)
        {
            IsEditNomenclaturePanelVisible = true;
        }

        private void TextBoxFindPartner_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (sender is TextBox textBox)
                    {
                        (DataContext as ViewModels.SaleViewModel).FindPartner(textBox.Text);
                    }
                    break;
            }
        }

        private void TextBoxDiscountCardNumber_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case nameof(TextBox.IsFocused):
                    if (sender is TextBox tb)
                    {
                        switch (tb.IsFocused)
                        {
                            case true:
                                // подписаться на сканер
                                break;
                            case false:
                                // отписаться от сканера
                                break;
                        }
                    }
                    break;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
