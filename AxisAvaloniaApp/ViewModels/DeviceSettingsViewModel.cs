using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Translation;
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

namespace AxisAvaloniaApp.ViewModels
{
    public class DeviceSettingsViewModel : ReactiveObject
    {
        private readonly ISettingsService settingsService;
        private readonly ITranslationService translationService;

        private double titleMinWidth;
        public double TitleMinWidth
        {
            get => titleMinWidth;
            set => this.RaiseAndSetIfChanged(ref titleMinWidth, value);
        }

        private ObservableCollection<ComboBoxItemModel> serialPortItems;
        public ObservableCollection<ComboBoxItemModel> SerialPortItems
        {
            get => serialPortItems;
            set => this.RaiseAndSetIfChanged(ref serialPortItems, value);
        }

        private ComboBoxItemModel selectedSerialPortItem;
        public ComboBoxItemModel SelectedSerialPortItem
        {
            get => selectedSerialPortItem;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedSerialPortItem, value);
            }
        }

        private bool isCheckedFiscalDevice;
        public bool IsCheckedFiscalDevice
        {
            get => isCheckedFiscalDevice;
            set => this.RaiseAndSetIfChanged(ref isCheckedFiscalDevice, value);
        }

        private ObservableCollection<ComboBoxItemModel> fiscalDeviceManufacturers;
        public ObservableCollection<ComboBoxItemModel> FiscalDeviceManufacturers
        {
            get => fiscalDeviceManufacturers;
            set => this.RaiseAndSetIfChanged(ref fiscalDeviceManufacturers, value);
        }

        private ComboBoxItemModel selectedFiscalDeviceManufacturer;
        public ComboBoxItemModel SelectedFiscalDeviceManufacturer
        {
            get => selectedFiscalDeviceManufacturer;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedFiscalDeviceManufacturer, value);
            }
        }

        private ObservableCollection<ComboBoxItemModel> fiscalDeviceModels;
        public ObservableCollection<ComboBoxItemModel> FiscalDeviceModels
        {
            get => fiscalDeviceModels;
            set => this.RaiseAndSetIfChanged(ref fiscalDeviceModels, value);
        }

        private ComboBoxItemModel selectedFiscalDeviceModel;
        public ComboBoxItemModel SelectedFiscalDeviceModel
        {
            get => selectedFiscalDeviceModel;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedFiscalDeviceModel, value);
            }
        }

        private ObservableCollection<ComboBoxItemModel> fiscalDeviceConnectionTypes;
        public ObservableCollection<ComboBoxItemModel> FiscalDeviceConnectionTypes
        {
            get => fiscalDeviceConnectionTypes;
            set => this.RaiseAndSetIfChanged(ref fiscalDeviceConnectionTypes, value);
        }

        private ComboBoxItemModel selectedFiscalDeviceConnectionType;
        public ComboBoxItemModel SelectedFiscalDeviceConnectionType
        {
            get => selectedFiscalDeviceConnectionType;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedFiscalDeviceConnectionType, value);
            }
        }

        private string fiscalDeviceIPAddress;
        public string FiscalDeviceIPAddress
        {
            get => fiscalDeviceIPAddress;
            set => this.RaiseAndSetIfChanged(ref fiscalDeviceIPAddress, value);
        }

        private string fiscalDeviceIPPort;
        public string FiscalDeviceIPPort
        {
            get => fiscalDeviceIPPort;
            set => this.RaiseAndSetIfChanged(ref fiscalDeviceIPPort, value);
        }

        private string operatorCode;
        public string OperatorCode
        {
            get => operatorCode;
            set => this.RaiseAndSetIfChanged(ref operatorCode, value);
        }

        private string operatorPassword;
        public string OperatorPassword
        {
            get => operatorPassword;
            set => this.RaiseAndSetIfChanged(ref operatorPassword, value);
        }



        private bool isCheckedPOSTerminal;
        public bool IsCheckedPOSTerminal
        {
            get => isCheckedPOSTerminal;
            set => this.RaiseAndSetIfChanged(ref isCheckedPOSTerminal, value);
        }


        private ObservableCollection<ComboBoxItemModel> posTerminalManufacturers;
        public ObservableCollection<ComboBoxItemModel> POSTerminalManufacturers
        {
            get => posTerminalManufacturers;
            set => this.RaiseAndSetIfChanged(ref posTerminalManufacturers, value);
        }

        private ComboBoxItemModel selectedPOSTerminalManufacturer;
        public ComboBoxItemModel SelectedPOSTerminalManufacturer
        {
            get => selectedPOSTerminalManufacturer;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedPOSTerminalManufacturer, value);
            }
        }

        private ObservableCollection<ComboBoxItemModel> posTerminalModels;
        public ObservableCollection<ComboBoxItemModel> POSTerminalModels
        {
            get => posTerminalModels;
            set => this.RaiseAndSetIfChanged(ref posTerminalModels, value);
        }

        private ComboBoxItemModel selectedPOSTerminalModel;
        public ComboBoxItemModel SelectedPOSTerminalModel
        {
            get => selectedPOSTerminalModel;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedPOSTerminalModel, value);
            }
        }

        private ObservableCollection<ComboBoxItemModel> posTerminalConnectionTypes;
        public ObservableCollection<ComboBoxItemModel> POSTerminalConnectionTypes
        {
            get => posTerminalConnectionTypes;
            set => this.RaiseAndSetIfChanged(ref posTerminalConnectionTypes, value);
        }

        private ComboBoxItemModel selectedPOSTerminalConnectionType;
        public ComboBoxItemModel SelectedPOSTerminalConnectionType
        {
            get => selectedPOSTerminalConnectionType;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedPOSTerminalConnectionType, value);
            }
        }


        private ObservableCollection<ComboBoxItemModel> posTerminalSerialPorts;
        public ObservableCollection<ComboBoxItemModel> POSTerminalSerialPorts
        {
            get => posTerminalSerialPorts;
            set => this.RaiseAndSetIfChanged(ref posTerminalSerialPorts, value);
        }

        private ComboBoxItemModel selectedPOSTerminalSerialPort;
        public ComboBoxItemModel SelectedPOSTerminalSerialPort
        {
            get => selectedPOSTerminalSerialPort;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedPOSTerminalSerialPort, value);
            }
        }

        private ObservableCollection<ComboBoxItemModel> posTerminalSpeeds;
        public ObservableCollection<ComboBoxItemModel> POSTerminalSpeeds
        {
            get => posTerminalSpeeds;
            set => this.RaiseAndSetIfChanged(ref posTerminalSpeeds, value);
        }

        private ComboBoxItemModel selectedPOSTerminalSpeed;
        public ComboBoxItemModel SelectedPOSTerminalSpeed
        {
            get => selectedPOSTerminalSpeed;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedPOSTerminalSpeed, value);
            }
        }

        private string posTerminalIPAddress;
        public string POSTerminalIPAddress
        {
            get => posTerminalIPAddress;
            set => this.RaiseAndSetIfChanged(ref posTerminalIPAddress, value);
        }

        private string posTerminalIPPort;
        public string POSTerminalIPPort
        {
            get => posTerminalIPPort;
            set => this.RaiseAndSetIfChanged(ref posTerminalIPPort, value);
        }



        private bool isCheckedAxisCloud;
        public bool IsCheckedAxisCloud
        {
            get => isCheckedAxisCloud;
            set => this.RaiseAndSetIfChanged(ref isCheckedAxisCloud, value);
        }

        private string axisCloudIPAddress;
        public string AxisCloudIPAddress
        {
            get => axisCloudIPAddress;
            set => this.RaiseAndSetIfChanged(ref axisCloudIPAddress, value);
        }

        private string axisCloudIPPort;
        public string AxisCloudIPPort
        {
            get => axisCloudIPPort;
            set => this.RaiseAndSetIfChanged(ref axisCloudIPPort, value);
        }

        private string axisCloudLogin;
        public string AxisCloudLogin
        {
            get => axisCloudLogin;
            set => this.RaiseAndSetIfChanged(ref axisCloudLogin, value);
        }

        private string axisCloudPassword;
        public string AxisCloudPassword
        {
            get => axisCloudPassword;
            set => this.RaiseAndSetIfChanged(ref axisCloudPassword, value);
        }


        public DeviceSettingsViewModel()
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();

            SerialPortItems = GetSerialPortItemsCollection();

            FiscalDeviceManufacturers = GetFiscalDeviceManufaturersCollection();
            FiscalDeviceModels = GetFiscalDeviceModelsCollection();
            FiscalDeviceConnectionTypes = GetFiscalDeviceConnectionTypesCollection();

            POSTerminalManufacturers = GetPOSTerminalManufaturersCollection();
            POSTerminalModels = GetPOSTerminalModelsCollection();
            POSTerminalConnectionTypes = GetPOSTerminalConnectionTypesCollection();
            POSTerminalSerialPorts = GetPOSTerminalSerialPortsCollection();
            POSTerminalSpeeds = GetPOSTerminalSpeedsCollection();
        }

        private ObservableCollection<ComboBoxItemModel> GetSerialPortItemsCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }

        private ObservableCollection<ComboBoxItemModel> GetFiscalDeviceManufaturersCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }


        private ObservableCollection<ComboBoxItemModel> GetFiscalDeviceModelsCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }

        private ObservableCollection<ComboBoxItemModel> GetFiscalDeviceConnectionTypesCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }


        private ObservableCollection<ComboBoxItemModel> GetPOSTerminalManufaturersCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }

        private ObservableCollection<ComboBoxItemModel> GetPOSTerminalModelsCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }

        private ObservableCollection<ComboBoxItemModel> GetPOSTerminalConnectionTypesCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }

        private ObservableCollection<ComboBoxItemModel> GetPOSTerminalSerialPortsCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }


        private ObservableCollection<ComboBoxItemModel> GetPOSTerminalSpeedsCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }

        public ReactiveCommand<Unit, Unit> ShowChoseIconCommand { get; }

        async void ShowChoseIconDialog()
        {


        }
    }
}
