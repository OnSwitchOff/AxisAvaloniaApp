using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.AxisCloud;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Payment.Device;
using AxisAvaloniaApp.Services.Scanning;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.Services.Validation;
using AxisAvaloniaApp.UserControls.Models;
using Common.Interfaces;
using Microinvest.DeviceService.Helpers;
using Microinvest.DeviceService.Models;
using PinPadService.Interfaces;
using PrinterService.Interfaces;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AxisAvaloniaApp.ViewModels.Settings
{
    public class DeviceSettingsViewModel : SettingsViewModelBase
    {
        private readonly IValidationService validationService;
        private readonly IScanningData scanningService;
        private readonly IPaymentService paymentService;
        private readonly IAxisCloudService axisCloudService;
        private readonly ITranslationService translationService;
        private readonly string noActivePortKey;
        private ObservableCollection<string> cOMScannerPorts;
        private string selectedComScannerPort;
        private ObservableCollection<ComboBoxItemModel> fiscalDeviceManufacturers;
        private ObservableCollection<ComboBoxItemModel> fiscalDeviceModels;
        private ObservableCollection<ComboBoxItemModel> fiscalDeviceConnectionTypes;
        private ObservableCollection<string> cOMPorts;
        private ObservableCollection<int> baudRates;
        private DeviceSettingsModel fiscalPrinter;
        private ObservableCollection<ComboBoxItemModel> posTerminalManufacturers;
        private ObservableCollection<ComboBoxItemModel> posTerminalModels;
        private ObservableCollection<ComboBoxItemModel> posTerminalConnectionTypes;
        private DeviceSettingsModel pOSTerminal;
        private bool axisCloudIsUsed;
        private string axisCloudIPAddress;
        private int axisCloudIPPort;
        private string axisCloudLogin;
        private string axisCloudPassword;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSettingsViewModel"/> class.
        /// </summary>
        public DeviceSettingsViewModel()
        {
            validationService = Splat.Locator.Current.GetRequiredService<IValidationService>();
            scanningService = Splat.Locator.Current.GetRequiredService<IScanningData>();
            paymentService = Splat.Locator.Current.GetRequiredService<IPaymentService>();
            axisCloudService = Splat.Locator.Current.GetRequiredService<IAxisCloudService>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            noActivePortKey = "strNotActive";

            cOMScannerPorts = new ObservableCollection<string>();
            cOMScannerPorts.Add(noActivePortKey);
            if (settingsService.COMScannerSettings[Enums.ESettingKeys.ComPort].ToString().ToLower().Equals(noActivePortKey))
            {
                selectedComScannerPort = noActivePortKey;
            }

            fiscalPrinter = new DeviceSettingsModel();
            fiscalPrinter.IsUsed = (bool)settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIsUsed];
            fiscalPrinter.PropertyChanged += FiscalPrinter_PropertyChanged;
            pOSTerminal = new DeviceSettingsModel();
            pOSTerminal.IsUsed = (bool)settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceIsUsed];
            pOSTerminal.PropertyChanged += POSTerminal_PropertyChanged;

            fiscalDeviceManufacturers = new ObservableCollection<ComboBoxItemModel>();
            foreach (IPrinterManufacturer manufacturer in FiscalPrinterModel.GetFiscalPrinterManufacturers(settingsService.Country.Convert()))
            {
                fiscalDeviceManufacturers.Add(new ComboBoxItemModel()
                {
                    Key = manufacturer.Name,
                    Value = manufacturer.Manufacturer,
                });

                if (manufacturer.Manufacturer.ToString().Equals(settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceManufacturer]))
                {
                    fiscalPrinter.Manufacturer = fiscalDeviceManufacturers[fiscalDeviceManufacturers.Count - 1];
                }
            }

            cOMPorts = new ObservableCollection<string>();
            foreach (string port in DeviceHelper.GetSerialPorts())
            {
                cOMPorts.Add(port);
                cOMScannerPorts.Add(port);
                if (port.ToLower() == settingsService.FiscalPrinterSettings[Enums.ESettingKeys.ComPort].ToString().ToLower())
                {
                    fiscalPrinter.SerialPort = port;
                }
                if (port.ToLower() == settingsService.POSTerminalSettings[Enums.ESettingKeys.ComPort].ToString().ToLower())
                {
                    pOSTerminal.SerialPort = port;
                }
                if (port.ToLower() == settingsService.COMScannerSettings[Enums.ESettingKeys.ComPort].ToString().ToLower())
                {
                    selectedComScannerPort = port;
                }
            }

            baudRates = new ObservableCollection<int>();
            foreach (int speed in DeviceHelper.GetDefaulfBaudRates())
            {
                baudRates.Add(speed);
                if (settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceBaudRate].ToString().Equals(speed.ToString()))
                {
                    fiscalPrinter.BaudRate = speed;
                }
            }

            posTerminalManufacturers = new ObservableCollection<ComboBoxItemModel>();
            foreach (IPinPadManufacturer manufacturer in POSTerminalModel.GetPOSTerminalManufacturers(settingsService.Country.Convert()))
            {
                posTerminalManufacturers.Add(new ComboBoxItemModel()
                {
                    Key = manufacturer.Name,
                    Value = manufacturer.Manufacturer,
                });

                if (manufacturer.Manufacturer.ToString().Equals(settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceManufacturer]))
                {
                    pOSTerminal.Manufacturer = posTerminalManufacturers[posTerminalManufacturers.Count - 1];
                }
            }

            axisCloudIsUsed = (bool)settingsService.AxisCloudSettings[Enums.ESettingKeys.DeviceIsUsed];
            using (System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                System.Net.IPEndPoint endPoint = socket.LocalEndPoint as System.Net.IPEndPoint;
                axisCloudIPAddress = endPoint.Address.ToString();
            }
            axisCloudIPPort = (int)settingsService.AxisCloudSettings[Enums.ESettingKeys.ComPort];
            axisCloudLogin = settingsService.AxisCloudSettings[Enums.ESettingKeys.UserName];
            axisCloudPassword = settingsService.AxisCloudSettings[Enums.ESettingKeys.Password];

            FiscalPrinter.RegisterValidationData<DeviceSettingsModel, string>(
                nameof(DeviceSettingsModel.IPAddress),
                () =>
                {
                    return !string.IsNullOrEmpty(FiscalPrinter.IPAddress) && !validationService.IsValidIPAddress(FiscalPrinter.IPAddress);
                },
                "msgInvalidIPAddress");
            FiscalPrinter.RegisterValidationData<DeviceSettingsModel, int>(
                nameof(DeviceSettingsModel.IPPort),
                () =>
                {
                    return !validationService.IsValidIPPort(FiscalPrinter.IPPort);
                },
                "msgInvalidIPPort");

            POSTerminal.RegisterValidationData<DeviceSettingsModel, string>(
                nameof(DeviceSettingsModel.IPAddress),
                () =>
                {
                    return !string.IsNullOrEmpty(POSTerminal.IPAddress) && !validationService.IsValidIPAddress(POSTerminal.IPAddress);
                },
                "msgInvalidIPAddress");
            POSTerminal.RegisterValidationData<DeviceSettingsModel, int>(
                nameof(DeviceSettingsModel.IPPort),
                () =>
                {
                    return !validationService.IsValidIPPort(POSTerminal.IPPort);
                },
                "msgInvalidIPPort");

            RegisterValidationData<DeviceSettingsViewModel, int>(
                this, 
                nameof(AxisCloudIPPort), 
                () =>
                {
                    return !validationService.IsValidIPPort(AxisCloudIPPort);
                }, 
                "msgInvalidIPPort");
        }

        /// <summary>
        /// Gets or sets a list of existing COM ports (with mandatory item "Non active").
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<string> COMScannerPorts
        {
            get => cOMScannerPorts;
            set => this.RaiseAndSetIfChanged(ref cOMScannerPorts, value);
        }

        /// <summary>
        /// Gets or sets COM port is selected for the scanner.
        /// </summary>
        /// <date>13.06.2022.</date>
        public string SelectedComScannerPort
        {
            get => selectedComScannerPort;
            set => this.RaiseAndSetIfChanged(ref selectedComScannerPort, value);
        }

        /// <summary>
        /// Gets or sets a list with manufacturers of fiscal printers.
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> FiscalDeviceManufacturers
        {
            get => fiscalDeviceManufacturers;
            set => this.RaiseAndSetIfChanged(ref fiscalDeviceManufacturers, value);
        }

        /// <summary>
        /// Gets or sets a list with fiscal printers.
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> FiscalDeviceModels
        {
            get => fiscalDeviceModels == null ? fiscalDeviceModels = new ObservableCollection<ComboBoxItemModel>() : fiscalDeviceModels;
            set => this.RaiseAndSetIfChanged(ref fiscalDeviceModels, value);
        }

        /// <summary>
        /// Gets or sets a list with types of connection to fiscal printer.
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> FiscalDeviceConnectionTypes
        {
            get => fiscalDeviceConnectionTypes == null ? fiscalDeviceConnectionTypes = new ObservableCollection<ComboBoxItemModel>() : fiscalDeviceConnectionTypes;
            set => this.RaiseAndSetIfChanged(ref fiscalDeviceConnectionTypes, value);
        }

        /// <summary>
        /// Gets or sets a list of existing COM ports.
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<string> COMPorts
        {
            get => cOMPorts;
            set => this.RaiseAndSetIfChanged(ref cOMPorts, value);
        }

        /// <summary>
        /// Gets or sets a list with baud rates.
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<int> BaudRates
        {
            get => baudRates;
            set => this.RaiseAndSetIfChanged(ref baudRates, value);
        }

        /// <summary>
        /// Gets or sets settings of the fiscal printer.
        /// </summary>
        /// <date>13.06.2022.</date>
        public DeviceSettingsModel FiscalPrinter
        {
            get => fiscalPrinter;
            set => this.RaiseAndSetIfChanged(ref fiscalPrinter, value);
        }

        /// <summary>
        /// Gets or sets a list with manufacturers of POS terminals.
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> POSTerminalManufacturers
        {
            get => posTerminalManufacturers;
            set => this.RaiseAndSetIfChanged(ref posTerminalManufacturers, value);
        }

        /// <summary>
        /// Gets or sets a list with POS terminals.
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> POSTerminalModels
        {
            get => posTerminalModels == null ? posTerminalModels = new ObservableCollection<ComboBoxItemModel>() : posTerminalModels;
            set => this.RaiseAndSetIfChanged(ref posTerminalModels, value);
        }

        /// <summary>
        /// Gets or sets a list with types of connerction to POS terminal.
        /// </summary>
        /// <date>13.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> POSTerminalConnectionTypes
        {
            get => posTerminalConnectionTypes == null ? posTerminalConnectionTypes = new ObservableCollection<ComboBoxItemModel>() : posTerminalConnectionTypes;
            set => this.RaiseAndSetIfChanged(ref posTerminalConnectionTypes, value);
        }

        /// <summary>
        /// Gets or sets settings of the POS terminal.
        /// </summary>
        /// <date>13.06.2022.</date>
        public DeviceSettingsModel POSTerminal
        {
            get => pOSTerminal;
            set => this.RaiseAndSetIfChanged(ref pOSTerminal, value);
        }

        /// <summary>
        /// Gets or sets value indicating whether AxisCloud app is used.
        /// </summary>
        /// <date>13.06.2022.</date>
        public bool AxisCloudIsUsed
        {
            get => axisCloudIsUsed;
            set => this.RaiseAndSetIfChanged(ref axisCloudIsUsed, value);
        }

        /// <summary>
        /// Gets or sets IP address to connect to AxisCloud.
        /// </summary>
        /// <date>13.06.2022.</date>
        public string AxisCloudIPAddress
        {
            get => axisCloudIPAddress;
            set => this.RaiseAndSetIfChanged(ref axisCloudIPAddress, value);
        }

        /// <summary>
        /// Gets or sets IP port to connect to AxisCloud.
        /// </summary>
        /// <date>13.06.2022.</date>
        public int AxisCloudIPPort
        {
            get => axisCloudIPPort;
            set => this.RaiseAndSetIfChanged(ref axisCloudIPPort, value);
        }

        /// <summary>
        /// Gets or sets login to connect to AxisCloud.
        /// </summary>
        /// <date>13.06.2022.</date>
        public string AxisCloudLogin
        {
            get => axisCloudLogin;
            set => this.RaiseAndSetIfChanged(ref axisCloudLogin, value);
        }

        /// <summary>
        /// Gets or sets password to connect to AxisCloud.
        /// </summary>
        /// <date>13.06.2022.</date>
        public string AxisCloudPassword
        {
            get => axisCloudPassword;
            set => this.RaiseAndSetIfChanged(ref axisCloudPassword, value);
        }

        /// <summary>
        /// Checks whether fiscal printer is connected.
        /// </summary>
        /// <date>16.06.2022.</date>
        public async void CheckFiscalPrinterConnection()
        {
            var result = await RealDevice.FiscalDeviceIsConnectedAsync(FiscalPrinter, settingsService.AppLanguage, settingsService.Country);
            if (result.IsConnected)
            {
                _ = loggerService.ShowDialog("msgSuccessfulConnection", "", UserControls.MessageBox.EButtonIcons.Success);
            }
            else
            {
                _ = loggerService.ShowDialog1(result.error, translationService.Localize("strError"), UserControls.MessageBox.EButtonIcons.Error);
            }
        }

        /// <summary>
        /// Checks whether POS terminal is connected.
        /// </summary>
        /// <date>16.06.2022.</date>
        public async void CheckPOSTerminalConnection()
        {
            var result = await RealDevice.POSTerminalIsConnectedAsync(POSTerminal, settingsService.AppLanguage, settingsService.Country);
            if (result.IsConnected)
            {
                _ = loggerService.ShowDialog("msgSuccessfulConnection", "", UserControls.MessageBox.EButtonIcons.Success);
            }
            else
            {
                _ = loggerService.ShowDialog1(result.error, translationService.Localize("strError"), UserControls.MessageBox.EButtonIcons.Error);
            }
        }

        /// <summary>
        /// Sets new settings of devices and saves them to database.
        /// </summary>
        /// <date>16.06.2022.</date>
        public async void SaveDeviceSettings()
        {
            try
            {
                settingsService.COMScannerSettings[Enums.ESettingKeys.ComPort].Value = SelectedComScannerPort;
                if (string.IsNullOrEmpty(SelectedComScannerPort) || SelectedComScannerPort.Equals(noActivePortKey))
                {
                    scanningService.StopCOMScanner();
                }
                else
                {
                    scanningService.StartCOMScanner(SelectedComScannerPort);
                }
                settingsService.UpdateSettings(Enums.ESettingGroups.COMScanner);

                settingsService.AxisCloudSettings[Enums.ESettingKeys.DeviceIsUsed].Value = AxisCloudIsUsed.ToString();
                settingsService.AxisCloudSettings[Enums.ESettingKeys.ComPort].Value = AxisCloudIPPort.ToString();
                settingsService.AxisCloudSettings[Enums.ESettingKeys.UserName].Value = AxisCloudLogin;
                settingsService.AxisCloudSettings[Enums.ESettingKeys.Password].Value = AxisCloudPassword;
                if (AxisCloudIsUsed)
                {
                    axisCloudService.StartServiceAsync(AxisCloudIPPort, settingsService);
                }
                else
                {
                    axisCloudService.StopService();
                }
                settingsService.UpdateSettings(Enums.ESettingGroups.AxisCloud);

                if (FiscalPrinter.IsUsed)
                {
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIsUsed].Value = FiscalPrinter.IsUsed.ToString();
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceManufacturer].Value = FiscalPrinter.Manufacturer.Value.ToString();
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceModel].Value = FiscalPrinter.Model.Value.ToString();
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceProtocol].Value = FiscalPrinter.Protocol.Value.ToString();
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.ComPort].Value = FiscalPrinter.SerialPort;
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceBaudRate].Value = FiscalPrinter.BaudRate.ToString();
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPAddress].Value = FiscalPrinter.IPAddress;
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPPort].Value = FiscalPrinter.IPPort.ToString();
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceLogin].Value = FiscalPrinter.Login;
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.OperatorCode].Value = FiscalPrinter.OperatorCode.ToString();
                    settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DevicePassword].Value = FiscalPrinter.Password;

                    settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceIsUsed].Value = POSTerminal.IsUsed.ToString();
                    settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceManufacturer].Value = POSTerminal.Manufacturer.Value.ToString();
                    settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceModel].Value = POSTerminal.Model.Value.ToString();
                    settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceProtocol].Value = POSTerminal.Protocol.Value.ToString();
                    settingsService.POSTerminalSettings[Enums.ESettingKeys.ComPort].Value = POSTerminal.SerialPort;
                    settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceBaudRate].Value = POSTerminal.BaudRate.ToString();
                    settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceIPAddress].Value = POSTerminal.IPAddress;
                    settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceIPPort].Value = POSTerminal.IPPort.ToString();

                    paymentService.SetPaymentTool(new RealDevice(settingsService));
                }
                else
                {
                    paymentService.SetPaymentTool(new NoDevice(settingsService));
                }
                settingsService.UpdateSettings(Enums.ESettingGroups.FiscalPrinter);
                settingsService.UpdateSettings(Enums.ESettingGroups.POSTerminal);                

                await loggerService.ShowDialog("msgSettingsSuccessfullySaved", "", UserControls.MessageBox.EButtonIcons.Success);
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(SaveDeviceSettings));
                await loggerService.ShowDialog("msgErrorDuringConnectingToDeviceOrSavingSettings", "strError", UserControls.MessageBox.EButtonIcons.Error);
            }
        }

        /// <summary>
        /// Updates dependent property when main property was changed.
        /// </summary>
        /// <param name="sender">DeviceSettingsModel</param>
        /// <param name="e">PropertyChangedEventArgs.</param>
        /// <date>15.06.2022.</date>
        private void POSTerminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DeviceSettingsModel.Manufacturer):
                    POSTerminalModels.Clear();

                    foreach (IDefaultPinPadConfiguration model in POSTerminalModel.GetPOSTerminalModels(POSTerminal.Manufacturer.Value.ToString(), settingsService.Country.Convert()))
                    {
                        POSTerminalModels.Add(new ComboBoxItemModel()
                        {
                            Key = model.Name,
                            Value = model.PinPadType,
                        });

                        if (model.PinPadType.ToString().Equals(settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceModel]))
                        {
                            POSTerminal.Model = POSTerminalModels[POSTerminalModels.Count - 1];
                        }
                    }
                    break;
                case nameof(DeviceSettingsModel.Model):
                    POSTerminalConnectionTypes.Clear();

                    foreach (IConfiguration protocol in POSTerminalModel.GetSupportedProtocols(POSTerminal.Manufacturer.Value.ToString(), POSTerminal.Model.Value.ToString()))
                    {
                        POSTerminalConnectionTypes.Add(new ComboBoxItemModel()
                        {
                            Key = protocol.Name,
                            Value = protocol.Type,
                        });

                        if (protocol.Type.ToString().Equals(settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceProtocol]))
                        {
                            POSTerminal.Protocol = POSTerminalConnectionTypes[POSTerminalConnectionTypes.Count - 1];
                        }
                    }

                    if (POSTerminal.Protocol == null && POSTerminalConnectionTypes.Count == 1)
                    {
                        POSTerminal.Protocol = POSTerminalConnectionTypes[0];
                    }
                    break;
                case nameof(DeviceSettingsModel.Protocol):
                    if (POSTerminal.Protocol != null)
                    {
                        IPinPadManufacturer selectedManufacturer = POSTerminalModel.ParseToPOSTerminalManufacturer(POSTerminal.Manufacturer.Value.ToString());
                        IDefaultPinPadConfiguration selectedModel = POSTerminalModel.ParseToPOSTerminalModel(selectedManufacturer, POSTerminal.Model.Value.ToString());

                        switch ((Common.Enums.SupportedCommunicationEnum)POSTerminal.Protocol.Value)
                        {
                            case Common.Enums.SupportedCommunicationEnum.Serial:
                                if (POSTerminal.BaudRate == 0)
                                {
                                    POSTerminal.BaudRate = POSTerminalModel.GetDefaultBaudRate(selectedModel);
                                }
                                break;
                            case Common.Enums.SupportedCommunicationEnum.Lan:
                                POSTerminal.IPAddress = settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceIPAddress];
                                POSTerminal.IPPort = string.IsNullOrEmpty(settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceIPPort]) ?
                                    POSTerminalModel.GetDefaultIPPort(selectedModel) :
                                    int.Parse(settingsService.POSTerminalSettings[Enums.ESettingKeys.DeviceIPPort]);
                                break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Updates dependent property when main property was changed.
        /// </summary>
        /// <param name="sender">DeviceSettingsModel</param>
        /// <param name="e">PropertyChangedEventArgs.</param>
        /// <date>15.06.2022.</date>
        private void FiscalPrinter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            IPrinterManufacturer selectedManufacturer;
            IDefaultPrinterConfiguration selectedPrinter;
            switch (e.PropertyName)
            {
                case nameof(DeviceSettingsModel.Manufacturer):
                    FiscalDeviceModels.Clear();

                    foreach (IDefaultPrinterConfiguration model in FiscalPrinterModel.GetFiscalPrinterModels(fiscalPrinter.Manufacturer.Value.ToString(), settingsService.Country.Convert()))
                    {
                        FiscalDeviceModels.Add(new ComboBoxItemModel()
                        {
                            Key = model.Name,
                            Value = model.PrinterType,
                        });

                        if (model.PrinterType.ToString().Equals(settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceModel]))
                        {
                            FiscalPrinter.Model = FiscalDeviceModels[FiscalDeviceConnectionTypes.Count - 1];
                        }
                    }
                    break;
                case nameof(DeviceSettingsModel.Model):
                    FiscalDeviceConnectionTypes.Clear();

                    selectedManufacturer = FiscalPrinterModel.ParseToPrinterManufacturer(fiscalPrinter.Manufacturer.Value.ToString());
                    selectedPrinter = FiscalPrinterModel.ParseToPrinterModel(selectedManufacturer, fiscalPrinter.Model.Value.ToString());
                    foreach (IConfiguration protocol in FiscalPrinterModel.GetSupportedProtocols(selectedPrinter))
                    {
                        FiscalDeviceConnectionTypes.Add(new ComboBoxItemModel
                        {
                            Key = protocol.Name,
                            Value = protocol.Type,
                        });

                        if (protocol.Type.ToString().Equals(settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceProtocol]))
                        {
                            FiscalPrinter.Protocol = FiscalDeviceConnectionTypes[FiscalDeviceConnectionTypes.Count - 1];
                        }
                    }

                    if (FiscalPrinter.Protocol == null && FiscalDeviceConnectionTypes.Count == 1)
                    {
                        FiscalPrinter.Protocol = FiscalDeviceConnectionTypes[0];
                    }

                    FiscalPrinter.Login = string.IsNullOrEmpty(settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceLogin]) ? selectedPrinter.UserCode : settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceLogin];
                    FiscalPrinter.Password = string.IsNullOrEmpty(settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DevicePassword]) ? selectedPrinter.UserPassword : settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DevicePassword];
                    break;
                case nameof(DeviceSettingsModel.Protocol):
                    if (FiscalPrinter.Protocol != null)
                    {
                        selectedManufacturer = FiscalPrinterModel.ParseToPrinterManufacturer(fiscalPrinter.Manufacturer.Value.ToString());
                        selectedPrinter = FiscalPrinterModel.ParseToPrinterModel(selectedManufacturer, fiscalPrinter.Model.Value.ToString());

                        switch ((Common.Enums.SupportedCommunicationEnum)FiscalPrinter.Protocol.Value)
                        {
                            case Common.Enums.SupportedCommunicationEnum.Serial:
                                if (FiscalPrinter.BaudRate == 0)
                                {
                                    FiscalPrinter.BaudRate = FiscalPrinterModel.GetDefaultBaudRate(selectedPrinter);
                                }
                                break;
                            case Common.Enums.SupportedCommunicationEnum.Lan:
                                FiscalPrinter.IPAddress = settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPAddress];
                                FiscalPrinter.IPPort = string.IsNullOrEmpty(settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPPort]) ?
                                    FiscalPrinterModel.GetDefaultIPPort(selectedPrinter) :
                                    int.Parse(settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPPort]);
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
