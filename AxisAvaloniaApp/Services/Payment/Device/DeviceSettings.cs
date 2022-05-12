namespace AxisAvaloniaApp.Services.Payment.Device
{
    /// <summary>
    /// Settings to connect to fiscal device and POS terminal.
    /// </summary>
    public class DeviceSettings : Microinvest.DeviceService.Interfaces.IDeviceSettings
    {
        private Settings.ISettingsService settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceSettings"/> class.
        /// </summary>
        /// <param name="settings">Application settings.</param>
        public DeviceSettings(Settings.ISettingsService settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Gets or sets fiscal printer manufacturer name (in according to SupportedPrinterManufacturers enum).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterManufacturer
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceManufacturer];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceManufacturer].Value = value;
        }

        /// <summary>
        /// Gets or sets fiscal printer type (in according to SupportedPrintersEnum enum).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterModel
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceModel];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceModel].Value = value;
        }

        /// <summary>
        /// Gets or sets protocol to connect to a fiscal printer (Serial, Lan or Bluetooth).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterProtocol
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceProtocol];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceProtocol].Value = value;
        }

        /// <summary>
        /// Gets or sets serial port name to connect by Serial protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterSerialPort
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.ComPort];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.ComPort].Value = value;
        }

        /// <summary>
        /// Gets or sets baud rate value of a serial port.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterBaudRate
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceBaudRate];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceBaudRate].Value = value;
        }

        /// <summary>
        /// Gets or sets IP address (IPv4) to connect by Lan protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterIPAddress
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPAddress];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPAddress].Value = value;
        }

        /// <summary>
        /// Gets or sets IP port to connect by Lan protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterIPPort
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPPort];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPPort].Value = value;
        }

        /// <summary>
        /// Gets or sets login to connect to fiscal printer.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterLogin
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceLogin];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceLogin].Value = value;
        }

        /// <summary>
        /// Gets or sets operator code (will be printed on the receipt).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterOperatorCode
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.OperatorCode];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.OperatorCode].Value = value;
        }

        /// <summary>
        /// Gets or sets password to connect to fiscal printer.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string FiscalPrinterPassword
        {
            get => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DevicePassword];
            set => this.settings.FiscalPrinterSettings[Enums.ESettingKeys.DevicePassword].Value = value;
        }

        /// <summary>
        /// Gets or sets POS terminal manufacturer name (in according to SupportedPinPadManufacturers enum).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalManufacturer
        {
            get => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceManufacturer];
            set => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceManufacturer].Value = value;
        }

        /// <summary>
        /// Gets or sets POS terminal type (in according to SupportedPinPadsEnum enum).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalModel
        {
            get => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceModel];
            set => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceModel].Value = value;
        }

        /// <summary>
        /// Gets or sets protocol to connect to POS terminal (Serial, Lan or Bluetooth).
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalProtocol
        {
            get => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceProtocol];
            set => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceProtocol].Value = value;
        }

        /// <summary>
        /// Gets or sets serial port name to connect by Serial protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalSerialPort
        {
            get => this.settings.POSTerminalSettings[Enums.ESettingKeys.ComPort];
            set => this.settings.POSTerminalSettings[Enums.ESettingKeys.ComPort].Value = value;
        }

        /// <summary>
        /// Gets or sets baud rate value of a serial port.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalBaudRate
        {
            get => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceBaudRate];
            set => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceBaudRate].Value = value;
        }

        /// <summary>
        /// Gets or sets IP address (IPv4) to connect by Lan protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalIPAddress
        {
            get => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceIPAddress];
            set => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceIPAddress].Value = value;
        }

        /// <summary>
        /// Gets or sets IP port to connect by Lan protocol.
        /// </summary>
        /// <date>17.03.2022.</date>
        public override string POSTerminalIPPort
        {
            get => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceIPPort];
            set => this.settings.POSTerminalSettings[Enums.ESettingKeys.DeviceIPPort].Value = value;
        }
    }
}
