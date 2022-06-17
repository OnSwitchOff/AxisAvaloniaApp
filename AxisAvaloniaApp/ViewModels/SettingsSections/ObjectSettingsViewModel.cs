using Avalonia.Controls;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Validation;
using AxisAvaloniaApp.UserControls.Models;
using Microinvest.CommonLibrary.Enums;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace AxisAvaloniaApp.ViewModels.Settings
{
    public class ObjectSettingsViewModel : SettingsViewModelBase
    {
        private readonly IValidationService validationService;
        private string iconSource;
        private ObservableCollection<ComboBoxItemModel> onlineShopTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectSettingsViewModel"/> class.
        /// </summary>
        public ObjectSettingsViewModel()
        {
            validationService = Splat.Locator.Current.GetRequiredService<IValidationService>();
            
            Company = new CompanyModel();
            Company.Name = settingsService.AppSettings[Enums.ESettingKeys.Company];
            Company.Principal = settingsService.AppSettings[Enums.ESettingKeys.Principal];
            Company.City = settingsService.AppSettings[Enums.ESettingKeys.City];
            Company.Address = settingsService.AppSettings[Enums.ESettingKeys.Address];
            Company.Phone = settingsService.AppSettings[Enums.ESettingKeys.Phone];
            Company.Email = settingsService.AppSettings[Enums.ESettingKeys.Email];
            Company.TaxNumber = settingsService.AppSettings[Enums.ESettingKeys.TaxNumber];
            Company.VATNumber = settingsService.AppSettings[Enums.ESettingKeys.VATNumber];
            Company.BankName = settingsService.AppSettings[Enums.ESettingKeys.BankName];
            Company.BankBIC = settingsService.AppSettings[Enums.ESettingKeys.BankBIC];
            Company.IBAN = settingsService.AppSettings[Enums.ESettingKeys.IBAN];            
            Company.OnlineShopNumber = settingsService.AppSettings[Enums.ESettingKeys.OnlineShopNumber];
            Company.OnlineShopDomainName = settingsService.AppSettings[Enums.ESettingKeys.OnlineShopDomainName];

            OnlineShopTypes = GetOnlineShopTypesCollection((int)settingsService.AppSettings[Enums.ESettingKeys.OnlineShopType]);
            IconSource = Configurations.AppConfiguration.LogoPath;
            ShowChoseIconCommand = ReactiveCommand.Create(ShowChoseIconDialog);


            Company.RegisterValidationData<CompanyModel, string>(
                nameof(CompanyModel.Email), 
                () =>
                { 
                    return !string.IsNullOrEmpty(Company.Email) && !validationService.IsEmail(Company.Email);
                },
                "msgInvalidEmail");
            Company.RegisterValidationData<CompanyModel, string>(
                nameof(CompanyModel.TaxNumber),
                () =>
                {
                    return !string.IsNullOrEmpty(Company.TaxNumber) && !validationService.IsValidTaxNumber(Company.TaxNumber);
                },
                "msgInvalidTaxNumber");
            Company.RegisterValidationData<CompanyModel, string>(
               nameof(CompanyModel.VATNumber),
               () =>
               {
                   return !string.IsNullOrEmpty(Company.VATNumber) && !validationService.IsValidVATNumber(Company.VATNumber);
               },
               "msgInvalidVATNumber");
            Company.RegisterValidationData<CompanyModel, string>(
                nameof(CompanyModel.IBAN),
                () =>
                {
                    return !string.IsNullOrEmpty(Company.IBAN) && !validationService.IsValidIBAN(Company.IBAN);
                },
                "msgInvalidIBAN");
        }

        /// <summary>
        /// Gets or sets data of our company.
        /// </summary>
        /// <date>13.06.2022.</date>
        public CompanyModel Company { get; set; }

        /// <summary>
        /// Gets or sets path to logo of the company.
        /// </summary>
        /// <date>09.06.2022.</date>
        public string IconSource
        {
            get => iconSource;
            set
            {
                this.RaiseAndSetIfChanged(ref iconSource, value);

                if (Configurations.AppConfiguration.LogoPath != IconSource)
                {
                    Configurations.AppConfiguration.LogoPath = IconSource;
                }
            }
        }

        /// <summary>
        /// Gets or sets list with types of online-shop.
        /// </summary>
        /// <date>09.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> OnlineShopTypes
        {
            get => onlineShopTypes;
            set => this.RaiseAndSetIfChanged(ref onlineShopTypes, value);
        }

        /// <summary>
        /// Gets command to change logo of the company.
        /// </summary>
        /// <date>09.06.2022.</date>
        public ReactiveCommand<Unit, Unit> ShowChoseIconCommand { get; }

        /// <summary>
        /// Fills list with types of online-shop.
        /// </summary>
        /// <param name="selectedOnlineShopTypeIndex">Selected type of online-shop.</param>
        /// <returns>returns list with types of online-shop.</returns>
        /// <date>09.06.2022.</date>
        private ObservableCollection<ComboBoxItemModel> GetOnlineShopTypesCollection(int selectedOnlineShopTypeIndex)
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();
            Enum.GetValues(typeof(EOnlineShopTypes)).Cast<EOnlineShopTypes>().ToList().ForEach(x => 
            {
                result.Add(new ComboBoxItemModel(x)); 
                if ((int)x == selectedOnlineShopTypeIndex)
                {
                    Company.ShopType = result[result.Count - 1];
                }
            });

            return result;
        }

        /// <summary>
        /// Opens FileDialog to choose and change logo of the company.
        /// </summary>
        /// <date>09.06.2022.</date>
        private async void ShowChoseIconDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "Image Files", Extensions = { "png", "jpg", "jpeg", "bmp", "ico" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "PNG", Extensions = { "png"} });
            dialog.Filters.Add(new FileDialogFilter() { Name = "JPEG", Extensions = { "jpg", "jpeg"} });
            dialog.Filters.Add(new FileDialogFilter() { Name = "BMP", Extensions = { "bmp" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "ICO", Extensions = { "ico" } });
            string[]? filePath = await dialog.ShowAsync(App.MainWindow);

            if (filePath != null)
            {
                IconSource = filePath[0];
            }
        }

        /// <summary>
        /// Writes new data of the company to Database.
        /// </summary>
        /// <date>13.06.2022.</date>
        private async void SaveObjectSettings()
        {
            try
            {
                settingsService.AppSettings[Enums.ESettingKeys.Company].Value = Company.Name;
                settingsService.AppSettings[Enums.ESettingKeys.Principal].Value = Company.Principal;
                settingsService.AppSettings[Enums.ESettingKeys.City].Value = Company.City;
                settingsService.AppSettings[Enums.ESettingKeys.Address].Value = Company.Address;
                settingsService.AppSettings[Enums.ESettingKeys.Phone].Value = Company.Phone;
                settingsService.AppSettings[Enums.ESettingKeys.Email].Value = Company.Email;
                settingsService.AppSettings[Enums.ESettingKeys.TaxNumber].Value = Company.TaxNumber;
                settingsService.AppSettings[Enums.ESettingKeys.VATNumber].Value = Company.VATNumber;
                settingsService.AppSettings[Enums.ESettingKeys.BankName].Value = Company.BankName;
                settingsService.AppSettings[Enums.ESettingKeys.BankBIC].Value = Company.BankBIC;
                settingsService.AppSettings[Enums.ESettingKeys.IBAN].Value = Company.IBAN;
                if (Company.ShopType != null)
                {
                    settingsService.AppSettings[Enums.ESettingKeys.OnlineShopType].Value = ((int)(EOnlineShopTypes)Company.ShopType.Value).ToString();
                }
                settingsService.AppSettings[Enums.ESettingKeys.OnlineShopNumber].Value = Company.OnlineShopNumber;
                settingsService.AppSettings[Enums.ESettingKeys.OnlineShopDomainName].Value = Company.OnlineShopDomainName;

                settingsService.UpdateSettings(Enums.ESettingGroups.App);

                await loggerService.ShowDialog("msgSettingsSuccessfullySaved", "", UserControls.MessageBox.EButtonIcons.Success);
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(SaveObjectSettings));
                await loggerService.ShowDialog("msgErrorDuringSavingSettings", "strError", UserControls.MessageBox.EButtonIcons.Error);
            }
        }
    }
}
