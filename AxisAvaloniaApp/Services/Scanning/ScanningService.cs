namespace AxisAvaloniaApp.Services.Scanning
{
    /// <summary>
    /// Describes scanning service.
    /// </summary>
    public class ScanningService : IScanningData
    {
        private readonly Settings.ISettingsService settingsService;
        private Microinvest.DeviceService.COMScannerService comScanner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanningService"/> class.
        /// </summary>
        /// <param name="settingsService">Service to get data of settings.</param>
        public ScanningService(Settings.ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            this.comScanner = new Microinvest.DeviceService.COMScannerService();
            this.comScanner.SendScannedBarcode += this.SendScannedBarcode;
        }

        /// <summary>
        /// SendScannedData event.
        /// </summary>
        /// <date>16.03.2022.</date>
        public event SendScannedBarcodeDelegate SendScannedDataEvent;

        /// <summary>
        /// Starts listening of COM port.
        /// </summary>
        /// <param name="cOMPort">COM port to listen.</param>
        //// <date>16.03.2022.</date>
        public void StartCOMScanner(string cOMPort)
        {
            try
            {
                this.comScanner?.StartComPortListener(cOMPort);
            }
            catch
            {
                this.settingsService.COMScannerSettings[Enums.ESettingKeys.ComPort].Value = string.Empty;
                this.settingsService.UpdateSettings(Enums.ESettingGroups.COMScanner);
            }
        }

        /// <summary>
        /// Stop listenin of COM port.
        /// </summary>
        //// <date>16.03.2022.</date>
        public void StopCOMScanner()
        {
            this.comScanner?.StopComPortListener();
        }

        /// <summary>
        /// Invoke SendScannedData event if data was received.
        /// </summary>
        /// <param name="barcode">Got data.</param>
        //// <date>16.03.2022.</date>
        private void SendScannedBarcode(string barcode)
        {
            if (this.SendScannedDataEvent != null)
            {
                this.SendScannedDataEvent.Invoke(barcode);
            }
        }
    }
}
