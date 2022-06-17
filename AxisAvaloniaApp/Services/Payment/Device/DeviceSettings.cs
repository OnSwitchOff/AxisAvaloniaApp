namespace AxisAvaloniaApp.Services.Payment.Device
{
    /// <summary>
    /// Settings to connect to fiscal device and POS terminal.
    /// </summary>
    public class DeviceSettings : Microinvest.DeviceService.Interfaces.IDeviceSettings
    {
        private string fiscalPrinterManufacturer;
        private string fiscalPrinterModel;
        private string fiscalPrinterProtocol;
        private string fiscalPrinterSerialPort;
        private string fiscalPrinterBaudRate;
        private string fiscalPrinterIPAddress;
        private string fiscalPrinterIPPort;
        private string fiscalPrinterLogin;
        private string fiscalPrinterOperatorCode;
        private string fiscalPrinterPassword;
        private string pOSTerminalManufacturer;
        private string pOSTerminalModel;
        private string pOSTerminalProtocol;
        private string pOSTerminalSerialPort;
        private string pOSTerminalBaudRate;
        private string pOSTerminalIPAddress;
        private string pOSTerminalIPPort;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSettings"/> class.
        /// </summary>
        /// <param name="settings">Application settings.</param>
        public DeviceSettings(Settings.ISettingsService settings)
        {
            fiscalPrinterManufacturer = settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceManufacturer];
            fiscalPrinterModel = settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceModel];
            fiscalPrinterProtocol = settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceProtocol];
            fiscalPrinterSerialPort = settings.FiscalPrinterSettings[Enums.ESettingKeys.ComPort];
            fiscalPrinterBaudRate = settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceBaudRate];
            fiscalPrinterIPAddress = settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPAddress];
            fiscalPrinterIPPort = settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPPort];
            fiscalPrinterLogin = settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceLogin];
            fiscalPrinterOperatorCode = settings.FiscalPrinterSettings[Enums.ESettingKeys.OperatorCode];
            fiscalPrinterPassword = settings.FiscalPrinterSettings[Enums.ESettingKeys.DevicePassword];

            pOSTerminalManufacturer = settings.POSTerminalSettings[Enums.ESettingKeys.DeviceManufacturer];
            pOSTerminalModel = settings.POSTerminalSettings[Enums.ESettingKeys.DeviceModel];
            pOSTerminalProtocol = settings.POSTerminalSettings[Enums.ESettingKeys.DeviceProtocol];
            pOSTerminalSerialPort = settings.POSTerminalSettings[Enums.ESettingKeys.ComPort];
            pOSTerminalBaudRate = settings.POSTerminalSettings[Enums.ESettingKeys.DeviceBaudRate];
            pOSTerminalIPAddress = settings.POSTerminalSettings[Enums.ESettingKeys.DeviceIPAddress];
            pOSTerminalIPPort = settings.POSTerminalSettings[Enums.ESettingKeys.DeviceIPPort];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSettings"/> class.
        /// </summary>
        /// <param name="printer">Settings of the fiscal printer.</param>
        /// <param name="terminal">Settings of the POS terminal</param>
        public DeviceSettings(Models.DeviceSettingsModel printer, Models.DeviceSettingsModel terminal)
        {
            if (printer != null)
            {
                fiscalPrinterManufacturer = printer.Manufacturer.Value.ToString();
                fiscalPrinterModel = printer.Model.Value.ToString();
                fiscalPrinterProtocol = printer.Protocol.Value.ToString();
                fiscalPrinterSerialPort = printer.SerialPort;
                fiscalPrinterBaudRate = printer.BaudRate.ToString();
                fiscalPrinterIPAddress = printer.IPAddress;
                fiscalPrinterIPPort = printer.IPPort.ToString();
                fiscalPrinterLogin = printer.Login;
                fiscalPrinterOperatorCode = printer.OperatorCode.ToString();
                fiscalPrinterPassword = printer.Password;
            }

            if (terminal != null)
            {
                pOSTerminalManufacturer = terminal.Manufacturer.Value.ToString();
                pOSTerminalModel = terminal.Model.Value.ToString();
                pOSTerminalProtocol = terminal.Protocol.Value.ToString();
                pOSTerminalSerialPort = terminal.SerialPort;
                pOSTerminalBaudRate = terminal.BaudRate.ToString();
                pOSTerminalIPAddress = terminal.IPAddress;
                pOSTerminalIPPort = terminal.IPPort.ToString();
            }
        }

        /// <summary>
        /// Gets or sets fiscal printer manufacturer name (in according to SupportedPrinterManufacturers enum).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterManufacturer
        {
            get => fiscalPrinterManufacturer;
            set => fiscalPrinterManufacturer = value;
        }

        /// <summary>
        /// Gets or sets fiscal printer type (in according to SupportedPrintersEnum enum).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterModel
        {
            get => fiscalPrinterModel;
            set => fiscalPrinterModel = value;
        }

        /// <summary>
        /// Gets or sets protocol to connect to a fiscal printer (Serial, Lan or Bluetooth).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterProtocol
        {
            get => fiscalPrinterProtocol;
            set => fiscalPrinterProtocol = value;
        }

        /// <summary>
        /// Gets or sets serial port name to connect by Serial protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterSerialPort
        {
            get => fiscalPrinterSerialPort;
            set => fiscalPrinterSerialPort = value;
        }

        /// <summary>
        /// Gets or sets baud rate value of a serial port.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterBaudRate
        {
            get => fiscalPrinterBaudRate;
            set => fiscalPrinterBaudRate = value;
        }

        /// <summary>
        /// Gets or sets IP address (IPv4) to connect by Lan protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterIPAddress
        {
            get => fiscalPrinterIPAddress;
            set => fiscalPrinterIPAddress = value;
        }

        /// <summary>
        /// Gets or sets IP port to connect by Lan protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterIPPort
        {
            get => fiscalPrinterIPPort;
            set => fiscalPrinterIPPort = value;
        }

        /// <summary>
        /// Gets or sets login to connect to fiscal printer.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterLogin
        {
            get => fiscalPrinterLogin;
            set => fiscalPrinterLogin = value;
        }

        /// <summary>
        /// Gets or sets operator code (will be printed on the receipt).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterOperatorCode
        {
            get => fiscalPrinterOperatorCode;
            set => fiscalPrinterOperatorCode = value;
        }

        /// <summary>
        /// Gets or sets password to connect to fiscal printer.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterPassword
        {
            get => fiscalPrinterPassword;
            set => fiscalPrinterPassword = value;
        }

        /// <summary>
        /// Gets or sets POS terminal manufacturer name (in according to SupportedPinPadManufacturers enum).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalManufacturer
        {
            get => pOSTerminalManufacturer;
            set => pOSTerminalManufacturer = value;
        }

        /// <summary>
        /// Gets or sets POS terminal type (in according to SupportedPinPadsEnum enum).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalModel
        {
            get => pOSTerminalModel;
            set => pOSTerminalModel = value;
        }

        /// <summary>
        /// Gets or sets protocol to connect to POS terminal (Serial, Lan or Bluetooth).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalProtocol
        {
            get => pOSTerminalProtocol;
            set => pOSTerminalProtocol = value;
        }

        /// <summary>
        /// Gets or sets serial port name to connect by Serial protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalSerialPort
        {
            get => pOSTerminalSerialPort;
            set => pOSTerminalSerialPort = value;
        }

        /// <summary>
        /// Gets or sets baud rate value of a serial port.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalBaudRate
        {
            get => pOSTerminalBaudRate;
            set => pOSTerminalBaudRate = value;
        }

        /// <summary>
        /// Gets or sets IP address (IPv4) to connect by Lan protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalIPAddress
        {
            get => pOSTerminalIPAddress;
            set => pOSTerminalIPAddress = value;
        }

        /// <summary>
        /// Gets or sets IP port to connect by Lan protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalIPPort
        {
            get => pOSTerminalIPPort;
            set => pOSTerminalIPPort = value;
        }
    }
}
