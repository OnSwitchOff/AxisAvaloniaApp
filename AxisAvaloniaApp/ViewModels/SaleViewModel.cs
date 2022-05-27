using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Settings;
using DataBase.Repositories.ApplicationLog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class SaleViewModel : ViewModelBase
    {
        private readonly ISettingsService settingsService;
        public SaleViewModel()
        {
            this.settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();

            IsCached = false;

            IsMainContentVisible = true;
            IsSaleTitleReadOnly = true;
            IsChoiceOfPartnerEnabled = true;

            USN = "Test USN";
            OperationItemModel operationItem = new OperationItemModel()
            {
                Code = "test",
                Name = "test",
            };

            Items = new ObservableCollection<OperationItemModel>();
            Items.Add(operationItem);
            Items.Add(operationItem);
            Items.Add(operationItem);
            Items.Add(operationItem);
        }

        private bool isMainContentVisible;

        /// <summary>
        /// 
        /// </summary>
        /// <date>26.05.2022.</date>
        public bool IsMainContentVisible
        {
            get => isMainContentVisible;
            set => this.RaiseAndSetIfChanged(ref isMainContentVisible, value);
        }

        private bool isSaleTitleReadOnly;

        /// <summary>
        /// 
        /// </summary>
        /// <date>26.05.2022.</date>
        public bool IsSaleTitleReadOnly
        {
            get => isSaleTitleReadOnly;
            set => this.RaiseAndSetIfChanged(ref isSaleTitleReadOnly, value);
        }

        private bool isChoiceOfPartnerEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether field to select partner is enabled.
        /// </summary>
        /// <date>26.05.2022.</date>
        public bool IsChoiceOfPartnerEnabled
        {
            get => isChoiceOfPartnerEnabled;
            set => this.RaiseAndSetIfChanged(ref isChoiceOfPartnerEnabled, value);
        }

        private string selectedPartnerString;

        /// <summary>
        /// Gets or sets a value indicating whether field to select partner is enabled.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string SelectedPartnerString
        {
            get => selectedPartnerString;
            set => this.RaiseAndSetIfChanged(ref selectedPartnerString, value);
        }

        private string uSN;

        /// <summary>
        /// Gets or sets unique sale number.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string USN
        {
            get => uSN;
            set => this.RaiseAndSetIfChanged(ref uSN, value);
        }

        private string? totalAmount = null;

        /// <summary>
        /// Gets or sets amount to pay by document.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string TotalAmount
        {
            get
            {
                if (totalAmount == null)
                {
                    totalAmount = 0.ToString(settingsService.PriceFormat);
                }

                return totalAmount;
            }
            set => this.RaiseAndSetIfChanged(ref totalAmount, value);
        }

        private ObservableCollection<OperationItemModel> items;

        /// <summary>
        /// Gets or sets amount to pay by document.
        /// </summary>
        /// <date>26.05.2022.</date>
        public ObservableCollection<OperationItemModel> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        /// <summary>
        /// Enables field to change title of the view.
        /// </summary>
        /// <date>26.05.2022.</date>
        public void ChangeSaleTitleReadOnly()
        {
            IsSaleTitleReadOnly = !IsSaleTitleReadOnly;
        }

        /// <summary>
        /// Changes property "Enabled" of TextBox to select partner.
        /// </summary>
        /// <date>26.05.2022.</date>
        public void ChangeSelectedPartnerLocked()
        {
            IsChoiceOfPartnerEnabled = !IsChoiceOfPartnerEnabled;
        }
    }
}
