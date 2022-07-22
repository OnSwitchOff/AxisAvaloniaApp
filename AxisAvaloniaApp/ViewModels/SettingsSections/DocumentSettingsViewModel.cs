using Avalonia.Controls;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Payment.Device;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.UserControls.Models;
using Microinvest.CommonLibrary.Enums;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.ViewModels.Settings
{
    public class DocumentSettingsViewModel : SettingsViewModelBase
    {
        private readonly ITranslationService translationService;
        private readonly IPaymentService paymentService;
        private string usn;
        private string operatorCode;
        private string userName;
        private ObservableCollection<ComboBoxItemModel> languages;
        private ComboBoxItemModel selectedLanguage;
        private string documentHeaderSource;
        private string documentFooterSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSettingsViewModel"/> class.
        /// </summary>
        public DocumentSettingsViewModel()
        {
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            paymentService = Splat.Locator.Current.GetRequiredService<IPaymentService>();

            USN = ((int)settingsService.AppSettings[Enums.ESettingKeys.UniqueSaleNumber]).ToString("D7");
            OperatorCode = settingsService.FiscalPrinterSettings[Enums.ESettingKeys.OperatorCode];
            UserName = settingsService.AppSettings[Enums.ESettingKeys.UserName];
            languages = new ObservableCollection<ComboBoxItemModel>();
            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    languages.Add(item);

                    if (settingsService.AppLanguage == eLang)
                    {
                        SelectedLanguage = item;
                    }
                }
            }

            DocumentHeaderSource = Configurations.AppConfiguration.DocumentHeaderPath;
            DocumentFooterSource = Configurations.AppConfiguration.DocumentFooterPath;
        }

        /// <summary>
        /// Gets or sets unique sale number.
        /// </summary>
        /// <date>09.06.2022.</date>
        public string USN
        {
            get => usn;
            set => this.RaiseAndSetIfChanged(ref usn, value);
        }

        /// <summary>
        /// Gets or sets code of operator to print on a receipt.
        /// </summary>
        /// <date>09.06.2022.</date>
        public string OperatorCode
        {
            get => operatorCode;
            set => this.RaiseAndSetIfChanged(ref operatorCode, value);
        }

        /// <summary>
        /// Gets or sets name of user.
        /// </summary>
        /// <date>09.06.2022.</date>
        public string UserName
        {
            get => userName;
            set => this.RaiseAndSetIfChanged(ref userName, value);
        }

        /// <summary>
        /// Gets or sets list with supported languages.
        /// </summary>
        /// <date>09.06.2022.</date>
        public ObservableCollection<ComboBoxItemModel> Languages
        {
            get => languages;
            set => this.RaiseAndSetIfChanged(ref languages, value);
        }

        /// <summary>
        /// Gets or sets language is selected by user.
        /// </summary>
        /// <date>09.06.2022.</date>
        public ComboBoxItemModel SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedLanguage, value);
            }
        }

        public string DocumentHeaderSource
        {
            get => documentHeaderSource;
            set
            {
                this.RaiseAndSetIfChanged(ref documentHeaderSource, value);

                if (Configurations.AppConfiguration.DocumentHeaderPath !=documentHeaderSource)
                {
                    Configurations.AppConfiguration.DocumentHeaderPath = documentHeaderSource;
                }
            }
        }


        public string DocumentFooterSource
        {
            get => documentFooterSource;
            set
            {
                this.RaiseAndSetIfChanged(ref documentFooterSource, value);

                if (Configurations.AppConfiguration.DocumentFooterPath != documentFooterSource)
                {
                    Configurations.AppConfiguration.DocumentFooterPath = documentFooterSource;
                }
            }
        }


        /// <summary>
        /// Exports log file.
        /// </summary>
        /// <date>09.06.2022.</date>
        private async void ExportLogFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "Text", Extensions = { "txt" } });
            string? filePath = await dialog.ShowAsync(App.MainWindow);

            if (!string.IsNullOrEmpty(filePath))
            {
                System.IO.File.Copy(Configurations.AppConfiguration.LogFilePath, filePath, true);
            }
        }

        /// <summary>
        /// Writes new document data to Database.
        /// </summary>
        /// <date>14.06.2022.</date>
        private async void SaveDocumentSettings()
        {
            try
            {
                settingsService.AppSettings[Enums.ESettingKeys.UniqueSaleNumber].Value = int.Parse(USN).ToString();
                settingsService.FiscalPrinterSettings[Enums.ESettingKeys.OperatorCode].Value = OperatorCode;
                settingsService.AppSettings[Enums.ESettingKeys.UserName].Value = UserName;
                ELanguages newLanguage = (ELanguages)SelectedLanguage.Value;
                if (settingsService.AppLanguage != newLanguage)
                {
                    if ((bool)settingsService.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIsUsed])
                    {
                        paymentService.SetPaymentTool(new RealDevice(settingsService));
                    }
                    else
                    {
                        paymentService.SetPaymentTool(new NoDevice(settingsService));
                    }
                }
                settingsService.AppLanguage = newLanguage;

                settingsService.UpdateSettings(Enums.ESettingGroups.App);
                settingsService.UpdateSettings(Enums.ESettingGroups.FiscalPrinter);

                await loggerService.ShowDialog("msgSettingsSuccessfullySaved", "", UserControls.MessageBoxes.EButtonIcons.Success);
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(SaveDocumentSettings));
                await loggerService.ShowDialog("msgErrorDuringSavingSettings", "strError", UserControls.MessageBoxes.EButtonIcons.Error);
            }
        }


        /// <summary>
        /// Opens FileDialog.
        /// </summary>
        /// <date>09.06.2022.</date>
        public async void ShowChoseDocumentHeaderCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "Image Files", Extensions = { "png", "jpg", "jpeg", "bmp", "ico" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "PNG", Extensions = { "png" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "JPEG", Extensions = { "jpg", "jpeg" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "BMP", Extensions = { "bmp" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "ICO", Extensions = { "ico" } });
            string[]? filePath = await dialog.ShowAsync(App.MainWindow);

            if (filePath != null)
            {
                DocumentHeaderSource = filePath[0];
            }
        }


        /// <summary>
        /// Opens FileDialog.
        /// </summary>
        /// <date>09.06.2022.</date>
        public async void ShowChoseDocumentFooterCommand()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "Image Files", Extensions = { "png", "jpg", "jpeg", "bmp", "ico" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "PNG", Extensions = { "png" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "JPEG", Extensions = { "jpg", "jpeg" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "BMP", Extensions = { "bmp" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "ICO", Extensions = { "ico" } });
            string[]? filePath = await dialog.ShowAsync(App.MainWindow);

            if (filePath != null)
            {
                DocumentFooterSource = filePath[0];
            }
        }
    }
}
