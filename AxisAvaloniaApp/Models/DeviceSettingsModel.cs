using AxisAvaloniaApp.UserControls.Models;
using ReactiveUI;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data to set device settings.
    /// </summary>
    public class DeviceSettingsModel : BaseModel
    {
        private bool isUsed;
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
        /// Gets or sets value indicating whether device is used.
        /// </summary>
        /// <date>14.06.2022.</date>
        public bool IsUsed
        {
            get => this.isUsed;
            set => this.RaiseAndSetIfChanged(ref this.isUsed, value);
        }

        /// <summary>
        /// Gets or sets manufacturer of device.
        /// </summary>
        /// <date>22.03.2022.</date>
        public ComboBoxItemModel Manufacturer
        {
            get => manufacturer == null ? manufacturer = new ComboBoxItemModel() : manufacturer;
            set => this.RaiseAndSetIfChanged(ref this.manufacturer, value);
        }

        /// <summary>
        /// Gets or sets model (name) of device.
        /// </summary>
        /// <date>22.03.2022.</date>
        public ComboBoxItemModel Model
        {
            get => model == null ? model = new ComboBoxItemModel() : model;
            set => this.RaiseAndSetIfChanged(ref this.model, value);
        }

        /// <summary>
        /// Gets or sets protocol to connect to a device (Serial, Lan or Bluetooth).
        /// </summary>
        /// <date>22.03.2022.</date>
        public ComboBoxItemModel Protocol
        {
            get => protocol == null ? protocol = new ComboBoxItemModel() : protocol;
            set => this.RaiseAndSetIfChanged(ref this.protocol, value);
        }

        /// <summary>
        /// Gets or sets serial port name to connect by Serial protocol.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string SerialPort
        {
            get => serialPort;
            set => this.RaiseAndSetIfChanged(ref this.serialPort, value);
        }

        /// <summary>
        /// Gets or sets baud rate value of a serial port.
        /// </summary>
        /// <date>22.03.2022.</date>
        public int BaudRate
        {
            get => baudRate;
            set => this.RaiseAndSetIfChanged(ref this.baudRate, value);
        }

        /// <summary>
        /// Gets or sets IP address (IPv4) to connect by Lan protocol.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string IPAddress
        {
            get => iPAddress;
            set => this.RaiseAndSetIfChanged(ref this.iPAddress, value);
        }

        /// <summary>
        /// Gets or sets IP port to connect by Lan protocol.
        /// </summary>
        /// <date>22.03.2022.</date>
        public int IPPort
        {
            get => iPPort;
            set => this.RaiseAndSetIfChanged(ref this.iPPort, value);
        }

        /// <summary>
        /// Gets or sets Login to connect to device.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string Login
        {
            get => login;
            set => this.RaiseAndSetIfChanged(ref this.login, value);
        }

        /// <summary>
        /// Gets or sets password to connect to device.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref this.password, value);
        }

        /// <summary>
        /// Gets or sets code of operator (will be printed on the receipt).
        /// </summary>
        /// <date>22.03.2022.</date>
        public int OperatorCode
        {
            get => operatorCode;
            set => this.RaiseAndSetIfChanged(ref this.operatorCode, value);
        }
    }
}
