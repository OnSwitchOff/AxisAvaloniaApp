using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Rules.Exchange;
using AxisAvaloniaApp.Services.Printing;
using AxisAvaloniaApp.UserControls.Models;
using Microinvest.ExchangeDataService.Enums;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;

namespace AxisAvaloniaApp.ViewModels
{
    public class ExchangeViewModel : OperationViewModelBase
    {
        private bool isImportDirection;
        private ObservableCollection<ComboBoxItemModel> importFromItems;
        private ComboBoxItemModel selectedImportFromItem;
        private ObservableCollection<ComboBoxItemModel> exportToItems;
        private ComboBoxItemModel selectedExportToItem;
        private bool isAnotherApp;
        private bool isSaleOperation;
        private string numberFrom;
        private string numberTo;
        private DateTime dateFrom;
        private DateTime dateTo;
        private string statusString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeViewModel"/> class.
        /// </summary>
        public ExchangeViewModel()
        {
            ExecuteCommand = ReactiveCommand.Create(Execute);
            ClearCommand = ReactiveCommand.Create(Clear);
            DateTo = DateTime.Now;
            DateFrom = DateTo.AddDays(-5);

            this.ViewClosing += ExchangeViewModel_ViewClosing;
            this.PropertyChanged += ExchangeViewModel_PropertyChanged;
        }

        /// <summary>
        /// Gets or sets value indicating whether is there import operation now.
        /// </summary>
        /// <date>07.06.2022.</date>
        public bool IsImportDirection
        { 
            get => isImportDirection; 
            set => this.RaiseAndSetIfChanged(ref isImportDirection, value); 
        }

        /// <summary>
        /// Gets or sets list with applications from which import is supported.  
        /// </summary>
        /// <date>07.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> ImportFromItems
        {
            get
            {
                if (importFromItems == null)
                {
                    importFromItems = new ObservableCollection<ComboBoxItemModel>()
                    {
                        new ComboBoxItemModel()
                        {
                            Key = "strWarehouseSkladPro",
                            Value = EExchanges.ImportFromWarehouseSkladPro,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strAnotherApp",
                            Value = EExchanges.ImportFromSomeApp,
                        },
                    };
                }

                return importFromItems;
            }

            set => this.RaiseAndSetIfChanged(ref importFromItems, value);
        }

        /// <summary>
        /// Gets or sets the application to import was selected by user.
        /// </summary>
        /// <date>07.06.2022.</date>
        public ComboBoxItemModel SelectedImportFromItem
        {
            get => selectedImportFromItem; 
            set => this.RaiseAndSetIfChanged(ref selectedImportFromItem, value);
        }

        /// <summary>
        /// Gets or sets list with applications to which export is supported.  
        /// </summary>
        /// <date>07.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> ExportToItems
        {
            get
            {
                if (exportToItems == null)
                {
                    exportToItems = new ObservableCollection<ComboBoxItemModel>()
                    {
                        new ComboBoxItemModel()
                        {
                            Key = "strWarehouseSkladPro",
                            Value = EExchanges.ExportToWarehouseSkladPro,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strDeltaPro",
                            Value = EExchanges.ExportToDeltaPro,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strNAP",
                            Value = EExchanges.ExportToNAP,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strAnotherApp",
                            Value = EExchanges.ExportToSomeApp,
                        },
                    };
                }

                return exportToItems;
            }
            set => this.RaiseAndSetIfChanged(ref exportToItems, value);
        }

        /// <summary>
        /// Gets or sets the application to export was selected by user.
        /// </summary>
        /// <date>07.06.2022.</date>
        public ComboBoxItemModel SelectedExportToItem 
        { 
            get => selectedExportToItem; 
            set => this.RaiseAndSetIfChanged(ref selectedExportToItem, value);
        }

        /// <summary>
        /// Gets or sets value indicating whether data will be exported/imported to unidentified application.
        /// </summary>
        /// <date>07.06.2022.</date>
        public bool IsAnotherApplication
        {
            get => isAnotherApp;
            set => this.RaiseAndSetIfChanged(ref isAnotherApp, value);
        }
        
        /// <summary>
        /// Gets or sets start number to get data from the database.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string NumberFrom 
        { 
            get => numberFrom; 
            set => this.RaiseAndSetIfChanged(ref numberFrom, value); }

        /// <summary>
        /// Gets or sets end number to get data from the database.
        /// </summary>
        /// <date>07.06.2022.</date>
        public string NumberTo
        {
            get => numberTo;
            set => this.RaiseAndSetIfChanged(ref numberTo, value);

        }

