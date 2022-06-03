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

namespace AxisAvaloniaApp.ViewModels
{
    public abstract class DocumentViewModel : ViewModelBase
    {
        protected abstract EDocumentTypes documentType { get; }

        private DataGridLength saleColumnWidth;
        private ObservableCollection<ComboBoxItemModel> periods;
        private ComboBoxItemModel selectedPeriod;
        private string fromDateString;
        private DateTimeOffset? fromDateTimeOffset;
        private string toDateString;
        private DateTimeOffset? toDateTimeOffset;
        private string filterString;
        private DocumentItem selectedItem;

        public DocumentItem SelectedItem {
            get=>selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);
            }
        }
        public ObservableCollection<DocumentItem> Items { get; set; }
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
        public DateTimeOffset? FromDateTimeOffset
        {
            get => fromDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref fromDateTimeOffset, value);
                if (value != null)
                {
                    FromDateString = ((DateTimeOffset)FromDateTimeOffset).Date.ToString("dd.MM.yyyy");
                }
            }
        }
        public string ToDateString { get => toDateString; set => this.RaiseAndSetIfChanged(ref toDateString, value); }
        public DateTimeOffset? ToDateTimeOffset
        {
            get => toDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref toDateTimeOffset, value);
                if (value != null)
                {
                    ToDateString = ((DateTimeOffset)ToDateTimeOffset).Date.ToString("dd.MM.yyyy");
                }
            }
        }
        public string FilterString { get => filterString; set => this.RaiseAndSetIfChanged(ref filterString, value); }

        public DocumentViewModel()
        {
            Periods = GetPeriodsCollection();
            SelectedPeriod = Periods[0];
            FromDateTimeOffset = new DateTimeOffset(DateTime.Today);
            ToDateTimeOffset = new DateTimeOffset(DateTime.Today);
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
        private string sale;
        private string saleDate;
        private string amount;
        private string invoiceNumber;
        private string invoiceDateString;
        private DateTimeOffset? invoiceDateTimeOffset;    
        private string invoicePrepared;
        private PartnerModel client;
        private string city;
        private string address;
        private string phone;
        private string receiver;
        private string dealDateString;
        private DateTimeOffset? dealDateTimeOffset;
        private string dealPlace;
        private string description;


        public string Sale { get => sale; set => this.RaiseAndSetIfChanged(ref sale, value); }
        public string SaleDate { get => saleDate; set => this.RaiseAndSetIfChanged(ref saleDate, value); }
        public string Amount { get => amount; set => this.RaiseAndSetIfChanged(ref amount, value); }
        public string InvoiceNumber { get => invoiceNumber; set => this.RaiseAndSetIfChanged(ref invoiceNumber, value); }
        public string InvoiceDateString { get => invoiceDateString; set => this.RaiseAndSetIfChanged(ref invoiceDateString, value); }
        public DateTimeOffset? InvoiceDateTimeOffset
        {
            get => invoiceDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref invoiceDateTimeOffset, value);
                if (value != null)
                {
                    InvoiceDateString = ((DateTimeOffset)InvoiceDateTimeOffset).Date.ToString("dd.MM.yyyy");
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
        public DateTimeOffset? DealDateTimeOffset
        {
            get => dealDateTimeOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref dealDateTimeOffset, value);
                if (value != null)
                {
                    DealDateString = ((DateTimeOffset)DealDateTimeOffset).Date.ToString("dd.MM.yyyy");
                }
            }
        }
        public string DealPlace { get => dealPlace; set =>  this.RaiseAndSetIfChanged(ref dealPlace, value);}
        public string Description { get => description; set => this.RaiseAndSetIfChanged(ref description, value);}
       

        public DocumentItem()
        {
            Sale = "sale";
            SaleDate = "saleDate";            
            Amount = "amount";
            InvoiceNumber = "invoice#";
            InvoiceDateTimeOffset = new DateTimeOffset(DateTime.Now);
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
            DealDateTimeOffset = new DateTimeOffset(DateTime.Now);
            DealPlace = "DealPlace";
            Description = "Description";

         
        }

        public DocumentItem(string sale, string saleDate, PartnerModel client, string amount, string invoice, DateTime invoiceDate)
        {
            Sale = sale;
            SaleDate = saleDate;
            Client = client;
            Amount = amount;
            InvoiceNumber = invoice;
            InvoiceDateTimeOffset = new DateTimeOffset(invoiceDate);
        }

        public DocumentItem(int x)
        {


            Sale = "sale"+x;
            SaleDate = "saleDate";
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
            InvoiceDateTimeOffset = new DateTimeOffset(DateTime.Now);
            DealDateTimeOffset = new DateTimeOffset(DateTime.Now);
            DealPlace = "DealPlace";
            Description = "Description";

        }

      
    }
}
