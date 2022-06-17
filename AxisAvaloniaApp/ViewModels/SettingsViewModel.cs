using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Validation;
using AxisAvaloniaApp.UserControls.Models;
using AxisAvaloniaApp.ViewModels.Settings;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class SettingsViewModel : OperationViewModelBase
    {
        private readonly ISettingsService settingsService;

        private ObservableCollection<ComboBoxItemModel> sections;
        private ComboBoxItemModel selectedSection;

        private DeviceSettingsViewModel deviceSettings;
        private DocumentSettingsViewModel documentSettings;
        private MainSettingsViewModel mainSettings;
        private ObjectSettingsViewModel objectSettings;

        public ObservableCollection<ComboBoxItemModel> Sections
        {
            get => sections;
            set => this.RaiseAndSetIfChanged(ref sections, value);
        }
        public ComboBoxItemModel SelectedSection
        {
            get => selectedSection;
            set
            {
                if (SelectedSection != null)
                {
                    ((IVisible)SelectedSection.Value).IsVisible = false;
                }
                this.RaiseAndSetIfChanged(ref selectedSection, value);
                if (SelectedSection != null)
                {
                    ((IVisible)SelectedSection.Value).IsVisible = true;
                }
            }
                
        }
        public DeviceSettingsViewModel DeviceSettings { get => deviceSettings; set => this.RaiseAndSetIfChanged(ref deviceSettings, value); }
        public DocumentSettingsViewModel DocumentSettings { get => documentSettings; set => this.RaiseAndSetIfChanged(ref documentSettings, value); }
        public MainSettingsViewModel MainSettings { get => mainSettings; set => this.RaiseAndSetIfChanged(ref mainSettings, value); }
        public ObjectSettingsViewModel ObjectSettings { get => objectSettings; set => this.RaiseAndSetIfChanged(ref objectSettings, value); }

        public SettingsViewModel()
        {
            this.settingsService = settingsService;

            LoadSettings();
            InitMenu();
            SelectSection();

            //RegisterValidationData<ObjectSettingsViewModel, string>(ObjectSettings, nameof(ObjectSettings.IBAN), () => { return true; }, "strParter");
        }

        private void SelectSection()
        {
            SelectedSection = sections[0];
        }

        private void InitMenu()
        {
            sections = new ObservableCollection<ComboBoxItemModel>();

            ComboBoxItemModel deviceItem = new ComboBoxItemModel() { Key = "strDevices", Value = DeviceSettings };
            ComboBoxItemModel documentItem = new ComboBoxItemModel() { Key = "strDocuments", Value = DocumentSettings };
            ComboBoxItemModel mainItem = new ComboBoxItemModel() { Key = "strBasics", Value = MainSettings };
            ComboBoxItemModel objectItem = new ComboBoxItemModel() { Key = "strFirm", Value = ObjectSettings };

            sections.Add(objectItem);
            sections.Add(documentItem);
            sections.Add(deviceItem);
            sections.Add(mainItem);
        }

        private void LoadSettings()
        {
            DeviceSettings = LoadDeviceSettings();
            DocumentSettings = LoadDocumentSettings();
            MainSettings = LoadMainSettings();
            ObjectSettings = new ObjectSettingsViewModel();
        }

        //private ObjectSettingsViewModel LoadObjectSettings()
        //{
        //    return new ObjectSettingsViewModel();
        //}

        private MainSettingsViewModel LoadMainSettings()
        {
            return new MainSettingsViewModel();
        }

        private DocumentSettingsViewModel LoadDocumentSettings()
        {
            return new DocumentSettingsViewModel();
        }

        private DeviceSettingsViewModel LoadDeviceSettings()
        {
            return new DeviceSettingsViewModel();
        }
    }
}
