using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Scanning;
using AxisAvaloniaApp.UserControls.Extensions;
using AxisAvaloniaApp.ViewModels;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;

namespace AxisAvaloniaApp.Views
{
    public partial class SaleView : UserControl
    {
        private SaleViewModel dataContext;
        private TextBox textBoxPartner;
        private DataGrid saleGrid;
        private ContextMenu saleContextMenu;
        private DataGrid itemsGrid;
        private ContextMenu itemsContextMenu;
        private DataGrid partnersGrid;
        private ContextMenu partnersContextMenu;
        private Dictionary<int, ESerializationKeys> saleDataGridColumns;
        private Dictionary<int, ESerializationKeys> itemsDataGridColumns;
        private Dictionary<int, ESerializationKeys> partnersDataGridColumns;       
        private readonly IScanningData scanningService;
        private readonly ILoggerService loggerService;
        private IControl? inputControl;

        public SaleView()
        {
            InitializeComponent();
            
            dataContext = Splat.Locator.Current.GetRequiredService<SaleViewModel>();
            this.DataContext = dataContext;
            dataContext.ViewClosing += SerializeVisualData;
            dataContext.OrderChanged += DataContext_OrderChanged;
            dataContext.InvalidOrderRecord += DataContext_InvalidOrderRecord;

            scanningService = Splat.Locator.Current.GetRequiredService<IScanningData>();
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();
            InputControl = null;

            saleDataGridColumns = new Dictionary<int, ESerializationKeys>()
            {
                {1, ESerializationKeys.ColCodeWidth},
                {2, ESerializationKeys.ColBarcodeWidth },
                {4, ESerializationKeys.ColMeasureWidth },
                {5, ESerializationKeys.ColQuantityWidth },
                {6, ESerializationKeys.ColPriceWidth },
                {7, ESerializationKeys.ColDiscountWidth },
                {8, ESerializationKeys.ColTotalSumWidth },
                {9, ESerializationKeys.ColNoteWidth },
            };
            itemsDataGridColumns = new Dictionary<int, ESerializationKeys>()
            {
                {1, ESerializationKeys.ColCodeWidth},
                {2, ESerializationKeys.ColBarcodeWidth },
                {3, ESerializationKeys.ColMeasureWidth },
                {4, ESerializationKeys.ColPriceWidth },
                {5, ESerializationKeys.ColVATGroupWidth },
            };
            partnersDataGridColumns = new Dictionary<int, ESerializationKeys>()
            {
                {1, ESerializationKeys.ColPrincipalWidth},
                {2, ESerializationKeys.ColPhoneWidth},
                {3, ESerializationKeys.ColCityWidth},
                {4, ESerializationKeys.ColAddressWidth},
                {5, ESerializationKeys.ColTaxNumberWidth},
                {6, ESerializationKeys.ColVATNumberWidth},
                {7, ESerializationKeys.ColEMailWidth},
                {8, ESerializationKeys.ColDiscountCardWidth},
            };
            
            IsPayInCashPanelVisible = false;
            IsEditPanelVisible = false;

            DeserializeVisualData();
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
        /// Gets or sets a value indicating whether panel with buttons to edit nomenclatures is visible.
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
        /// Gets or sets a value indicating whether panel to edit nomenclatures is visible.
        /// </summary>
        /// <date>31.05.2022.</date>
        public bool IsEditNomenclaturePanelVisible
        {
            get => GetValue(IsEditNomenclaturePanelVisibleProperty);
            set => SetValue(IsEditNomenclaturePanelVisibleProperty, value);
        }

        /// <summary>
        /// Gets or sets control to set scanned data.
        /// </summary>
        /// <date>21.06.2022.</date>
        private IControl? InputControl
        {
            get => inputControl;
            set
            {
                inputControl = value;

                if (inputControl != null)
                {
                    scanningService.SendScannedDataEvent += SetScannedData;
                }
                else
                {
                    scanningService.SendScannedDataEvent -= SetScannedData;
                }
            }
        }

        /// <summary>
        /// Restores visual data.
        /// </summary>
        /// <date>31.05.2022.</date>
        private void DeserializeVisualData()
        {
            textBoxPartner = this.FindControl<TextBox>("TextBoxPartner");

            // восстанавливаем данные основного раздела
            saleGrid = this.FindControl<DataGrid>("SaleGrid");
            foreach (var column in saleGrid.Columns)
            {
                if (saleDataGridColumns.ContainsKey(column.DisplayIndex))
                {
                    column.Width = new DataGridLength((double)dataContext.SerializationService[saleDataGridColumns[column.DisplayIndex]]);
                }
            }
            saleContextMenu = this.FindControl<ContextMenu>("SaleContextMenu");
            if (saleContextMenu != null)
            {
                foreach (CheckedMenuItem item in saleContextMenu.Items)
                {
                    int saleTableColumns = (int)dataContext.SerializationService[ESerializationKeys.AddColumns];
                    if (item.Tag != null && item.Tag is EAdditionalSaleTableColumns column)
                    {
                        item.IsChecked = (saleTableColumns & (int)column) > 0;
                    }
                }
            }

            // восстанавливаем данные раздела с товарами
            itemsGrid = this.FindControl<DataGrid>("ItemsGrid");
            foreach (var column in itemsGrid.Columns)
            {
                if (itemsDataGridColumns.ContainsKey(column.DisplayIndex))
                {
                    column.Width = new DataGridLength((double)dataContext.SerializationItems[itemsDataGridColumns[column.DisplayIndex]]);
                }
            }
            itemsContextMenu = this.FindControl<ContextMenu>("ItemsContextMenu");
            if (itemsContextMenu != null)
            {
                int itemsTableColumns = (int)dataContext.SerializationItems[ESerializationKeys.AddColumns];
                foreach (CheckedMenuItem item in itemsContextMenu.Items)
                {
                    if (item.Tag != null && item.Tag is EAdditionalItemsTableColumns column)
                    {
                        item.IsChecked = (itemsTableColumns & (int)column) > 0;
                    }
                }
            }

            // восстанавливаем данные раздела с партнёрами
            partnersGrid = this.FindControl<DataGrid>("PartnersGrid");
            foreach (var column in partnersGrid.Columns)
            {
                if (itemsDataGridColumns.ContainsKey(column.DisplayIndex))
                {
                    column.Width = new DataGridLength((double)dataContext.SerializationPartners[partnersDataGridColumns[column.DisplayIndex]]);
                }
            }
            partnersContextMenu = this.FindControl<ContextMenu>("PartnersContextMenu");
            if (partnersContextMenu != null)
            {
                int partnerTableColumns = (int)dataContext.SerializationPartners[ESerializationKeys.AddColumns];
                foreach (CheckedMenuItem item in partnersContextMenu.Items)
                {
                    if (item.Tag != null && item.Tag is EAdditionalPartnersTableColumns column)
                    {
                        item.IsChecked = (partnerTableColumns & (int)column) > 0;
                    }
                }
            }
        }

        /// <summary>
        /// Saves visual data to database.
        /// </summary>
        /// <param name="viewId">Id of a ViewModel.</param>
        /// <date>31.05.2022.</date>
        private void SerializeVisualData(string viewId)
        {
            dataContext.ViewClosing -= SerializeVisualData;
            dataContext.OrderChanged -= DataContext_OrderChanged;
            dataContext.InvalidOrderRecord -= DataContext_InvalidOrderRecord;

            // сохраняем данные основного раздела
            foreach (var column in saleGrid.Columns)
            {
                if (saleDataGridColumns.ContainsKey(column.DisplayIndex))
                {
                    dataContext.SerializationService[saleDataGridColumns[column.DisplayIndex]].Value = column.ActualWidth.ToString();
                }
            }
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
            dataContext.SerializationService[ESerializationKeys.AddColumns].Value = ((int)tableColumns).ToString();
            dataContext.SerializationService.Update();

            // сохраняем данные раздела с товарами 
            foreach (var column in itemsGrid.Columns)
            {
                if (itemsDataGridColumns.ContainsKey(column.DisplayIndex))
                {
                    dataContext.SerializationItems[itemsDataGridColumns[column.DisplayIndex]].Value = column.ActualWidth.ToString();
                }
            }
            EAdditionalItemsTableColumns itemsColumns = 0;
            if (itemsContextMenu != null)
            {
                foreach (CheckedMenuItem item in itemsContextMenu.Items)
                {
                    if (item.IsChecked && item.Tag != null && item.Tag is EAdditionalItemsTableColumns column)
                    {
                        itemsColumns = itemsColumns | column;
                    }
                }
            }
            dataContext.SerializationItems[ESerializationKeys.AddColumns].Value = ((int)itemsColumns).ToString();
            dataContext.SerializationItems.Update();

            // сохраняем данные раздела с партнёрами
            foreach (var column in partnersGrid.Columns)
            {
                if (partnersDataGridColumns.ContainsKey(column.DisplayIndex))
                {
                    dataContext.SerializationPartners[partnersDataGridColumns[column.DisplayIndex]].Value = column.ActualWidth.ToString();
                }
            }
            EAdditionalPartnersTableColumns partnersColumns = 0;
            if (partnersContextMenu != null)
            {
                foreach (CheckedMenuItem item in partnersContextMenu.Items)
                {
                    if (item.IsChecked && item.Tag != null && item.Tag is EAdditionalPartnersTableColumns column)
                    {
                        partnersColumns = partnersColumns | column;
                    }
                }
            }
            dataContext.SerializationPartners[ESerializationKeys.AddColumns].Value = ((int)partnersColumns).ToString();
            dataContext.SerializationPartners.Update();
        }

        /// <summary>
        /// Sets focus to start control when TextBox with partner data is attached to visual tree. 
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">VisualTreeAttachmentEventArgs.</param>
        /// <date>07.06.2022.</date>
        private void TextBoxPartner_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (dataContext.IsChoiceOfPartnerEnabled && dataContext.OperationPartner == null)
            {
                if (sender is TextBox textBox)
                {
                    textBox.Focus();
                }
            }
            else
            {
                saleGrid.Focus();
                saleGrid.BeginEdit();
            }
        }

