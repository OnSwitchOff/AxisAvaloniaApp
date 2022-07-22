using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.UserControls.Models;
using System.Reactive;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.Documents;
using AxisAvaloniaApp.Helpers;
using DataBase.Entities.OperationHeader;
using DataBase.Entities.Documents;
using System.Diagnostics;
using AxisAvaloniaApp.Services.Document;
using System.Reactive.Subjects;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Services.Logger;
using Avalonia.Interactivity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AxisAvaloniaApp.ViewModels
{
    public abstract class DocumentViewModel : OperationViewModelBase
    {
        private readonly IOperationHeaderRepository operationHeaderRepository;
        private readonly IDocumentsRepository documentsRepository;

        private readonly IDocumentService documentService;
        private readonly ITranslationService translationService;
        private readonly ISettingsService settingsService;
        private readonly ILoggerService loggerService;

        protected abstract EDocumentTypes documentType { get; }
        protected string nextDocumentNumber { get; set; }

        #region MainContent
        private bool isMainContentVisible;
        private DocumentItem selectedItem;
        private ObservableCollection<DocumentItem> items;
        private ObservableCollection<DocumentItem> filtredItems;
        private ObservableCollection<System.Drawing.Image> pages;

        #endregion

        #region Filter section
        private ObservableCollection<ComboBoxItemModel> periods;
        private ComboBoxItemModel selectedPeriod;

        private string fromDateString;
        private DateTime fromDateTimeOffset;

        private string toDateString;
        private DateTime toDateTimeOffset;

        private string filterString;
        #endregion

        public bool IsMainContentVisible { get => isMainContentVisible; set => this.RaiseAndSetIfChanged(ref isMainContentVisible, value); }
        public IDocumentService DocumentService
        {
            get => documentService;
        }
        public DocumentItem SelectedItem {
            get=>selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);
                DocumentIsSelected = value != null;
            }
        }
        public ObservableCollection<DocumentItem> Items { get => items; set => this.RaiseAndSetIfChanged(ref items, value); }
        public ObservableCollection<DocumentItem> FiltredItems { get => filtredItems; set => this.RaiseAndSetIfChanged(ref filtredItems, value); }

        public ObservableCollection<System.Drawing.Image> Pages
        {
            get => pages == null ? pages = new ObservableCollection<System.Drawing.Image>() : pages;
            set => this.RaiseAndSetIfChanged(ref pages, value);
        }

        public ObservableCollection<ComboBoxItemModel> Periods
        {
            get
            {
                if (periods == null)
                {
                    periods = new ObservableCollection<ComboBoxItemModel>()
                    {
                        new ComboBoxItemModel() { Key = "strAll", Value = EPeriods.All, },
                        new ComboBoxItemModel() { Key = "strToday", Value = EPeriods.Today, },
                        new ComboBoxItemModel() { Key = "strWeekAgo", Value = EPeriods.WeekAgo, },
                        new ComboBoxItemModel() { Key = "strMonthAgo", Value = EPeriods.MonthAgo, },
                        new ComboBoxItemModel() { Key = "strYearAgo", Value = EPeriods.YearAgo, },
                        new ComboBoxItemModel() { Key = "strCustom", Value = EPeriods.Custom, },
                    };
                }

                return periods;
            }
            set => this.RaiseAndSetIfChanged(ref periods, value);
        }
        public ComboBoxItemModel SelectedPeriod
        {
            get => selectedPeriod;
            set => this.RaiseAndSetIfChanged(ref selectedPeriod, value);
        }
        public string FromDateString { get => fromDateString; set => this.RaiseAndSetIfChanged(ref fromDateString, value); }
        public DateTime FromDateTimeOffset
        {
            get => fromDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref fromDateTimeOffset, value);
                if (value != null)
                {
                    FromDateString = ((DateTime)FromDateTimeOffset).Date.ToString("dd.MM.yyyy");

                    TryToGetSourceCollection();
                }
            }
        }

        public async void SaveDocument()
        {
            Document document = Document.Create(
                SelectedItem.OperationHeader,
                SelectedItem.TempDocumentNumber,
                SelectedItem.InvoiceDateTimeOffset,
                documentType,
                SelectedItem.DealDateTimeOffset,
                "", new DateTime(),
                SelectedItem.Receiver,
                SelectedItem.InvoicePrepared,
                SelectedItem.Description,
                SelectedItem.DealPlace);

            int docId = await documentsRepository.AddDocumentAsync(document);

            if (docId > 0)
            {
                SelectedItem.Document = document;
                if (document != null)
                {
                    SelectedItem.InvoicePrepared = document.CreatorName;
                    SelectedItem.Receiver = document.RecipientName;

                    SelectedItem.InvoiceNumber = document.DocumentNumber;
                    SelectedItem.TempDocumentNumber = document.DocumentNumber;
                    SelectedItem.InvoiceDateTimeOffset = document.DocumentDate;
                    SelectedItem.DealDateTimeOffset = document.TaxDate;
                    SelectedItem.DealPlace = document.DealLocation;
                    SelectedItem.Description = document.DealDescription;
                }

                string prev = nextDocumentNumber;
                nextDocumentNumber = await documentsRepository.GetNextDocumentNumberAsync(documentType);
                foreach (DocumentItem docItem in Items)
                {
                    if (docItem != SelectedItem && (string.IsNullOrEmpty(docItem.TempDocumentNumber) || docItem.TempDocumentNumber == prev))
                    {
                        docItem.TempDocumentNumber = nextDocumentNumber;
                    }
                }
                IsMainContentVisible = true;
            }
        }

        private void FilterSourceCollection()
        {
            isFiltring = true;
            FiltredItems.Clear();

            foreach (DocumentItem doc in Items)
            {
                if (string.IsNullOrEmpty(FilterString)
                    || (doc.InvoiceNumber != null && doc.InvoiceNumber.Contains(FilterString))
                    || doc.Sale.Contains(FilterString)
                    || doc.Client.Name.Contains(FilterString)
                    || doc.Client.Address.Contains(FilterString)
                    || doc.Client.Phone.Contains(FilterString)
                    || doc.Client.City.Contains(FilterString))
                {
                    FiltredItems.Add(doc);
                }         
            }

            if (FiltredItems.Count > 0)
            {
                SelectedItem = FiltredItems[0];
            }
            isFiltring = false;
        }

        private async Task TryToGetSourceCollection()
        {
            if (FromDateTimeOffset > ToDateTimeOffset)
            {
                return;
            }

            Items.Clear();

            foreach (OperationHeader oh in await operationHeaderRepository.GetOperationHeadersByDatesAsync(FromDateTimeOffset, ToDateTimeOffset, EOperTypes.Sale))
            {
                DocumentItem docItem = new DocumentItem(oh, await documentsRepository.GetDocumentsByOperationHeaderAsync(oh, documentType));
                if (docItem.Document == null)
                {
                    docItem.TempDocumentNumber = nextDocumentNumber;
                }
                Items.Add(docItem);
            }

            FilterSourceCollection();
        }

        public string ToDateString { get => toDateString; set => this.RaiseAndSetIfChanged(ref toDateString, value); }
        public DateTime ToDateTimeOffset
        {
            get => toDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref toDateTimeOffset, value);
                if (value != null)
                {
                    ToDateString = ((DateTime)ToDateTimeOffset).Date.ToString("dd.MM.yyyy");
                    TryToGetSourceCollection();
                }
            }
        }

        private Avalonia.Threading.DispatcherTimer filterTimer;

        private bool isFiltring;

        private string lastFilterString;
        public string FilterString
        {
            get => filterString;
            set 
            { 
                this.RaiseAndSetIfChanged(ref filterString, value);

                if (filterTimer != null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                filterTimer = new Avalonia.Threading.DispatcherTimer();
                filterTimer.Interval = TimeSpan.FromMilliseconds(1000);
                filterTimer.Tick += FilterTimer_Tick;
                filterTimer.Start();
                
            } 
        }

        public ReactiveCommand<Unit, Unit> PrintCommand { get; }
        private readonly BehaviorSubject<bool> _DocumentIsSelectedSubject = new BehaviorSubject<bool>(false);
        public bool DocumentIsSelected
        {
            get => _DocumentIsSelectedSubject.Value;
            set => _DocumentIsSelectedSubject.OnNext(value);
        }
        public IObservable<bool> ObservableDocumentIsSelected => _DocumentIsSelectedSubject;

        public ReactiveCommand<Unit, Unit> RefreshItemsListCommand { get; }

        public DocumentViewModel()
        {
            operationHeaderRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
            documentsRepository = Splat.Locator.Current.GetRequiredService<IDocumentsRepository>();
            documentService = Splat.Locator.Current.GetRequiredService<IDocumentService>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();
            Items = new ObservableCollection<DocumentItem>();
            FiltredItems = new ObservableCollection<DocumentItem>();
            SelectedPeriod = Periods[0];
            FromDateTimeOffset = DateTime.Today;
            ToDateTimeOffset = DateTime.Today;
            PrintCommand = ReactiveCommand.Create(Print, ObservableDocumentIsSelected);
            RefreshItemsListCommand = ReactiveCommand.Create(RefreshItemList);
            IsMainContentVisible = true;
            InitDocumentTitle();
            SetNextDocumentNumber();
        }

        private async void SetNextDocumentNumber()
        {
            nextDocumentNumber = await Task.Run(() => documentsRepository.GetNextDocumentNumberAsync(documentType));
        }

        private void InitDocumentTitle()
        {
            switch (documentType)
            {
                case EDocumentTypes.Unknown:
                    Title = translationService.Localize("strUnknownDocument");
                    break;
                case EDocumentTypes.Invoice:
                    Title = translationService.Localize("strInvoice");
                    break;
                case EDocumentTypes.DebitNote:
                    Title = translationService.Localize("strDebitNote");
                    break;
                case EDocumentTypes.CreditNote:
                    Title = translationService.Localize("strCreditNote");
                    break;
                case EDocumentTypes.ProformInvoice:
                    Title = translationService.Localize("strProformInvoice");
                    break;
                case EDocumentTypes.Receipt:
                    Title = translationService.Localize("strReceipt");
                    break;
                case EDocumentTypes.Report:
                    Title = translationService.Localize("strReport");
                    break;
                default:
                    Title = translationService.Localize("strUnknown");
                    break;
            }

            this.PropertyChanged += DocumentViewModel_PropertyChanged;
        }

        private void DocumentViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedPeriod):
                    switch ((EPeriods)SelectedPeriod.Value)
                    {
                        case EPeriods.All:
                            FromDateTimeOffset = new DateTime(2022, 1, 1);
                            ToDateTimeOffset = DateTime.Now;
                            break;
                        case EPeriods.Today:
                            FromDateTimeOffset = DateTime.Now;
                            ToDateTimeOffset = DateTime.Now;
                            break;
                        case EPeriods.WeekAgo:
                            FromDateTimeOffset = DateTime.Now.AddDays(-7);
                            ToDateTimeOffset = DateTime.Now;
                            break;
                        case EPeriods.MonthAgo:
                            FromDateTimeOffset = DateTime.Now.AddMonths(-1);
                            ToDateTimeOffset = DateTime.Now;
                            break;
                        case EPeriods.YearAgo:
                            FromDateTimeOffset = DateTime.Now.AddYears(-1);
                            ToDateTimeOffset = DateTime.Now;
                            break;
                        case EPeriods.Custom:
                            break;
                    }
                    break;
                    case nameof(FromDateTimeOffset):
                    break;
            }
        }

        async void Print()
        {
            if (await CheckDocNumberRepeating(SelectedItem.TempDocumentNumber, documentType))
            {
                loggerService.ShowDialog("msgErrorDuplicateDocumentNumber",icon: UserControls.MessageBoxes.EButtonIcons.Info);
                return;
            }
            if (CheckDocDataAfterOperation(SelectedItem.SaleDateTimeOffset, SelectedItem.InvoiceDateTimeOffset))
            {
                loggerService.ShowDialog("msgErrorDocDataAfterOperation");
                return;
            }
            else
            {

            }

            OperationHeader? operationData = SelectedItem.OperationHeader;
            if (operationData == null)
            {
                throw new InvalidCastException();
            }

            // заполняем данные о покупателе
            documentService.CustomerData = operationData.Partner;

            // заполняем описание документа
            documentService.DocumentDescription.DocumentName = translationService.Localize("str" + documentType.ToString());
            //documentService.DocumentDescription.DocumentDescription = translationService.Localize("strForSale");
            documentService.DocumentDescription.DocumentNumber = operationData.Acct.ToString("D10");
            documentService.DocumentDescription.DocumentDate = DateTime.Now;
            documentService.DocumentDescription.PaymentType = operationData.Payment.Name;
            documentService.DocumentDescription.DealReason = translationService.Localize("strSale");
            documentService.DocumentDescription.DocumentSum = settingsService.AppLanguage.MoneyByWords(
                (double)operationData.OperationDetails.Sum(d => d.Qtty * d.SalePrice),
                settingsService.Country.CurrencyCode.Convert());
            documentService.DocumentDescription.ReceivedBy = operationData.Partner.Principal;
            documentService.DocumentDescription.CreatedBy = settingsService.AppSettings[ESettingKeys.Principal];

            // размечаем таблицу с товарами
            documentService.ItemsData = new System.Data.DataTable();
            documentService.ItemsData.Columns.Add(translationService.Localize("strRowNumber"), typeof(int));
            documentService.ItemsData.Columns.Add(translationService.Localize("strCode"), typeof(string));
            documentService.ItemsData.Columns.Add(translationService.Localize("strGoods"), typeof(string));
            documentService.ItemsData.Columns.Add(translationService.Localize("strMeasure"), typeof(string));
            documentService.ItemsData.Columns.Add(translationService.Localize("strQtty"), typeof(double));
            documentService.ItemsData.Columns.Add(translationService.Localize("strPrice"), typeof(double));
            documentService.ItemsData.Columns.Add(translationService.Localize("strAmount_Short"), typeof(double));

            // заполняем таблицу с товарами данными
            documentService.ClearVATList();
            for (int i = 0; i < operationData.OperationDetails.Count; i++)
            {
                var item = operationData.OperationDetails[i];

                documentService.ItemsData.Rows.Add(
                    i+1,
                    item.Goods.Code,
                    item.Goods.Name,
                    item.Goods.Measure,
                    item.Qtty,
                    item.SalePrice,
                    item.Qtty * item.SalePrice);

                Microinvest.PDFCreator.Models.VATModel vAT = new Microinvest.PDFCreator.Models.VATModel()
                {
                    VATRate = (float)item.Goods.Vatgroup.VATValue,
                    VATSum = (double)Math.Round((item.SaleVAT * item.Qtty), 2),
                    VATBase = (double)Math.Round((item.SalePrice - item.SaleVAT) * item.Qtty, 2),
                };

                documentService.AddNewVATRecord(vAT);
            }           

            // генерируем документ
            if (documentService.GenerateDocument(documentType, EDocumentVersionsPrinting.Original, operationData.Payment.PaymentIndex))
            {
                Pages = documentService.ConvertDocumentToImageList().Clone();
            }
            else
            {
               loggerService.ShowDialog("msgErrorDuringReceiptGeneration", "strWarning", UserControls.MessageBoxes.EButtonIcons.Warning);
            }

            IsMainContentVisible = Pages.Count == 0;
        }



        private async Task<bool> CheckDocNumberRepeating(string documentNumber, EDocumentTypes documentType)
        { 
            return await documentsRepository.IsExistDocumentNumberAsync(documentNumber, documentType);
        }

        private bool CheckDocDataAfterOperation(DateTime saleDateTimeOffset, DateTime invoiceDateTimeOffset)
        {
            return DateTime.Compare(invoiceDateTimeOffset, saleDateTimeOffset) < 0 ;
        }

        void RefreshItemList()
        {
            TryToGetSourceCollection();
        }

        private void FilterTimer_Tick(object? sender, EventArgs e)
        {
            Debug.WriteLine("TryToFilter -" + FilterString);

            if (isFiltring)
            {
                Debug.WriteLine("Already Filtring");
                return;
            }

            if (lastFilterString == FilterString)
            {
                Debug.WriteLine("No Changes");
                return;
            }

            FilterSourceCollection();
            lastFilterString = FilterString;
        }
    }

    public class DocumentItem: ReactiveObject
    {
        #region Fields
        private string sale;
        private string saleDateString;
        private DateTime saleDateTimeOffset;
        private PartnerModel client;
        private string city;
        private string address;
        private string phone;
        private string amount;
        private string tempDocumentNumber;
        private string invoiceNumber;
        private string invoiceDateString;
        private DateTime invoiceDateTimeOffset;        
        private string invoicePrepared;
        private string receiver;
        private string dealDateString;
        private DateTime dealDateTimeOffset;
        private string dealPlace;
        private string description;
        private OperationHeader oh;
        private Document? document;
        #endregion

        #region Properties
        public string Sale { get => sale; set => this.RaiseAndSetIfChanged(ref sale, value); }
        public string SaleDateString { get => saleDateString; set => this.RaiseAndSetIfChanged(ref saleDateString, value); }
        public DateTime SaleDateTimeOffset
        {
            get => saleDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref saleDateTimeOffset, value);
                if (value != null)
                {
                    SaleDateString = ((DateTime)SaleDateTimeOffset).Date.ToString("dd.MM.yyyy");
                }
            }
        }
        public string Amount { get => amount; set => this.RaiseAndSetIfChanged(ref amount, value); }
        public string TempDocumentNumber
        {
            get => tempDocumentNumber;
            set
            {
                this.RaiseAndSetIfChanged(ref tempDocumentNumber, value);
            }
        }
        public string InvoiceNumber
        {
            get => invoiceNumber;
            set
            {
                this.RaiseAndSetIfChanged(ref invoiceNumber, value);
            }
        }
        public string InvoiceDateString { get =>  invoiceDateString; set => this.RaiseAndSetIfChanged(ref invoiceDateString, value); }
        public DateTime InvoiceDateTimeOffset
        {
            get => invoiceDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref invoiceDateTimeOffset, value);
                if (value != null)
                {
                    InvoiceDateString = ((DateTime)InvoiceDateTimeOffset).Date.ToString("dd.MM.yyyy");
                }
                if (string.IsNullOrEmpty(InvoiceNumber))
                {
                    InvoiceDateString = "";
                }
            }
        }
        public string InvoicePrepared { get => invoicePrepared; set => this.RaiseAndSetIfChanged(ref invoicePrepared, value); }
        public PartnerModel Client
        {
            get => client;
            set => this.RaiseAndSetIfChanged(ref client, value);
        }
        public string City { get => city; set => this.RaiseAndSetIfChanged(ref city, value); }
        public string Address { get => address; set => this.RaiseAndSetIfChanged(ref address, value); }
        public string Phone { get => phone; set => this.RaiseAndSetIfChanged(ref phone, value); }
        public string Receiver { get => receiver; set => this.RaiseAndSetIfChanged(ref receiver, value); }
        public string DealDateString { get => dealDateString; set => this.RaiseAndSetIfChanged(ref dealDateString, value); }
        public DateTime DealDateTimeOffset
        {
            get => dealDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref dealDateTimeOffset, value);
                if (value != null)
                {
                    DealDateString = ((DateTime)DealDateTimeOffset).Date.ToString("dd.MM.yyyy");
                }
            }
        }
        public string DealPlace { get => dealPlace; set =>  this.RaiseAndSetIfChanged(ref dealPlace, value);}
        public string Description { get => description; set => this.RaiseAndSetIfChanged(ref description, value);}
        public OperationHeader OperationHeader { get => oh; set => this.RaiseAndSetIfChanged(ref oh, value); }
        public Document? Document
        {
            get => document;
            set 
            {
                this.RaiseAndSetIfChanged(ref document, value);
                this.RaisePropertyChanged(nameof(HasDocument));
            }
        }
        public bool HasDocument { get => Document != null; }
        #endregion

        #region Commands

        #endregion
       

        public DocumentItem(OperationHeader oh, Document? doc)
        {
            OperationHeader = oh;
            Document = doc;

            Sale = oh.Acct.ToString();
            SaleDateTimeOffset = oh.Date;
            Client = oh.Partner;
            Amount = oh.OperationDetails.Sum(od => od.Qtty * od.SalePrice).ToString("F");

            InvoiceDateTimeOffset = DateTime.Now;
            DealDateTimeOffset = DateTime.Now;

            if (doc != null)
            {
                InvoicePrepared = doc.CreatorName;
                Receiver = doc.RecipientName;
        
                InvoiceNumber = doc.DocumentNumber;
                TempDocumentNumber = doc.DocumentNumber;
                InvoiceDateTimeOffset = doc.DocumentDate;
                DealDateTimeOffset = doc.TaxDate;
                DealPlace = doc.DealLocation;
                Description = doc.DealDescription;
            }           
        }
    }
}
