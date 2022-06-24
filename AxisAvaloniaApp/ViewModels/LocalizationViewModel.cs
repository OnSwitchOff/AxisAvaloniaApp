using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.UserControls.Models;
using AxisAvaloniaApp.Views;
using Microinvest.CommonLibrary.Enums;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class LocalizationViewModel : ReactiveObject
    {
        private readonly BehaviorSubject<bool> _allChosedSubject = new BehaviorSubject<bool>(false);
        public bool AllChosedState
        {
            get => _allChosedSubject.Value;
            set => _allChosedSubject.OnNext(value);
        }
        public IObservable<bool> ObservableAllChosedState => _allChosedSubject;

        protected readonly ISettingsService settingsService;

        private ObservableCollection<ComboBoxItemModel> countries;
        private ComboBoxItemModel selectedCountry;
        private ObservableCollection<ComboBoxItemModel> languages;
        private ComboBoxItemModel selectedLanguage;

        public ObservableCollection<ComboBoxItemModel> Countries
        {
            get => countries;
            set => this.RaiseAndSetIfChanged(ref countries, value);
        }

        public ComboBoxItemModel SelectedCountry
        {
            get => selectedCountry;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedCountry, value);
                AllChosedState = SelectedCountry != null && SelectedLanguage != null;
            }
        }

        public ObservableCollection<ComboBoxItemModel> Languages
        {
            get => languages;
            set => this.RaiseAndSetIfChanged(ref languages, value);
        }

        public ComboBoxItemModel SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedLanguage, value);
                AllChosedState = SelectedCountry != null && SelectedLanguage != null;
            }
        }

        public ReactiveCommand<LocalizationView, Unit> ConfirmCommand { get; }

        public LocalizationViewModel()
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            FillCountries();
            FillLangiuages();
            ConfirmCommand = ReactiveCommand.Create<LocalizationView>(Confirm, ObservableAllChosedState);
        }

        private void Confirm(LocalizationView view)
        {
            settingsService.AppLanguage = (ELanguages)SelectedLanguage.Value.GetType().GetProperty("EnumValue").GetValue(SelectedLanguage.Value);
            settingsService.Country = (ECountries)SelectedCountry.Value.GetType().GetProperty("EnumValue").GetValue(SelectedCountry.Value);
            settingsService.UpdateSettings(Enums.ESettingGroups.App);
            view.DialogResult = true;
        }

        private void FillLangiuages()
        {
            Languages = new ObservableCollection<ComboBoxItemModel>();
            foreach (var lang in settingsService.SupportedLanguages)
            {
                Languages.Add(GetLanguageItem(lang));
            }
        }

        private void FillCountries()
        {
            Countries = new ObservableCollection<ComboBoxItemModel>();
            foreach (var country in settingsService.SupportedCountries)
            {
                countries.Add(GetCountryItem(country));
            }
        }

        private ComboBoxItemModel GetCountryItem(ECountries country)
        {
            string key = "str" + country.EnglishName;
            string image = "/Assets/Flags/unknown_flag.png";
            string countryName = country.EnglishName;
            if (country == ECountries.Bulgaria)
            {
                image = "/Assets/Flags/bg.png";
                countryName = "България";
            }
            if (country == ECountries.Ukraine)
            {
                image = "/Assets/Flags/ua.png";
                countryName = "Україна";
            }
            if (country == ECountries.UnitedStates)
            {
                image = "/Assets/Flags/usa.png";
                countryName = country.EnglishName;
            }
            if (country == ECountries.Russia)
            {
                image = "/Assets/Flags/ru.png";
                countryName = "Россия";
            }
            if (country == ECountries.UnitedKingdom)
            {
                image = "/Assets/Flags/uk.png";
                countryName = country.EnglishName;
            }
            if (country == ECountries.Georgia)
            {
                image = "/Assets/Flags/ge.png";
                countryName = country.EnglishName;
            }

            return new ComboBoxItemModel() { Key = key, Value = new { ImageSource = image, CountryName = countryName, EnumValue = country } };
        }

        private ComboBoxItemModel GetLanguageItem(ELanguages lang)
        {
            string key = "str" + lang.EnglishName;
            string image = "/Assets/Flags/unknown_flag.png";
            string langName = lang.EnglishName;
            if (lang == ELanguages.Albanian)
            {
                image = "/Assets/Flags/albanian.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Arabic)
            {
                image = "/Assets/Flags/sasaudiarabia.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Armenian)
            {
                image = "/Assets/Flags/armenian.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Bulgarian)
            {
                image = "/Assets/Flags/bg.png";
                langName = "Български";
            }
            if (lang == ELanguages.English)
            {
                image = "/Assets/Flags/eng.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Finnish)
            {
                image = "/Assets/Flags/finland.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.French)
            {
                image = "/Assets/Flags/france.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Georgian)
            {
                image = "/Assets/Flags/ge.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.German)
            {
                image = "/Assets/Flags/germany.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Greek)
            {
                image = "/Assets/Flags/greece.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Latvian)
            {
                image = "/Assets/Flags/latvia.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Polish)
            {
                image = "/Assets/Flags/poland.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Romanian)
            {
                image = "/Assets/Flags/romania.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Russian)
            {
                image = "/Assets/Flags/ru.png";
                langName = "Руский";
            }
            if (lang == ELanguages.Serbian)
            {
                image = "/Assets/Flags/serbia.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Spanish)
            {
                image = "/Assets/Flags/spain.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Turkish)
            {
                image = "/Assets/Flags/turkey.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Turkmen)
            {
                image = "/Assets/Flags/turkmenistan.png";
                langName = lang.EnglishName;
            }
            if (lang == ELanguages.Ukrainian)
            {
                image = "/Assets/Flags/ua.png";
                langName = "Українська";
            }

            return new ComboBoxItemModel() { Key = key, Value = new { ImageSource = image, LangName = langName, EnumValue = lang } };
        }
    }
}
