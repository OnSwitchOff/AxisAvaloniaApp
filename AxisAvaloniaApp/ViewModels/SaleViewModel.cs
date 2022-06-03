using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Serialization;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.UserControls.Models;
using DataBase.Repositories.ApplicationLog;
using Microinvest.CommonLibrary.Enums;
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
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();

            IsCached = false;

            IsMainContentVisible = true;
            IsSaleTitleReadOnly = true;
            IsChoiceOfPartnerEnabled = true;
            IsNomenclaturePanelVisible = true;

            USN = "Test USN";
            OperationItemModel operationItem = new OperationItemModel()
            {
                Code = "test",
                Name = "test11",
                SelectedMeasure = new ItemCodeModel() { Measure = "item" },
                Measures = new ObservableCollection<ItemCodeModel> 
                { 
                    new ItemCodeModel() { Measure = "item" }, 
                    new ItemCodeModel() { Measure = "box" }
                },
            };

            Order = new ObservableCollection<OperationItemModel>();
            Order.Add(operationItem);
            operationItem = new OperationItemModel()
            {
                Code = "test",
                Name = "test12",
                SelectedMeasure = new ItemCodeModel() { Measure = "item" },
                Measures = new ObservableCollection<ItemCodeModel>
                {
                    new ItemCodeModel() { Measure = "item" },
                    new ItemCodeModel() { Measure = "box" }
                },
            };
            Order.Add(operationItem);
            operationItem = new OperationItemModel()
            {
                Code = "test",
                Name = "test23",
                SelectedMeasure = new ItemCodeModel() { Measure = "item" },
                Measures = new ObservableCollection<ItemCodeModel>
                {
                    new ItemCodeModel() { Measure = "item" },
                    new ItemCodeModel() { Measure = "box" }
                },
            };
            Order.Add(operationItem);
            operationItem = new OperationItemModel()
            {
                Code = "test",
                Name = "test24",
                SelectedMeasure = new ItemCodeModel() { Measure = "item" },
                Measures = new ObservableCollection<ItemCodeModel>
                {
                    new ItemCodeModel() { Measure = "item" },
                    new ItemCodeModel() { Measure = "box" }
                },
            };
            Order.Add(operationItem);

            ItemsGroups = new ObservableCollection<GroupModel>();
            PartnersGroups = new ObservableCollection<GroupModel>();
            GroupModel itemGroup = new GroupModel()
            {
                Name = "Group 1",
                SubGroups = new ObservableCollection<GroupModel>() { new GroupModel() { Name = "Sub 1"} },
            };
            GroupModel partnerGroup = new GroupModel()
            {
                Name = "Group 1",
                SubGroups = new ObservableCollection<GroupModel>() { new GroupModel() { Name = "Sub 1" } },
            };
            ItemsGroups.Add(itemGroup);
            PartnersGroups.Add(partnerGroup);
            itemGroup = new GroupModel()
            {
                Name = "Group 2",
                SubGroups = new ObservableCollection<GroupModel>() { new GroupModel() { Name = "Sub 1" }, new GroupModel() { Name = "Sub 2" } },
            };
            partnerGroup = new GroupModel()
            {
                Name = "Group 2",
                SubGroups = new ObservableCollection<GroupModel>() { new GroupModel() { Name = "Sub 1" }, new GroupModel() { Name = "Sub 2" } },
            };
            ItemsGroups.Add(itemGroup);
            PartnersGroups.Add(partnerGroup);

            SelectedPartnersGroup = PartnersGroups[1].SubGroups[1];


            Partners = new ObservableCollection<PartnerModel>();
            PartnerModel partner = new PartnerModel()
            {
                Name = "Partner 1",
                City = "Sofia",
            };
            Partners.Add(partner);
            partner = new PartnerModel()
            {
                Name = "Partner 2",
                City = "Sofia",
                Address = "123",
            };
            Partners.Add(partner);
            partner = new PartnerModel()
            {
                Name = "Partner 3",
                City = "Burgas",
            };
            Partners.Add(partner);
            OperationPartner = Partners[0];

            Items = new ObservableCollection<ItemModel>();
            ItemModel item = new ItemModel()
            {
                Name = "Item 1",
                Code = "1",
                Price = 13.3M,
                Measure = "kg",
            };
            Items.Add(item);
            item = new ItemModel()
            {
                Name = "Item 2",
                Code = "2",
                Measure = "kg",
                Price = 13.3M,
                VATGroup = new VATGroupModel()
                {
                    Name = "VAT 1",
                },
            };
            Items.Add(item);

            SelectedItem = new ItemModel();

            VATGroups = new ObservableCollection<VATGroupModel>();
            VATGroups.Add(new VATGroupModel()
            {
                Name = "VAT 1",
            });
            VATGroups.Add(new VATGroupModel()
            {
                Name = "VAT 2",
            });
            VATGroups.Add(new VATGroupModel()
            {
                Name = "VAT 3",
            });

            ItemsTypes = new ObservableCollection<ComboBoxItemModel>();
            ItemsTypes.Add(new ComboBoxItemModel()
            {
                Key = "strItemType_Standard",
                Value = EItemTypes.Standard,
            });
            ItemsTypes.Add(new ComboBoxItemModel()
            {
                Key = "strItemType_Excise",
                Value = EItemTypes.Excise,
            });
            ItemsTypes.Add(new ComboBoxItemModel()
            {
                Key = "strItemType_Work",
                Value = EItemTypes.Work,
            });
            ItemsTypes.Add(new ComboBoxItemModel()
            {
                Key = "strItemType_Service",
                Value = EItemTypes.Service,
            });
            ItemsTypes.Add(new ComboBoxItemModel()
            {
                Key = "strItemType_LottaryTicket",
                Value = EItemTypes.LottaryTicket,
            });
            ItemsTypes.Add(new ComboBoxItemModel()
            {
                Key = "strItemType_Payment",
                Value = EItemTypes.Payment,
            });
            ItemsTypes.Add(new ComboBoxItemModel()
            {
                Key = "strItemType_Other",
                Value = EItemTypes.Other,
            });

            Measures = new ObservableCollection<string>() { "item", "kg", "other", };
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

        private PartnerModel operationPartner;

        /// <summary>
        /// Gets or sets partner that is used in the operation.
        /// </summary>
        /// <date>01.07.2022.</date>
        public PartnerModel OperationPartner
        {
            get => operationPartner;
            set => this.RaiseAndSetIfChanged(ref operationPartner, value);
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

        private ObservableCollection<OperationItemModel> order;

        /// <summary>
        /// Gets or sets list with items to buy.
        /// </summary>
        /// <date>26.05.2022.</date>
        public ObservableCollection<OperationItemModel> Order
        {
            get => order;
            set => this.RaiseAndSetIfChanged(ref order, value);
        }

        private ObservableCollection<OperationItemModel> selectedOrderRecord;

        /// <summary>
        /// Gets or sets item that is selected by user.
        /// </summary>
        /// <date>27.05.2022.</date>
        public ObservableCollection<OperationItemModel> SelectedOrderRecord
        {
            get => selectedOrderRecord;
            set => this.RaiseAndSetIfChanged(ref selectedOrderRecord, value);
        }

        private double amountToPay;

        /// <summary>
        /// Gets or sets amount to payment.
        /// </summary>
        /// <date>30.05.2022.</date>
        public double AmountToPay
        {
            get => amountToPay;
            set => this.RaiseAndSetIfChanged(ref amountToPay, value);
        }

        private double change;

        /// <summary>
        /// Gets or sets amount to payment.
        /// </summary>
        /// <date>30.05.2022.</date>
        public double Change
        {
            get => change;
            set => this.RaiseAndSetIfChanged(ref change, value);
        }

        private bool isNomenclaturePanelVisible;

        /// <summary>
        /// 
        /// </summary>
        /// <date>03.06.2022.</date>
        public bool IsNomenclaturePanelVisible
        {
            get => isNomenclaturePanelVisible;
            set => this.RaiseAndSetIfChanged(ref isNomenclaturePanelVisible, value);
        }

        private ObservableCollection<GroupModel> itemsGroups;

        /// <summary>
        /// Gets or sets list with groups of items.
        /// </summary>
        /// <date>31.05.2022.</date>
        public ObservableCollection<GroupModel> ItemsGroups
        {
            get => itemsGroups;
            set => this.RaiseAndSetIfChanged(ref itemsGroups, value);
        }

        private GroupModel selectedItemsGroup;

        /// <summary>
        /// Gets or sets group of items that is selected by user.
        /// </summary>
        /// <date>31.05.2022.</date>
        public GroupModel SelectedItemsGroup
        {
            get => selectedItemsGroup;
            set => this.RaiseAndSetIfChanged(ref selectedItemsGroup, value);
        }

        private ObservableCollection<ItemModel> items;

        /// <summary>
        /// Gets or sets list with items.
        /// </summary>
        /// <date>01.07.2022.</date>
        public ObservableCollection<ItemModel> Items
        {
            get => items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        private ItemModel selectedItem;

        /// <summary>
        /// Gets or sets item that is selected by user.
        /// </summary>
        /// <date>01.07.2022.</date>
        public ItemModel SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        private ObservableCollection<VATGroupModel> vATGroups;

        /// <summary>
        /// Gets or sets list with VAT groups.
        /// </summary>
        /// <date>02.07.2022.</date>
        public ObservableCollection<VATGroupModel> VATGroups
        {
            get => vATGroups;
            set => this.RaiseAndSetIfChanged(ref vATGroups, value);
        }

        /// <summary>
        /// Gets or sets list with types of items.
        /// </summary>
        /// <date>02.07.2022.</date>
        public ObservableCollection<ComboBoxItemModel> ItemsTypes { get; }

        private ObservableCollection<string> measures;

        /// <summary>
        /// Gets or sets list with measures.
        /// </summary>
        /// <date>03.07.2022.</date>
        public ObservableCollection<string> Measures
        {
            get => measures;
            set => this.RaiseAndSetIfChanged(ref measures, value);
        }

        private ObservableCollection<GroupModel> partnersGroups;

        /// <summary>
        /// Gets or sets list with groups of partners.
        /// </summary>
        /// <date>31.05.2022.</date>
        public ObservableCollection<GroupModel> PartnersGroups
        {
            get => partnersGroups;
            set => this.RaiseAndSetIfChanged(ref partnersGroups, value);
        }

        private GroupModel selectedPartnersGroup;

        /// <summary>
        /// Gets or sets group of partners that is selected by user.
        /// </summary>
        /// <date>31.05.2022.</date>
        public GroupModel SelectedPartnersGroup
        {
            get => selectedPartnersGroup;
            set => this.RaiseAndSetIfChanged(ref selectedPartnersGroup, value);
        }

        private ObservableCollection<PartnerModel> partners;

        /// <summary>
        /// Gets or sets list with partners.
        /// </summary>
        /// <date>01.07.2022.</date>
        public ObservableCollection<PartnerModel> Partners
        {
            get => partners;
            set => this.RaiseAndSetIfChanged(ref partners, value);
        }

        private PartnerModel selectedPartner;

        /// <summary>
        /// Gets or sets partner that is selected by user.
        /// </summary>
        /// <date>01.07.2022.</date>
        public PartnerModel SelectedPartner
        {
            get => selectedPartner;
            set => this.RaiseAndSetIfChanged(ref selectedPartner, value);
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

        /// <summary>
        /// Pay the order.
        /// </summary>
        /// <param name="paymentType">Type of payment.</param>
        /// <date>30.05.2022.</date>
        public void PaymentSale(EPaymentTypes paymentType)
        {
            // TODO: initialize method
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>03.06.2022.</date>
        public void AddItem(ItemModel item)
        {
            // TODO: initialize method
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>03.06.2022.</date>
        public void AddItems(System.Collections.IList items)
        {
            // TODO: initialize method
        }

        /// <summary>
        /// Add new nomenclature to database.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>31.05.2022.</date>
        public void AddNewNomenclature(ENomenclatures nomenclature)
        {
            IsNomenclaturePanelVisible = false;
            // TODO: initialize method
        }

        /// <summary>
        /// Update nomenclature in the database.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>31.05.2022.</date>
        public void UpdateNomenclature(ENomenclatures nomenclature)
        {
            IsNomenclaturePanelVisible = false;
            // TODO: initialize method
        }

        /// <summary>
        /// Delete nomenclature from the database.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>31.05.2022.</date>
        public void DeleteNomenclature(ENomenclatures nomenclature)
        {
            // TODO: initialize method
        }

        /// <summary>
        /// Add new code to list with additional codes of item.
        /// </summary>
        /// <date>31.05.2022.</date>
        public void AddAdditionalItemCode()
        {
            SelectedItem.Codes.Add(new ItemCodeModel());
            // TODO: initialize method
        }

        /// <summary>
        /// Delete code from list with additional codes of item.
        /// <param name="itemCode">Code to delete.</param>
        /// </summary>
        /// <date>31.05.2022.</date>
        public void DeleteAdditionalItemCode(ItemCodeModel itemCode)
        {
            SelectedItem.Codes.Remove(itemCode);
            // TODO: initialize method
        }

        /// <summary>
        /// Cancel edit of nomenclature and restore base state.
        /// </summary>
        /// <param name="key">Key to search partner.</param>
        /// <date>03.06.2022.</date>
        public void FindPartner(string key)
        {
            // TODO: initialize method
        }

        /// <summary>
        /// Save editable nomenclature to database.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>03.06.2022.</date>
        public void SaveNomenclature(ENomenclatures nomenclature)
        {
            IsNomenclaturePanelVisible = true;
            // TODO: initialize method
        }

        /// <summary>
        /// Cancel edit of nomenclature and restore base state.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>03.06.2022.</date>
        public void CancelNomenclature(ENomenclatures nomenclature)
        {
            IsNomenclaturePanelVisible = true;
            // TODO: initialize method
        }
    }
}
