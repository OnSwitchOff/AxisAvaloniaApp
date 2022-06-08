using Avalonia.Controls;
using Avalonia.Media.Imaging;
using AxisAvaloniaApp.UserControls.Models;
using Microinvest.CommonLibrary.Enums;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels.Settings
{
    public class ObjectSettingsViewModel : ReactiveObject, IVisible
    {
        private double titleMinWidth;
        public double TitleMinWidth
        {
            get => titleMinWidth;
            set => this.RaiseAndSetIfChanged(ref titleMinWidth, value);
        }

        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value);
        }

        private string firm;
        public string Firm
        {
            get => firm;
            set => this.RaiseAndSetIfChanged(ref firm, value);
        }

        private string principal;
        public string Principal
        {
            get => principal;
            set => this.RaiseAndSetIfChanged(ref principal, value);
        }

        private string city;
        public string City
        {
            get => city;
            set => this.RaiseAndSetIfChanged(ref city, value);
        }

        private string address;
        public string Address
        {
            get => address;
            set => this.RaiseAndSetIfChanged(ref address, value);
        }

        private string phone;
        public string Phone
        {
            get => phone;
            set => this.RaiseAndSetIfChanged(ref phone, value);
        }

        private string taxNumber;
        public string TaxNumber
        {
            get => taxNumber;
            set => this.RaiseAndSetIfChanged(ref taxNumber, value);
        }

        private string vat;
        public string Vat
        {
            get => vat;
            set => this.RaiseAndSetIfChanged(ref vat, value);
        }

        private string bankName;
        public string BankName
        {
            get => bankName;
            set => this.RaiseAndSetIfChanged(ref bankName, value);
        }

        private string bankBIC;
        public string BankBIC
        {
            get => bankBIC;
            set => this.RaiseAndSetIfChanged(ref bankBIC, value);
        }

        private string iban;
        public string IBAN
        {
            get => iban;
            set => this.RaiseAndSetIfChanged(ref iban, value);
        }

        private string onlineShopNumber;
        public string OnlineShopNumber
        {
            get => onlineShopNumber;
            set => this.RaiseAndSetIfChanged(ref onlineShopNumber, value);
        }

        private string iconSource;
        public string IconSource
        {
            get => iconSource;
            set => this.RaiseAndSetIfChanged(ref iconSource, value);
        }

        private ObservableCollection<ComboBoxItemModel> onlineShopTypes;
        public ObservableCollection<ComboBoxItemModel> OnlineShopTypes
        {
            get => onlineShopTypes;
            set => this.RaiseAndSetIfChanged(ref onlineShopTypes, value);
        }

        private ComboBoxItemModel selectedOnlineShopType;
        public ComboBoxItemModel SelectedOnlineShopType
        {
            get => selectedOnlineShopType;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedOnlineShopType, value);
            }
        }


        private string onlineShopDomainName;
        public string OnlineShopDomainName
        {
            get => onlineShopDomainName;
            set => this.RaiseAndSetIfChanged(ref onlineShopDomainName, value);
        }

        public ObjectSettingsViewModel()
        {
            TitleMinWidth = 0;
            Firm = "firm";
            Principal = "principal";
            City = "city";
            Address = "address";
            Phone = "phone";
            TaxNumber = "MyTaxNumber";
            Vat = "vat";
            BankName = "bankName";
            BankBIC = "bankBIC";
            IBAN = "iBAN";
            OnlineShopNumber = "onlineShopNumber";
            OnlineShopTypes = GetOnlineShopTypesCollection();
            SelectedOnlineShopType = OnlineShopTypes[0];
            OnlineShopDomainName = "onlineShopDomainName";
            IconSource = "/Assets/AxisIcon.ico";
            ShowChoseIconCommand = ReactiveCommand.Create(ShowChoseIconDialog);
        }


        private ObservableCollection<ComboBoxItemModel> GetOnlineShopTypesCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();
            Enum.GetValues(typeof(EOnlineShopTypes)).Cast<EOnlineShopTypes>().ToList().ForEach(x => result.Add(new ComboBoxItemModel(x)));
            return result;
        }

        public ReactiveCommand<Unit, Unit> ShowChoseIconCommand { get; }

        async void ShowChoseIconDialog()
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "PNG", Extensions = { "png" } });
            string[]? filePath = await dialog.ShowAsync(new Window());

            if (filePath != null)
            {
                IconSource = filePath[0];
            }
        }
    }
}
