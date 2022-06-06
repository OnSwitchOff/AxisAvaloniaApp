using AxisAvaloniaApp.UserControls.Models;
using ReactiveUI;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data to set device settings.
    /// </summary>
    public class DeviceSettingsModel : ReactiveObject
    {
        private ComboBoxItemModel manufacturer;
        private ComboBoxItemModel model;
        private ComboBoxItemModel protocol;
        private string serialPort;
        private int baudRate;
        private string iPAddress;
        private int iPPort;
        private string login;
        private string password;
        private int operatorCode;

        /// <summary>
        /// Gets or sets manufacturer of device.
        /// </summary>
        /// <date>22.03.2022.</date>
        public ComboBoxItemModel Manufacturer
        {
            get => this.manufacturer;
            set => this.RaiseAndSetIfChanged(ref this.manufacturer, value);
        }

        /// <summary>
        /// Gets or sets model (name) of device.
        /// </summary>
        /// <date>22.03.2022.</date>
        public ComboBoxItemModel Model
        {
            get => this.model;
            set => this.RaiseAndSetIfChanged(ref this.model, value);
        }

        /// <summary>
        /// Gets or sets protocol to connect to a device (Serial, Lan or Bluetooth).
        /// </summary>
        /// <date>22.03.2022.</date>
        public ComboBoxItemModel Protocol
        {
            get => this.protocol;
            set => this.RaiseAndSetIfChanged(ref this.protocol, value);
        }

        /// <summary>
        /// Gets or sets serial port name to connect by Serial protocol.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string SerialPort
        {
            get => this.serialPort;
            set => this.RaiseAndSetIfChanged(ref this.serialPort, value);
        }

        /// <summary>
        /// Gets or sets baud rate value of a serial port.
        /// </summary>
        /// <date>22.03.2022.</date>
        public int BaudRate
        {
            get => this.baudRate;
            set => this.RaiseAndSetIfChanged(ref this.baudRate, value);
        }

        /// <summary>
        /// Gets or sets IP address (IPv4) to connect by Lan protocol.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string IPAddress
        {
            get => this.iPAddress;
            set => this.RaiseAndSetIfChanged(ref this.iPAddress, value);
        }

        /// <summary>
        /// Gets or sets IP port to connect by Lan protocol.
        /// </summary>
        /// <date>22.03.2022.</date>
        public int IPPort
        {
            get => this.iPPort;
            set => this.RaiseAndSetIfChanged(ref this.iPPort, value);
        }

        /// <summary>
        /// Gets or sets Login to connect to device.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string Login
        {
            get => this.login;
            set => this.RaiseAndSetIfChanged(ref this.login, value);
        }

        /// <summary>
        /// Gets or sets password to connect to device.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string Password
        {
            get => this.password;
            set => this.RaiseAndSetIfChanged(ref this.password, value);
        }

        /// <summary>
        /// Gets or sets code of operator (will be printed on the receipt).
        /// </summary>
        /// <date>22.03.2022.</date>
        public int OperatorCode
        {
            get => this.operatorCode;
            set => this.RaiseAndSetIfChanged(ref this.operatorCode, value);
        }
    }
}
