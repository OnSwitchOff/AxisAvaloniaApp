namespace AxisAvaloniaApp.Enums
{
    /// <summary>
    /// Keys to get or update data in the Settings table of the database
    /// </summary>
    public enum ESettingKeys
    {
        /// <summary>
        /// COM name
        /// </summary>
        ComPort,

        /// <summary>
        /// Device is used flag
        /// </summary>
        DeviceIsUsed,

        /// <summary>
        /// Device manufacturer
        /// </summary>
        DeviceManufacturer,

        /// <summary>
        /// Device model
        /// </summary>
        DeviceModel,

        /// <summary>
        /// Protocol to connect to device
        /// </summary>
        DeviceProtocol,

        /// <summary>
        /// COM port baud rate
        /// </summary>
        DeviceBaudRate,

        /// <summary>
        /// IP address to connect with device
        /// </summary>
        DeviceIPAddress,

        /// <summary>
        /// COM port name to connect with device by lan protocol
        /// </summary>
        DeviceIPPort,

        /// <summary>
        /// Login to connect to device
        /// </summary>
        DeviceLogin,

        /// <summary>
        /// Password to connect to device
        /// </summary>
        DevicePassword,

        /// <summary>
        /// Operator code to print on a receipt
        /// </summary>
        OperatorCode,

        /// <summary>
        /// Unique sale number to print on a receipt
        /// </summary>
        UniqueSaleNumber,

        /// <summary>
        /// Name of the user
        /// </summary>
        UserName,

        /// <summary>
        /// User password
        /// </summary>
        Password,

        /// <summary>
        /// Country for which the app was installed
        /// </summary>
        Country,

        /// <summary>
        /// App language
        /// </summary>
        Language,

        /// <summary>
        /// Database version
        /// </summary>
        DBVersion,

        /// <summary>
        /// Company name
        /// </summary>
        Company,

        /// <summary>
        /// Person in charge of the company
        /// </summary>
        Principal,

        /// <summary>
        /// City where the company is registered or located
        /// </summary>
        City,

        /// <summary>
        /// Address where the company is registered or located
        /// </summary>
        Address,

        /// <summary>
        /// Company phone
        /// </summary>
        Phone,

        /// <summary>
        /// Company e-mail
        /// </summary>
        Email,

        /// <summary>
        /// Company tax number
        /// </summary>
        TaxNumber,

        /// <summary>
        /// Company VAT number
        /// </summary>
        VATNumber,

        /// <summary>
        /// Company bank name
        /// </summary>
        BankName,

        /// <summary>
        /// Company bank BIC
        /// </summary>
        BankBIC,

        /// <summary>
        /// Company IBAN
        /// </summary>
        IBAN,

        /// <summary>
        /// Date of the last database backup
        /// </summary>
        LastBackup,

        /// <summary>
        /// Software version
        /// </summary>
        SoftwareID,

        /// <summary>
        /// Key to verify attempted database hacking
        /// </summary>
        SoftwareVersion,

        /// <summary>
        /// Online shop number that is received from National Revenue Agency
        /// </summary>
        OnlineShopNumber,

        /// <summary>
        /// Type of an online shop
        /// </summary>
        OnlineShopType,

        /// <summary>
        /// Domain name of the online shop or link to one
        /// </summary>
        OnlineShopDomainName,

        /// <summary>
        /// BackUpOption
        /// </summary>
        BackUpOption,
    }
}