        /// <summary>
        /// Gets or sets start date to get data from the database.
        /// </summary>
        /// <date>07.06.2022.</date>
        public DateTime DateFrom 
        { 
            get => dateFrom; 
            set => this.RaiseAndSetIfChanged(ref dateFrom, value); }

        /// <summary>
        /// Gets or sets end date to get data from the database.
        /// </summary>
        /// <date>07.06.2022.</date>
        public DateTime DateTo 
        { 
            get => dateTo; 
            set => this.RaiseAndSetIfChanged(ref dateTo, value); 
        }

        /// <summary>
        /// Gets or sets description with status of execution.
        /// </summary>
        /// <date>12.07.2022.</date>
        public string StatusString
        {
            get => statusString;
            set => this.RaiseAndSetIfChanged(ref statusString, value);
        }

        #region Commands
        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        #endregion

        /// <summary>
        /// Unsubscribes from the events when view is closing.
        /// </summary>
        /// <param name="viewId">Id of closing view.</param>
        /// <date>12.07.2022.</date>
        private void ExchangeViewModel_ViewClosing(string viewId)
        {
            this.ViewClosing -= ExchangeViewModel_ViewClosing;
            this.PropertyChanged -= ExchangeViewModel_PropertyChanged;
        }

        /// <summary>
        /// Updates dependent property when main property was changed.
        /// </summary>
        /// <param name="sender">ExchangeViewModel.</param>
        /// <param name="e">PropertyChangedEventArgs.</param>
        /// <date>12.07.2022.</date>
        private void ExchangeViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsImportDirection):
                    switch (IsImportDirection)
                    {
                        case true:
                            this.RaisePropertyChanged(nameof(SelectedImportFromItem));
                            break;
                        case false:
                            this.RaisePropertyChanged(nameof(SelectedExportToItem));
                            break;
                    }
                    break;
                case nameof(SelectedExportToItem):
                    if (SelectedExportToItem == null)
                    {
                        IsAnotherApplication = false;
                    }
                    else
                    {
                        if (SelectedExportToItem.Value != null &&
                            SelectedExportToItem.Value is EExchanges export)
                        {
                            switch (export)
                            {
                                case EExchanges.ExportToSomeApp:
                                    IsAnotherApplication = true;
                                    break;
                                default:
                                    IsAnotherApplication = false;
                                    break;
                            }
                        }
                    }
                    break;
                case nameof(SelectedImportFromItem):
                    if (SelectedImportFromItem == null)
                    {
                        IsAnotherApplication = false;
                    }
                    else
                    {
                        if (SelectedImportFromItem.Value != null &&
                            SelectedImportFromItem.Value is EExchanges import)
                        {
                            switch (import)
                            {
                                case EExchanges.ImportFromSomeApp:
                                    IsAnotherApplication = true;
                                    break;
                                default:
                                    IsAnotherApplication = false;
                                    break;
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Prepares data and exports/imports it.
        /// </summary>
        /// <date>12.07.2022.</date>
        private void Execute()
        {
            IStage fullMonthForNAP;
            IStage dataWasExportedEarlier;
            EExchanges exchange;
            long acctFrom = string.IsNullOrEmpty(NumberFrom) ? 0 : long.Parse(NumberFrom);
            long acctTo = string.IsNullOrEmpty(NumberTo) ? long.MaxValue : long.Parse(NumberTo);
            switch (IsImportDirection)
            {
                case true:
                    if (SelectedImportFromItem != null && SelectedImportFromItem.Value != null)
                    {
                        exchange = (EExchanges)SelectedImportFromItem.Value;
                        fullMonthForNAP = new FullMonthForNAP(exchange, DateFrom, DateTo);
                        dataWasExportedEarlier = new DataWasExportedEarlier(exchange, DateFrom, DateTo, acctFrom, acctTo);
                    }
                    break;
                case false:
                    if (SelectedExportToItem != null && SelectedExportToItem.Value != null)
                    {
                        exchange = (EExchanges)SelectedExportToItem.Value;
                        fullMonthForNAP = new FullMonthForNAP(exchange, DateFrom, DateTo);
                        dataWasExportedEarlier = new DataWasExportedEarlier(exchange, DateFrom, DateTo, acctFrom, acctTo);
                    }
                    break;
            }
        }

        /// <summary>
        /// Clears string with description of status of execution.
        /// </summary>
        /// <date>12.07.2022.</date>
        private void Clear()
        {
            StatusString = string.Empty;
        }
    }
}
