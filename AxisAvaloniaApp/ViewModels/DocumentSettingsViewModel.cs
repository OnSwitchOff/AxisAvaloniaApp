using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.UserControls.Models;
using Microinvest.CommonLibrary.Enums;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class DocumentSettingsViewModel : ReactiveObject
    {

        private readonly ISettingsService settingsService;
        private readonly ITranslationService translationService;

        private double titleMinWidth;
        public double TitleMinWidth
        {
            get => titleMinWidth;
            set => this.RaiseAndSetIfChanged(ref titleMinWidth, value);
        }

        private string usn;
        public string USN
        {
            get => usn;
            set => this.RaiseAndSetIfChanged(ref usn, value);
        }

        private string operatorCode;
        public string OperatorCode
        {
            get => operatorCode;
            set => this.RaiseAndSetIfChanged(ref operatorCode, value);
        }

        private string userName;
        public string UserName
        {
            get => userName;
            set => this.RaiseAndSetIfChanged(ref userName, value);
        }

        private ObservableCollection<ComboBoxItemModel> languages;
        public ObservableCollection<ComboBoxItemModel> Languages
        {
            get => languages;
            set => this.RaiseAndSetIfChanged(ref languages, value);
        }

        private ComboBoxItemModel selectedLanguage;
        public ComboBoxItemModel SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedLanguage, value);
            }
        }

        public DocumentSettingsViewModel()
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();

            Languages = GetLanguagessCollection();

        }

        private ObservableCollection<ComboBoxItemModel> GetLanguagessCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            foreach (ELanguages eLang in settingsService.SupportedLanguages)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Value = eLang;

                if (translationService.SupportedLanguages.ContainsKey(eLang.CombineCode))
                {
                    item.Key = translationService.SupportedLanguages[eLang.CombineCode];
                    result.Add(item);
                }
            }
            return result;
        }

        public ReactiveCommand<Unit, Unit> ShowChoseIconCommand { get; }

        async void ShowChoseIconDialog()
        {


        }       
    }
}
