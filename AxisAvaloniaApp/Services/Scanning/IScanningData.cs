namespace AxisAvaloniaApp.Services.Scanning
{
    /// <summary>
    /// Delegate describes structure of SendScannedBarcode event.
    /// </summary>
    /// <param name="barcode">Sent barcode.</param>
    /// <date>16.03.2022.</date>
    public delegate void SendScannedBarcodeDelegate(string barcode);

    /// <summary>
    /// Describes data to use COM scanner.
    /// </summary>
    public interface IScanningData
    {
        /// <summary>
        /// SendScannedData event.
        /// </summary>
        /// <date>16.03.2022.</date>
        event SendScannedBarcodeDelegate SendScannedDataEvent;

        /// <summary>
        /// Starts listening of COM port.
        /// </summary>
        /// <param name="cOMPort">COM port to listen.</param>
        /// <date>16.03.2022.</date>
        void StartCOMScanner(string cOMPort);

        /// <summary>
        /// Stops listening of COM port.
        /// </summary>
        /// <date>16.03.2022.</date>
        void StopCOMScanner();
    }
}
