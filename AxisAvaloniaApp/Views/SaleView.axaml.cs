using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.VisualTree;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Serialization;
using AxisAvaloniaApp.UserControls.Extensions;
using AxisAvaloniaApp.ViewModels;
using DynamicData;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace AxisAvaloniaApp.Views
{
    public partial class SaleView : UserControl
    {
        private ViewModels.SaleViewModel dataContext;
        private DataGrid saleGrid;
        private DataGrid itemsGrid;
        private DataGrid partnersGrid;
        ContextMenu saleContextMenu;
        TreeView itemsGroupsTreeView;
        TreeView partnersGroupsTreeView;
        private readonly ISerializationService serializationItems;
        private readonly ISerializationService serializationPartners;
        private Dictionary<int, ESerializationKeys> saleDataGridColumns;
        private Dictionary<int, ESerializationKeys> itemsDataGridColumns;
        private Dictionary<int, ESerializationKeys> partnersDataGridColumns;

        public SaleView()
        {
            InitializeComponent();

            serializationItems = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            serializationPartners = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            dataContext = Splat.Locator.Current.GetRequiredService<ViewModels.SaleViewModel>();
            this.DataContext = dataContext;
            dataContext.ViewClosing += SerializeVisualData;
            dataContext.PropertyChanged += DataContext_PropertyChanged;

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
            
            IsPaymentPanelVisible = false;
            IsPayInCashPanelVisible = false;
            IsEditPanelVisible = false;

            DeserializeVisualData();








            

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

        private void DataContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SaleViewModel.OperationPartner):
                    saleGrid.Focus();
                    saleGrid.BeginEdit();

                    //var focused = FocusManager.Instance.Current;
                    var currentRow = saleGrid.FindDescendantOfType<DataGridRowsPresenter>()
                        .Children.OfType<DataGridRow>()
                        .FirstOrDefault(r => r.FindDescendantOfType<DataGridCellsPresenter>()
                            .Children.Any(p => p.Classes.Contains(":current")));
                    var item = currentRow?.DataContext;

                    

                    //var el = typeof(DataGridColumn).GetField("_editingElement", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    

                    //var presenter = saleGrid.Columns[4].GetCellContent(currentRow);

                    var currentCell = currentRow?.FindDescendantOfType<DataGridCellsPresenter>().Children
                        .OfType<DataGridCell>().FirstOrDefault(p => p.Classes.Contains(":current"));

                    IEnumerable<DataGridCell> currentCellList = currentRow?.FindDescendantOfType<DataGridCellsPresenter>().Children
                        .OfType<DataGridCell>();
                    var cell = currentCellList.ElementAt(3);
                    cell.Focus();

                    var keyboard = KeyboardDevice.Instance;
                    var inputManager = InputManager.Instance;
                    MouseDevice mouseDevice = new MouseDevice(new Pointer(0, PointerType.Mouse, true));
                    inputManager.ProcessInput(new RawInputEventArgs(mouseDevice, (ulong)DateTime.Now.Ticks, null));
                    //inputManager.ProcessInput(new RawKeyEventArgs(keyboard, (ulong)DateTime.Now.Ticks, null, RawKeyEventType.KeyDown, Key.Enter, RawInputModifiers.None));


                    //var method = saleGrid.Columns[3].GetType().GetMethod("GenerateEditingElementDirect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    //var textBox = el.GetValue(saleGrid.Columns[3]);
                    //var tb = method.Invoke(saleGrid.Columns[3], new object[] { cell, string.Empty });
                    //(tb as TextBox).Background = Brushes.Red; //.Focus();

                    //FocusableDataGridTextColumn focusableData = saleGrid.Columns[3] as FocusableDataGridTextColumn;
                    //if (saleGrid.Columns[3] is FocusableDataGridTextColumn textColumn)
                    //{
                    //    textColumn.Test(cell, "");
                    //}

                    //Keyboard.ClearFocus();

                    //FocusManager.SetFocusedElement(cell, cell);
                    //Keyboard.Focus(cell);

                    //var EditingColumnIndex = saleGrid.GetType().GetProperty("EditingColumnIndex", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    //EditingColumnIndex.SetValue(saleGrid, 3);
                    //var CurrentSlot = saleGrid.GetType().GetProperty("CurrentSlot", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    //CurrentSlot.SetValue(saleGrid, 3);

                    //var CurrentColumnIndex = saleGrid.GetType().GetProperty("CurrentColumnIndex", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    //CurrentColumnIndex.SetValue(saleGrid, 3);

                    //var EditingRow = saleGrid.GetType().GetProperty("EditingRow", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    //EditingRow.SetValue(saleGrid, currentRow);


                    
                    FocusManager.Instance.Focus(cell, NavigationMethod.Unspecified);
                    //MouseDevice mouseDevice = new MouseDevice(new Pointer(0, PointerType.Mouse, true));
                    mouseDevice.Capture(cell);
                    mouseDevice.ProcessRawEvent(new Avalonia.Input.Raw.RawInputEventArgs(mouseDevice, 0, null));
                    //mouseDevice.ProcessRawEvent(new Avalonia.Input.Raw.RawInputEventArgs())
                    cell.Focus();

                    //FocusManager.Instance.Focus(cell);
                    break;
            }
        }

        /// <summary>
        /// Subscribes to LanguageChanged event when the PaymentButton is added to a rooted visual tree.
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>09.06.2022.</date>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            //this.translationService.LanguageChanged += Localize;
            base.OnAttachedToVisualTree(e);
        }

        /// <summary>
        /// Unsubscribes for LanguageChanged event when the PaymentButton is removed from a rooted visual tree.
        /// </summary>
        /// <param name="e">VisualTreeAttachmentEventArgs</param>
        /// <date>09.06.2022.</date>
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            //this.translationService.LanguageChanged -= Localize;
            base.OnDetachedFromVisualTree(e);
        }

        private void DeserializeVisualData()
        {
            saleGrid = this.FindControl<DataGrid>("SaleGrid");
            saleGrid.BeginningEdit += fdfsdf;
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
                    if (item.Tag != null && item.Tag is EAdditionalSaleTableColumns column)
                    {
                        item.IsChecked = ((EAdditionalSaleTableColumns)dataContext.SerializationService[ESerializationKeys.AddColumns] & column) > 0;
                    }
                }
            }

            serializationItems.InitSerializationData(ESerializationGroups.ItemsNomenclature);
            itemsGroupsTreeView = this.FindControl<TreeView>("ItemsGroupsTreeView");
            itemsGrid = this.FindControl<DataGrid>("ItemsGrid");
            foreach (var column in itemsGrid.Columns)
            {
                if (itemsDataGridColumns.ContainsKey(column.DisplayIndex))
                {
                    column.Width = new DataGridLength((double)serializationItems[itemsDataGridColumns[column.DisplayIndex]]);
                }
            }

            serializationPartners.InitSerializationData(ESerializationGroups.PartnersNomenclature);
            partnersGroupsTreeView = this.FindControl<TreeView>("PartnersGroupsTreeView");
            partnersGroupsTreeView.SelectionChanged += dsadsa;
            partnersGroupsTreeView.ItemContainerGenerator.Dematerialized += fsdf;
            partnersGroupsTreeView.ItemContainerGenerator.Materialized += fsdf1;
            partnersGroupsTreeView.ItemContainerGenerator.Recycled += fsdf2;
            //partnersGroupsTreeView.AttachedToVisualTree += trerfsd;
        }

        private void fdfsdf(object? sender, DataGridBeginningEditEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void dsadsa(object? sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void fsdf2(object? sender, ItemContainerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void fsdf1(object? sender, ItemContainerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void fsdf(object? sender, ItemContainerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void ItemsGroupsTreeView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void PartnersGroupsTreeView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            //throw new NotImplementedException();
        }

        protected override void OnMeasureInvalidated()
        {
            base.OnMeasureInvalidated();

           // if (partnersGroupsTreeView.ItemContainerGenerator.Index = .Containers.)
        }

        private void SerializeVisualData(string viewId)
        {
            dataContext.ViewClosing -= SerializeVisualData;
            dataContext.PropertyChanged -= DataContext_PropertyChanged;

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

        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();

        //    TextBox textBox = this.FindControl<TextBox>("TextBoxPartner");
        //    if (textBox != null)
        //    {
        //        textBox.Focus();
        //    }
        //}

        //public override void EndInit()
        //{
        //    base.EndInit();

        //    //TextBox textBox = this.FindControl<TextBox>("TextBoxPartner");
        //    //if (textBox != null)
        //    //{
        //    //    textBox.Focus();
        //    //}
        //}

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

        private void ItemsGroupsTreeView_GotFocus(object? sender, GotFocusEventArgs e)
        {
            ActiveNomenclature = ENomenclatures.ItemsGroups;
        }

        private void PartnersGroupsTreeView_GotFocus(object? sender, GotFocusEventArgs e)
        {
            ActiveNomenclature = ENomenclatures.PartnersGroups;
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