        /// <summary>
        /// Sets scanned data to input control.
        /// </summary>
        /// <param name="barcode">Data is sent from scanner.</param>
        /// <date>21.06.2022.</date>
        private void SetScannedData(string barcode)
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => 
            {
                if (textBoxPartner.IsFocused)
                {
                    dataContext.FindPartnerByCardNumber(barcode);
                }
                else
                {
                    switch (inputControl)
                    {
                        case TextBox textBox:
                            textBox.Text = barcode;
                            break;
                    }
                }
            });
        }

        /// <summary>
        /// Subscribes to TemplateApplied event when row is loading
        /// </summary>
        /// <param name="sender">DataGrid.</param>
        /// <param name="e">DataGridRowEventArgs</param>
        /// <date>20.06.2022.</date>
        private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            //if (dataContext.Order.Count == 1 && dataContext.Order[0].Item.Id == 0)
            //{
            //    e.Row.TemplateApplied += DataGridRow_TemplateApplied;
            //}

            e.Row.TemplateApplied += DataGridRow_TemplateApplied;
        }

        /// <summary>
        /// Sets focus to start control when template for DataGridRow is applied. 
        /// </summary>
        /// <param name="sender">DataGridRow.</param>
        /// <param name="e">TemplateAppliedEventArgs.</param>
        /// <date>21.06.2022.</date>
        private void DataGridRow_TemplateApplied(object? sender, TemplateAppliedEventArgs e)
        {
            if (sender is DataGridRow gridRow)
            {
                gridRow.TemplateApplied -= DataGridRow_TemplateApplied;
            }

            if (!dataContext.IsChoiceOfPartnerEnabled || dataContext.OperationPartner != null)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(100);
                    Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        SetFocusToDataGrid();
                    });
                });
            }
        }
        
        /// <summary>
        /// Activates DataGridCell to edit.
        /// </summary>
        /// <date>20.06.2022.</date>
        private void SetFocusToDataGrid()
        {
            if (dataContext.Order.Count > 1)
            {
                saleGrid.SelectedItems.Clear();
                saleGrid.SelectedItems.Add(dataContext.Order[dataContext.Order.Count - 1]);
            }

            if (!saleGrid.IsFocused)
            {
                saleGrid.Focus();
                saleGrid.BeginEdit();
            }

            DataGridRow? lastRow = saleGrid.GetLastRow();
            if (lastRow != null)
            {
                DataGridCell? itemNameCell = saleGrid.GetCell(lastRow, 3);

                if (itemNameCell != null)
                {
                    itemNameCell.Focus();
                    itemNameCell.BeginCellEdit();
                }
            }
        }

        /// <summary>
        /// Activates DataGridCell to edit when partner was selected or qty of item was changed programmatic.
        /// </summary>
        /// <date>22.06.2022.</date>
        private void DataContext_OrderChanged()
        {
            SetFocusToDataGrid();
        }

        /// <summary>
        /// Sets InputControl when cell began edit.
        /// </summary>
        /// <param name="sender">IControl.</param>
        /// <param name="e">DataGridBeginningEditEventArgs</param>
        /// <date>21.06.2022.</date>
        private void DataGridColumn_CellEditBegan(object? sender, DataGridBeginningEditEventArgs e)
        {
            if (sender != null && sender is IControl control)
            {
                InputControl = control;
            }
        }

        /// <summary>
        /// Clears InputControl when cell ended edit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">DataGridCellEditEndedEventArgs.</param>
        /// <date>21.06.2022</date>
        private void DataGrid_CellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
        {
            InputControl = null;
        }

        /// <summary>
        /// Shows user invalid data.
        /// </summary>
        /// <param name="invalidColumn">Index of column with invalid data.</param>
        /// <param name="invalidRow">Index of row with invalid data.</param>
        /// <date>23.06.2022.</date>
        private void DataContext_InvalidOrderRecord(int invalidColumn, int invalidRow)
        {
            if (invalidColumn == -1 && invalidRow == -1)
            {
                SetFocusToDataGrid();
            }
            else
            {
                DataGridCell invalidCell = saleGrid.GetCell(invalidColumn, invalidRow);
                saleGrid.SelectedItems.Clear();
                saleGrid.SelectedItems.Add(dataContext.Order[invalidRow]);                
                invalidCell.BeginCellEdit();
                invalidCell.BeginCellEdit();
            }
        }

        /// <summary>
        /// Changes the visibility property of the panel to pay in cash.
        /// </summary>
        /// <date>27.05.2022.</date>
        private void ChangeIsPayInCashPanelVisible()
        {
            IsPayInCashPanelVisible = !IsPayInCashPanelVisible;
        }

        /// <summary>
        /// Changes the visibility property of the panel with item editing buttons.
        /// </summary>
        /// <param name="sender">Button.</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <date>27.05.2022.</date>
        private void ButtonEditNomenclature_Click(object? sender, RoutedEventArgs e)
        {
            IsEditPanelVisible = !IsEditPanelVisible;
        }

        /// <summary>
        /// Causes the TextBox for entering a view title to lose focus  when Enter is pressed in this TextBox.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">KeyEventArgs</param>
        /// <date>27.05.2022.</date>
        private void TextBoxTitle_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.Focus();
                    break;
            }
        }

        /// <summary>
        /// Update dependents properties if main property of TextBox to input title of the view was changed.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">AvaloniaPropertyChangedEventArgs</param>
        /// <date>27.05.2022.</date>
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
                        dataContext.IsSaleTitleReadOnly = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets Partners to ActiveNomenclature and InputControl when TextBox to input parter data got focus; otherwise clears InputControl.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">GotFocusEventArgs</param>
        /// <date>30.05.2022.</date>
        private void TextBoxPartner_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case (nameof(TextBox.IsFocused)):
                    if (sender is TextBox textBox)
                    {
                        if (textBox.IsFocused)
                        {
                            ActiveNomenclature = ENomenclatures.Partners;
                            InputControl = textBox;
                        }
                        else
                        {
                            InputControl = null;

                            if (dataContext.OperationPartner != null && !textBox.Text.Equals(dataContext.OperationPartner.Name))
                            {
                                textBox.Text = dataContext.OperationPartner.Name;
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Finds and sets item to Order (property of SaleViewModel).
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">KeyEventArgs</param>
        /// <date>21.06.2022.</date>
        private void CellTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    switch (dataContext.Items.Count)
                    {
                        case 0:
                            dataContext.AddItems(null);
                            break;
                        case 1:
                            dataContext.AddItems(dataContext.Items);
                            break;
                        default:
                            itemsGrid.Focus();
                            itemsGrid.SelectedIndex = 0;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Finds and sets partner to OperationPartner (property of SaleViewModel).
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">KeyEventArgs</param>
        /// <date>21.06.2022.</date>
        private void TextBoxPartner_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    switch (dataContext.Partners.Count)
                    {
                        case 0:
                            string searchKey = (sender as TextBox).Text;
                            dataContext.AddPartner(new PartnerModel()
                            {
                                Name = searchKey,
                                Principal = searchKey,
                            });
                            break;
                        case 1:
                            dataContext.AddPartner(dataContext.Partners[0]);
                            break;
                        default:
                            partnersGrid.Focus();
                            partnersGrid.SelectedIndex = 0;
                            break;
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets Items to ActiveNomenclature when DataGrid with items or to order got focus.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">GotFocusEventArgs</param>
        /// <date>30.05.2022.</date>
        private void DataGridItems_GotFocus(object? sender, GotFocusEventArgs e)
        {
            ActiveNomenclature = ENomenclatures.Items;
        }

        /// <summary>
        /// Sets Partners to ActiveNomenclature when DataGrid with partners got focus.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">GotFocusEventArgs</param>
        /// <date>30.05.2022.</date>
        private void DataGridPartners_GotFocus(object? sender, GotFocusEventArgs e)
        {
            ActiveNomenclature = ENomenclatures.Partners;
        }

        /// <summary>
        /// Sets ItemsGroups to ActiveNomenclature when TreeView with groups of items got focus.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">GotFocusEventArgs</param>
        /// <date>30.05.2022.</date>
        private void ItemsGroupsTreeView_GotFocus(object? sender, GotFocusEventArgs e)
        {
            ActiveNomenclature = ENomenclatures.ItemsGroups;
        }

        /// <summary>
        /// Sets PartnersGroups to ActiveNomenclature when TreeView with groups of partners got focus.
        /// </summary>
        /// <param name="sender">TreeView.</param>
        /// <param name="e">GotFocusEventArgs</param>
        /// <date>30.05.2022.</date>
        private void PartnersGroupsTreeView_GotFocus(object? sender, GotFocusEventArgs e)
        {
            ActiveNomenclature = ENomenclatures.PartnersGroups;
        }

        /// <summary>
        /// Shows panel to edit nomenclatures.
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">RoutedEventArgs</param>
        /// <date>27.05.2022.</date>
        private void EditNomenclatureButton_Click(object? sender, RoutedEventArgs e)
        {
            IsEditNomenclaturePanelVisible = true;
        }

        /// <summary>
        /// Finds partner by tax or VAT number when Enter was pressed in accordinally TextBox.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">AvaloniaPropertyChangedEventArgs.</param>
        /// <date>27.05.2022.</date>
        private void TextBoxFindPartner_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (sender is TextBox textBox)
                    {
                        dataContext.FindPartner(textBox.Text);
                    }
                    break;
            }
        }

        /// <summary>
        /// Finds item by barcode when Enter was pressed in accordinally TextBox.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">AvaloniaPropertyChangedEventArgs.</param>
        /// <date>04.07.2022.</date>
        private void TextBoxFindItem_KeyDown(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (sender is TextBox textBox)
                    {
                        dataContext.FindItem(textBox.Text);
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets InputControl when TextBox for number of card is focused; otherwise clears InputControl.
        /// </summary>
        /// <param name="sender">TextBox.</param>
        /// <param name="e">AvaloniaPropertyChangedEventArgs.</param>
        /// <date>27.05.2022.</date>
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
                                InputControl = tb;
                                break;
                            case false:
                                InputControl = null;
                                break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Initialize components.
        /// </summary>
        /// <date>27.05.2022.</date>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Update dependents properties if main property was changed.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="change">History of changing of property.</param>
        /// <date>31.05.2022.</date>
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
    }
}
