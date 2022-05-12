using Microinvest.IntegrationService.Interfaces;
using Microinvest.IntegrationService.Models.AxisCloud;
using System;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Services.Settings;

namespace AxisAvaloniaApp.Services.AxisCloud
{
    /// <summary>
    /// Class to prepare and process AxisCloud data.
    /// </summary>
    public class AxisCloudHelper : IAxisCloudIntegration
    {
        private readonly string axisCloudAppName;
        private ISettingsService settingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisCloudHelper"/> class.
        /// </summary>
        /// <param name="settingsService">Settings of the application.</param>
        public AxisCloudHelper(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            this.axisCloudAppName = "AxisCloud";
        }

        /// <summary>
        /// Check if a transaction has been written to the database.
        /// </summary>
        /// <param name="transactionNumber">Transaction number.</param>
        /// <param name="recordId">Id of the database record.</param>
        /// <param name="acctNumber">Document number of the current transaction.</param>
        /// <returns>Returns true if the transaction has been written to a database successfully; otherwise retuns false.</returns>
        /// <date>22.03.2022.</date>
        public bool CheckTransactionNumber(string transactionNumber, out long recordId, out long acctNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check entered user login and user password.
        /// </summary>
        /// <param name="login">Entered user login.</param>
        /// <param name="password">Entered user password.</param>
        /// <returns>Returns true if user login and user password are correct; otherwise returns false.</returns>
        /// <date>22.03.2022.</date>
        public bool CheckUserData(string login, string password)
        {
            return this.settingsService.AxisCloudSettings[Enums.ESettingKeys.UserName] == login && this.settingsService.AxisCloudSettings[Enums.ESettingKeys.Password] == password;
        }

        /// <summary>
        /// Prepare data of the company for AxisCloud.
        /// </summary>
        /// <returns>CompanyModel.</returns>
        /// <date>22.03.2022.</date>
        public CompanyModel GetCompanyData()
        {
            CompanyModel company = new CompanyModel();
            company.Company.Id = 1;
            company.Company.Name = this.settingsService.AppSettings[ESettingKeys.Company];
            company.Company.City = this.settingsService.AppSettings[ESettingKeys.City];
            company.Company.Address = this.settingsService.AppSettings[ESettingKeys.Address];
            company.Company.ContactPerson = this.settingsService.AppSettings[ESettingKeys.Principal];
            company.Company.CountryId = (int)this.settingsService.Country;
            company.Company.CountryCode = this.settingsService.Country.Alpha2Code;
            company.Company.CurrencyCode = this.settingsService.Country.CurrencyCode;
            company.Company.VATNumber = this.settingsService.AppSettings[ESettingKeys.VATNumber];
            company.Company.TaxNumber = this.settingsService.AppSettings[ESettingKeys.TaxNumber];
            company.Company.Phone = this.settingsService.AppSettings[ESettingKeys.Phone];
            company.Company.PricePercision = this.settingsService.Culture.NumberFormat.CurrencyDecimalDigits;
            company.Company.QntPercision = this.settingsService.Culture.NumberFormat.NumberDecimalDigits;
            company.Company.PricesWithVat = true;
            company.Company.IsRegulation18 = this.settingsService.Country == Microinvest.CommonLibrary.Enums.ECountries.Bulgaria;

            return company;
        }

        /// <summary>
        /// Prepare current user data for AxisCloud.
        /// </summary>
        /// <returns>CurrentUserModel.</returns>
        /// <date>22.03.2022.</date>
        public CurrentUserModel GetCurrentUserData()
        {
            CurrentUserModel currentUser = new CurrentUserModel();
            currentUser.CurrentUser.Id = 1;
            currentUser.CurrentUser.Name = this.settingsService.AppSettings[ESettingKeys.Principal];
            currentUser.CurrentUser.Phone = this.settingsService.AppSettings[ESettingKeys.Phone];

            return currentUser;
        }

        /// <summary>
        /// Get the latest sale number for a specific fiscal device.
        /// </summary>
        /// <param name="fiscalDeviceSerialNumber">Serial number of a specific fiscal device.</param>
        /// <returns>The latest sale number.</returns>
        /// <date>22.03.2022.</date>
        public long GetLastSaleNumber(string fiscalDeviceSerialNumber)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare objects data for AxisCloud.
        /// </summary>
        /// <returns>ObjectsModel.</returns>
        /// <date>22.03.2022.</date>
        public ObjectsModel GetObjectsData()
        {
            ObjectsModel objects = new ObjectsModel();
            objects.Objects.Add(new ObjectFields()
            {
                Id = 1,
                Name = this.settingsService.AppSettings[ESettingKeys.Company],
                Address = this.settingsService.AppSettings[ESettingKeys.Address],
                Deleted = false,
            });

            return objects;
        }

        /// <summary>
        /// Prepare partners data for AxisCloud.
        /// </summary>
        /// <returns>PartnersModel.</returns>
        /// <date>22.03.2022.</date>
        public PartnersModel GetPartnersData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare partners group data for AxisCloud.
        /// </summary>
        /// <returns>PartnersGroupsModel.</returns>
        /// <date>22.03.2022.</date>
        public PartnersGroupsModel GetPartnersGroupsData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare payments data for AxisCloud.
        /// </summary>
        /// <returns>PaymentButtonsModel.</returns>
        /// <date>22.03.2022.</date>
        public PaymentButtonsModel GetPaymentTypesData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare products data for AxisCloud.
        /// </summary>
        /// <returns>ProductsModel.</returns>
        /// <date>22.03.2022.</date>
        public ProductsModel GetProductsData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare products group data for AxisCloud.
        /// </summary>
        /// <returns>ProductsGroupsModel.</returns>
        /// <date>22.03.2022.</date>
        public ProductsGroupsModel GetProductsGroupsData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prepare users data for AxisCloud.
        /// </summary>
        /// <returns>UsersModel.</returns>
        /// <date>22.03.2022.</date>
        public UsersModel GetUsersData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write data to database.
        /// </summary>
        /// <param name="operationData">Data for write to database.</param>
        /// <param name="transactionNumber">Generated number of the current transaction.</param>
        /// <param name="recordId">Id of the database record that will be written.</param>
        /// <param name="acctNumber">Document number of the current transaction that will be written.</param>
        /// <returns>Returns true if data has been written successfully; otherwise returns false.</returns>
        /// <date>22.03.2022.</date>
        public bool WriteOperationToDataBase(OperationModel operationData, string transactionNumber, out long recordId, out long acctNumber)
        {
            throw new NotImplementedException();
        }
    }
}
