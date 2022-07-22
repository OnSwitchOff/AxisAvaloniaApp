using Avalonia.Controls;
using AxisAvaloniaApp.Configurations;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Zip;
using AxisAvaloniaApp.UserControls.MessageBoxes;
using AxisAvaloniaApp.UserControls.Models;
using AxisAvaloniaApp.ViewModels.Settings;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels.SettingsSections
{
    public class SpecialSettingsViewModel: SettingsViewModelBase
    {
        private readonly IZipService zipService;
        private readonly ILoggerService loggerService;
        private readonly ISettingsService settingsService;


        private bool isAutoBackupEnabled = true;
        private ObservableCollection<ComboBoxItemModel> backupOptions;
        private ComboBoxItemModel selectedBackupOption;
        private double titleMinWidth;

        public bool IsAutoBackUpEnabled
        {
            get => isAutoBackupEnabled;
            set => this.RaiseAndSetIfChanged(ref isAutoBackupEnabled, value);
        }

        public ObservableCollection<ComboBoxItemModel> BackupOptions
        {
            get => backupOptions;
            set => this.RaiseAndSetIfChanged(ref backupOptions, value);
        }

        public ComboBoxItemModel SelectedBackupOption
        {
            get => selectedBackupOption;
            set => this.RaiseAndSetIfChanged(ref selectedBackupOption, value);
        }


  
        public double TitleMinWidth
        {
            get => titleMinWidth;
            set => this.RaiseAndSetIfChanged(ref titleMinWidth, value);
        }

        public ReactiveCommand<Unit, Task<bool>> BackupCommand { get; }
        public ReactiveCommand<Unit, Task<bool>> RestoreCommand { get; }

        public SpecialSettingsViewModel()
        {
            zipService = Splat.Locator.Current.GetRequiredService<IZipService>();
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();

            BackupOptions = LoadBackUpOptions();
            BackupCommand = ReactiveCommand.Create(BackUp);
            RestoreCommand = ReactiveCommand.Create(Restore);
            TitleMinWidth = 250;
        }

        private ObservableCollection<ComboBoxItemModel> LoadBackUpOptions()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();
            result.Add(new ComboBoxItemModel() { Key = "strNotActive", Value = Enums.EBackUpOptions.NOT_ACTIVE });
            result.Add(new ComboBoxItemModel() { Key = "strEveryStart", Value = Enums.EBackUpOptions.EVERY_START });
            SelectedBackupOption = result[0];
            var x = settingsService.AppSettings[Enums.ESettingKeys.BackUpOption];
            foreach (ComboBoxItemModel item in result)
            {
                if (((int)item.Value).ToString() == settingsService.AppSettings[Enums.ESettingKeys.BackUpOption])
                {
                    SelectedBackupOption = item;
                    break;
                }
            }
            return result;
        }

        private async Task<bool> BackUp()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "Zip", Extensions = { "zip" } });
            string? filePath = await dialog.ShowAsync(App.MainWindow);
            if (filePath == null)
            {
                return false;
            }
            if (!zipService.CompressFileToZip(AppConfiguration.DatabaseFullName, filePath, AppConfiguration.DatabaseShortName))
            {
                EButtonResults errResult = await loggerService.ShowDialog("msgErrorDuringSaving", icon: EButtonIcons.Error);
                return false;
            }
            EButtonResults sucRessult = await loggerService.ShowDialog("msgFileWasSaved", icon: EButtonIcons.Success);
            return true;
        }

        private async Task<bool> Restore()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "Zip", Extensions = { "zip" } });
            string[]? filePath = await dialog.ShowAsync(App.MainWindow);
            if (filePath == null || filePath.Length == 0)
            {
                return false;
            }

            bool? extractDbResult = zipService.ExtractDbFromZip(filePath[0], AppConfiguration.DatabaseLocation);

            if (!(bool)extractDbResult)
            {
                EButtonResults errResult = await loggerService.ShowDialog("msgErrorDuringSaving", icon: UserControls.MessageBoxes.EButtonIcons.Error);
                return false;
            }
            if (extractDbResult == null)
            {
                EButtonResults errResult = await loggerService.ShowDialog("msgNoDbFile", icon: UserControls.MessageBoxes.EButtonIcons.Error);
                return false;
            }
            EButtonResults sucRessult = await loggerService.ShowDialog("msgFileWasExtracted", icon: UserControls.MessageBoxes.EButtonIcons.Success);
            return true;
        }


        private async void SaveSpecialSettings()
        {
            try
            {
                settingsService.AppSettings[Enums.ESettingKeys.BackUpOption].Value = ((int)SelectedBackupOption.Value).ToString();
                settingsService.UpdateSettings(Enums.ESettingGroups.App);

                await loggerService.ShowDialog("msgSettingsSuccessfullySaved", "", UserControls.MessageBoxes.EButtonIcons.Success);
            }
            catch (Exception ex)
            {
                loggerService.RegisterError(this, ex, nameof(SaveSpecialSettings));
                await loggerService.ShowDialog("msgErrorDuringSavingSettings", "strError", UserControls.MessageBoxes.EButtonIcons.Error);
            }
        }
    }
}
