namespace AxisAvaloniaApp.Enums
{
    /// <summary>
    /// Key to save or update data in the Settings table of the database by groups
    /// </summary>
    public enum ESettingGroups
    {
        /// <summary>
        /// Group of fiscal printer settings
        /// </summary>
        FiscalPrinter,

        /// <summary>
        /// Group of POS terminal settings
        /// </summary>
        POSTerminal,

        /// <summary>
        /// Group of settings to intedrated with Axis-cloud
        /// </summary>
        AxisCloud,

        /// <summary>
        /// Group of COM-scanner settings
        /// </summary>
        COMScanner,

        /// <summary>
        /// Group of current app settings
        /// </summary>
        App,
    }
}
