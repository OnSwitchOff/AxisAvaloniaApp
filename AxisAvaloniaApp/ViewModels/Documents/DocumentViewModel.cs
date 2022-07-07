using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxisAvaloniaApp.Models;
using Avalonia.Controls;
using AxisAvaloniaApp.UserControls.Models;
using System.Reactive;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.Documents;
using AxisAvaloniaApp.Helpers;
using DataBase.Entities.OperationHeader;
using DataBase.Entities.Documents;

namespace AxisAvaloniaApp.ViewModels
{
    public abstract class DocumentViewModel : OperationViewModelBase
    {
        private readonly IOperationHeaderRepository operationHeaderRepository;
        private readonly IDocumentsRepository documentsRepository;

        protected abstract EDocumentTypes documentType { get; }

        #region MainContent
        private DocumentItem selectedItem;
        private ObservableCollection<DocumentItem> items;
        private ObservableCollection<DocumentItem> filtredItems;
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

        public DocumentItem SelectedItem {
            get=>selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);
            }
        }
        public ObservableCollection<DocumentItem> Items { get => items; set => this.RaiseAndSetIfChanged(ref items, value); }
        public ObservableCollection<DocumentItem> FiltredItems { get => filtredItems; set => this.RaiseAndSetIfChanged(ref filtredItems, value); }


        public ObservableCollection<ComboBoxItemModel> Periods
        {
            get => periods;
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

        private async void TryToGetSourceCollection()
        {
            if (FromDateTimeOffset > ToDateTimeOffset)
            {
                return;
            }

            Items.Clear();
            //foreach (OperationHeader oh in await operationHeaderRepository.GetOperationHeadersByDatesAsync(FromDateTimeOffset, ToDateTimeOffset))
            //{
            //    Items.Add(new DocumentItem(oh, await documentsRepository.GetDocumentsByOperationHeaderAsync(oh, documentType)));
            //}
            
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
                }
            }
        }
        public string FilterString { get => filterString; set => this.RaiseAndSetIfChanged(ref filterString, value); }

        public DocumentViewModel()
        {
            operationHeaderRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
            documentsRepository = Splat.Locator.Current.GetRequiredService<IDocumentsRepository>();
            Items = new ObservableCollection<DocumentItem>();
            Periods = GetPeriodsCollection();
            SelectedPeriod = Periods[0];
            FromDateTimeOffset = DateTime.Today;
            ToDateTimeOffset = DateTime.Today;
            FilterString = "filterstring";
            //Items = new ObservableCollection<DocumentItem>() { new DocumentItem(1), new DocumentItem(2), new DocumentItem(3) };
            Items = new ObservableCollection<DocumentItem>() { new DocumentItem(4), new DocumentItem(5), new DocumentItem(6) };
            SelectedItem = Items[0];
            //Items.Add(new DocumentItem(99));          
        }

        private ObservableCollection<ComboBoxItemModel> GetPeriodsCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            for (int i = 0; i < 5; i++)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = i;
                item.Key = "period" + i;
                result.Add(item);
            }

            return result;
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
        private Task<Document?> task;
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
        public string InvoiceNumber { get => invoiceNumber; set => this.RaiseAndSetIfChanged(ref invoiceNumber, value); }
        public string InvoiceDateString { get => invoiceDateString; set => this.RaiseAndSetIfChanged(ref invoiceDateString, value); }
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
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> PrintCommand { get;}
        #endregion

        public DocumentItem()
        {
            Sale = "sale";
            SaleDateTimeOffset = DateTime.Now;         
            Amount = "amount";
            InvoiceNumber = "invoice#";
            InvoiceDateTimeOffset = DateTime.Now;
            InvoicePrepared = "prep";

            Client = new PartnerModel();
            Client.Address = "Address";
            Client.BankBIC = "BankBIC";
            Client.BankName = "BankName";
            Client.City = "City";
            Client.DiscountCardNumber = "DiscountCardNumber";            
            Client.Email = "Email";
            Client.IBAN = "Iban";
            Client.Id = 0;
            Client.Name = "Name";
            Client.Phone = "Phone";
            Client.Principal = "Principal";
            Client.Status = ENomenclatureStatuses.Active;
            Client.TaxNumber = "TaxNumber";
            Client.VATNumber = "VatNumber";

            Receiver = "Reciever";
            DealDateTimeOffset = DateTime.Now;
            DealPlace = "DealPlace";
            Description = "Description";


            PrintCommand = ReactiveCommand.Create(Print);
        }

        public DocumentItem(string sale, DateTime saleDate, PartnerModel client, string amount, string invoice, DateTime invoiceDate)
        {
            Sale = sale;
            SaleDateTimeOffset = saleDate;
            Client = client;
            Amount = amount;
            InvoiceNumber = invoice;
            InvoiceDateTimeOffset = invoiceDate;
            PrintCommand = ReactiveCommand.Create(Print);
        }

        public DocumentItem(int x)
        {

            Sale = "sale"+x;
            SaleDateTimeOffset = DateTime.Now; ;
            InvoicePrepared = "InvoicePrepared"+x;

            Client = new PartnerModel();
            Client.Address = "Address";
            Client.BankBIC = "BankBIC";
            Client.BankName = "BankName";
            Client.City = "City";
            Client.DiscountCardNumber = "DiscountCardNumber";
            Client.Email = "Email";
            Client.IBAN = "Iban";
            Client.Id = 0;
            Client.Name = "Name";
            Client.Phone = "Phone";
            Client.Principal = "Principal";
            Client.Status = ENomenclatureStatuses.Active;
            Client.TaxNumber = "TaxNumber";
            Client.VATNumber = "VatNumber";

            Receiver = "Reciever";
            Amount = "amount";
            InvoiceNumber = "invoice#";
            InvoiceDateTimeOffset = DateTime.Now;
            DealDateTimeOffset = DateTime.Now;
            DealPlace = "DealPlace";
            Description = "Description";

            PrintCommand = ReactiveCommand.Create(Print);
        }

        public DocumentItem(OperationHeader oh, Document? doc)
        {
            Sale = oh.Acct.ToString();
            SaleDateTimeOffset = oh.Date;
            Client = oh.Partner;
            Amount = oh.OperationDetails.Sum(od => od.Qtty * od.SalePrice).ToString("F");


            if (doc != null)
            {
                InvoicePrepared = doc.CreatorName;
                Receiver = doc.RecipientName;
        
                InvoiceNumber = doc.DocumentNumber;
                InvoiceDateTimeOffset = doc.DocumentDate;
                DealDateTimeOffset = doc.TaxDate;
                DealPlace = doc.DealLocation;
                Description = doc.DealDescription;
            }



            PrintCommand = ReactiveCommand.Create(Print);
        }

        void Print()
        {

        }      
    }
}
