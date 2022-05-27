using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public abstract class DocumentViewModel : ViewModelBase
    {
        protected abstract EDocumentTypes documentType { get; }


        public string TestString { get => "IamHere"; }

        private DocumentItem selectedItem;
        public DocumentItem SelectedItem {
            get=>selectedItem;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedItem, value);
            }
        }
        public ObservableCollection<DocumentItem> Items { get; set; }

        public DocumentViewModel()
        {
            Items = new ObservableCollection<DocumentItem>() { new DocumentItem(1), new DocumentItem(2), new DocumentItem(3) };
            Items = new ObservableCollection<DocumentItem>() { new DocumentItem(4), new DocumentItem(5), new DocumentItem(6) };
            SelectedItem = Items[0];
            Items.Add(new DocumentItem(99));
        }
    }

    public class DocumentItem
    {
        private string sale;
        private string saleDate;
        private string client;
        private string amount;
        private string invoice;
        private string invoiceDate;

        public string Sale { get => sale; set => sale = value; }
        public string SaleDate { get => saleDate; set => saleDate = value; }
        public string Client { get => client; set => client = value; }
        public string Amount { get => amount; set => amount = value; }
        public string Invoice { get => invoice; set => invoice = value; }
        public string InvoiceDate { get => invoiceDate; set => invoiceDate = value; }

        public DocumentItem()
        {
            Sale = "sale";
            SaleDate = "saleDate";
            Client = "client";
            Amount = "amount";
            Invoice = "invoice";
            InvoiceDate = "invoiceDate";
        }

        public DocumentItem(string sale, string saleDate, string client, string amount, string invoice, string invoiceDate)
        {
            Sale = sale;
            SaleDate = saleDate;
            Client = client;
            Amount = amount;
            Invoice = invoice;
            InvoiceDate = invoiceDate;
        }

        public DocumentItem(int x)
        {
            Sale = "sale"+x;
            SaleDate = "saleDate";
            Client = "client";
            Amount = "amount";
            Invoice = "invoice";
            InvoiceDate = "invoiceDate";
        }
    }
}
