using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Serialization;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.UserControls.Models;
using DataBase.Entities.Interfaces;
using DataBase.Entities.Items;
using DataBase.Entities.ItemsGroups;
using DataBase.Entities.PartnersGroups;
using DataBase.Repositories.ApplicationLog;
using DataBase.Repositories.Items;
using DataBase.Repositories.ItemsGroups;
using DataBase.Repositories.Partners;
using DataBase.Repositories.PartnersGroups;
using DataBase.Repositories.VATGroups;
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
    public class SaleViewModel : OperationViewModelBase
    {
        private readonly ISettingsService settingsService;
        private readonly ISerializationService serializationService;
        private readonly IPaymentService paymentService;
        private readonly ITranslationService translationService;
        private readonly ILoggerService loggerService;
        private readonly IPartnerRepository partnerRepository;
        private readonly IItemsGroupsRepository itemsGroupsRepository;
        private readonly IItemRepository itemRepository;
        private readonly IPartnersGroupsRepository partnersGroupsRepository;
        private readonly IVATsRepository vATsRepository;        

        private bool isMainContentVisible;
        private bool isSaleTitleReadOnly;
        private bool isChoiceOfPartnerEnabled;
        private string operationPartnerString;
        private PartnerModel operationPartner;
        private string uSN;
        private string totalAmount;
        private ObservableCollection<OperationItemModel> order;
        private ObservableCollection<OperationItemModel> selectedOrderRecord;
        private double amountToPay;
        private double change;

        private bool isNomenclaturePanelVisible;
        private ObservableCollection<GroupModel> itemsGroups;
        private GroupModel selectedItemsGroup;
        private ObservableCollection<ItemModel> items;
        private ItemModel selectedItem;
        private ObservableCollection<VATGroupModel> vATGroups;
        private ObservableCollection<ComboBoxItemModel> itemsTypes;
        private ObservableCollection<string> measures;
        private ObservableCollection<PartnerModel> partners;
        private PartnerModel selectedPartner;
        private ObservableCollection<GroupModel> partnersGroups;
        private GroupModel selectedPartnersGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleViewModel"/> class.
        /// </summary>
        public SaleViewModel()
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            serializationService = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            serializationService.InitSerializationData(ESerializationGroups.Sale);
            paymentService = Splat.Locator.Current.GetRequiredService<IPaymentService>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            translationService.LanguageChanged += Application_PropertyChanged;
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();

            itemsGroupsRepository = Splat.Locator.Current.GetRequiredService<IItemsGroupsRepository>();
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
            partnersGroupsRepository = Splat.Locator.Current.GetRequiredService<IPartnersGroupsRepository>();
            partnerRepository = Splat.Locator.Current.GetRequiredService<IPartnerRepository>();
            vATsRepository = Splat.Locator.Current.GetRequiredService<IVATsRepository>();

            IsCached = false;

            IsMainContentVisible = true;
            IsSaleTitleReadOnly = true;
            IsNomenclaturePanelVisible = true;

            IsChoiceOfPartnerEnabled = (bool)serializationService[ESerializationKeys.TbPartnerEnabled];
            if ((int)serializationService[ESerializationKeys.TbPartnerID] > 0)
            {
                OperationPartner = partnerRepository.GetPartnerByIdAsync((int)serializationService[ESerializationKeys.TbPartnerID]).GetAwaiter().GetResult();
            }

            USN = paymentService.FiscalDevice.ReceiptNumber;
            TotalAmount = 0.ToString(settingsService.PriceFormat);
            Order.Add(new OperationItemModel());

            ItemsGroups = GetGroups(itemsGroupsRepository.GetItemsGroupsAsync().GetAwaiter().GetResult());
            FillItems();
            FillMeasures();
            PartnersGroups = GetGroups<PartnersGroup>(partnersGroupsRepository.GetPartnersGroupsAsync().GetAwaiter().GetResult());
            FillPartners();

            FillVATs();
        }

        private async void FillMeasures()
        {
            foreach (string measure in await itemRepository.GetMeasuresAsync())
            {
                if (!Measures.Contains(measure))
                {
                    Measures.Add(measure);
                }
            }
        }

        private async void FillVATs()
        {
            await foreach (var vAT in vATsRepository.GetVATGroupsAsync())
            {
                VATGroups.Add(vAT);
            }
        }

        protected override void CloseView()
        {
            translationService.LanguageChanged -= Application_PropertyChanged;

            base.CloseView();
        }

        private void Application_PropertyChanged()
        {
            Measures[0] = translationService.Localize("strMeasureItem");
            Measures[1] = translationService.Localize("strMeasureKilo");
            Measures[2] = translationService.Localize("strMeasureLiter");
            Measures[3] = translationService.Localize("strMeasureBox");
        }

        private async void FillItems()
        {
            await foreach (var item in itemRepository.GetItemsAsync())
            {
                Items.Add((ItemModel)item);
            }
        }

        private async void FillPartners()
        {
            await foreach (var partner in partnerRepository.GetParnersAsync())
            {
                Partners.Add(partner);
            }
        }

        private ObservableCollection<GroupModel> GetGroups<T>(List<T> groups, GroupModel parentGroup = null, int pathLenght = 3, int startIndex = 0, string parentGroupPath = "")
            where T : INomenclaturesGroup
        {
            ObservableCollection<GroupModel> groupsList = new ObservableCollection<GroupModel>();
            GroupModel group;

            for (; startIndex < groups.Count; startIndex++)
            {
                if ((groups[startIndex].Path.Length == pathLenght || groups[startIndex].Path.Equals("-1")) && (string.IsNullOrEmpty(parentGroupPath) || groups[startIndex].Path.StartsWith(parentGroupPath)))
                {
                    group = new GroupModel()
                    { 
                        Id = groups[startIndex].Id,
                        Name = groups[startIndex].Name,
                        Path = groups[startIndex].Path,
                        ParentGroup = parentGroup,
                    };

                    groupsList.Add(group);

                    group.SubGroups = GetGroups(groups, group, pathLenght + 3, startIndex + 1, group.Path);
                }
            }

            return groupsList;
        }

        /// <summary>
        /// Gets service t oserialize/deserialize visual data of the main content (work area).
        /// </summary>
        /// <date>17.06.2022.</date>
        public ISerializationService SerializationService => serializationService;

        /// <summary>
        /// Gets or sets value indicating whether main content (work area) is visible.
        /// </summary>
        /// <date>26.05.2022.</date>
        public bool IsMainContentVisible
        {
            get => isMainContentVisible;
            set => this.RaiseAndSetIfChanged(ref isMainContentVisible, value);
        }

        /// <summary>
        /// Gets or sets value indicating whether title is ReadOnly.
        /// </summary>
        /// <date>26.05.2022.</date>
        public bool IsSaleTitleReadOnly
        {
            get => isSaleTitleReadOnly;
            set => this.RaiseAndSetIfChanged(ref isSaleTitleReadOnly, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether field to select partner is enabled.
        /// </summary>
        /// <date>26.05.2022.</date>
        public bool IsChoiceOfPartnerEnabled
        {
            get => isChoiceOfPartnerEnabled;
            set
            {
                this.RaiseAndSetIfChanged(ref isChoiceOfPartnerEnabled, value);

                if (!value && OperationPartner == null)
                {
                    OperationPartner = partnerRepository.GetPartnerByIdAsync(1).GetAwaiter().GetResult();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether field to select partner is enabled.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string OperationPartnerString
        {
            get => operationPartnerString;
            set => this.RaiseAndSetIfChanged(ref operationPartnerString, value);
        }        

        /// <summary>
        /// Gets or sets partner that is used in the operation.
        /// </summary>
        /// <date>01.07.2022.</date>
        public PartnerModel OperationPartner
        {
            get => operationPartner;
            set
            {
                this.RaiseAndSetIfChanged(ref operationPartner, value);

                if (operationPartner == null)
                {
                    OperationPartnerString = string.Empty;
                }
                else
                {
                    OperationPartnerString = operationPartner.Name;
                }
            }
        }

        /// <summary>
        /// Gets or sets unique sale number.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string USN
        {
            get => uSN;
            set => this.RaiseAndSetIfChanged(ref uSN, value);
        }

        /// <summary>
        /// Gets or sets amount to pay by document.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string TotalAmount
        {
            get => totalAmount;
            set => this.RaiseAndSetIfChanged(ref totalAmount, value);
        }

        /// <summary>
        /// Gets or sets list with items to buy.
        /// </summary>
        /// <date>26.05.2022.</date>
        public ObservableCollection<OperationItemModel> Order
        {
            get => order == null ? order = new ObservableCollection<OperationItemModel>() : order;
            set => this.RaiseAndSetIfChanged(ref order, value);
        }

        /// <summary>
        /// Gets or sets item that is selected by user.
        /// </summary>
        /// <date>27.05.2022.</date>
        public ObservableCollection<OperationItemModel> SelectedOrderRecord
        {
            get => selectedOrderRecord;
            set => this.RaiseAndSetIfChanged(ref selectedOrderRecord, value);
        }

        /// <summary>
        /// Gets or sets amount to payment.
        /// </summary>
        /// <date>30.05.2022.</date>
        public double AmountToPay
        {
            get => amountToPay;
            set => this.RaiseAndSetIfChanged(ref amountToPay, value);
        }

        /// <summary>
        /// Gets or sets amount to payment.
        /// </summary>
        /// <date>30.05.2022.</date>
        public double Change
        {
            get => change;
            set => this.RaiseAndSetIfChanged(ref change, value);
        }

        /// <summary>
        /// Gets or sets value indicating whether panel to choose nomenclature is visible.
        /// </summary>
        /// <date>03.06.2022.</date>
        public bool IsNomenclaturePanelVisible
        {
            get => isNomenclaturePanelVisible;
            set => this.RaiseAndSetIfChanged(ref isNomenclaturePanelVisible, value);
        }

        /// <summary>
        /// Gets or sets list with groups of items.
        /// </summary>
        /// <date>31.05.2022.</date>
        public ObservableCollection<GroupModel> ItemsGroups
        {
            get => itemsGroups == null ? itemsGroups = new ObservableCollection<GroupModel>() : itemsGroups;
            set => this.RaiseAndSetIfChanged(ref itemsGroups, value);
        }

        /// <summary>
        /// Gets or sets group of items that is selected by user.
        /// </summary>
        /// <date>31.05.2022.</date>
        public GroupModel SelectedItemsGroup
        {
            get => selectedItemsGroup;
            set => this.RaiseAndSetIfChanged(ref selectedItemsGroup, value);
        }
        

        /// <summary>
        /// Gets or sets list with items.
        /// </summary>
        /// <date>01.07.2022.</date>
        public ObservableCollection<ItemModel> Items
        {
            get => items == null ? items = new ObservableCollection<ItemModel>() : items;
            set => this.RaiseAndSetIfChanged(ref items, value);
        }

        /// <summary>
        /// Gets or sets item that is selected by user.
        /// </summary>
        /// <date>01.07.2022.</date>
        public ItemModel SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        /// <summary>
        /// Gets or sets list with VAT groups.
        /// </summary>
        /// <date>02.07.2022.</date>
        public ObservableCollection<VATGroupModel> VATGroups
        {
            get => vATGroups == null ? vATGroups = new ObservableCollection<VATGroupModel>() : vATGroups;
            set => this.RaiseAndSetIfChanged(ref vATGroups, value);
        }

        /// <summary>
        /// Gets or sets list with types of items.
        /// </summary>
        /// <date>02.07.2022.</date>
        public ObservableCollection<ComboBoxItemModel> ItemsTypes
        {
            get
            {
                if (itemsTypes == null)
                {
                    itemsTypes = new ObservableCollection<ComboBoxItemModel>()
                    {
                        new ComboBoxItemModel()
                        {
                            Key = "strItemType_Standard",
                            Value = EItemTypes.Standard,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strItemType_Excise",
                            Value = EItemTypes.Excise,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strItemType_Work",
                            Value = EItemTypes.Work,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strItemType_Service",
                            Value = EItemTypes.Service,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strItemType_LottaryTicket",
                            Value = EItemTypes.LottaryTicket,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strItemType_Payment",
                            Value = EItemTypes.Payment,
                        },
                        new ComboBoxItemModel()
                        {
                            Key = "strItemType_Other",
                            Value = EItemTypes.Other,
                        },
                    };
                }

                return itemsTypes;
            }
        }

        /// <summary>
        /// Gets or sets list with measures.
        /// </summary>
        /// <date>03.07.2022.</date>
        public ObservableCollection<string> Measures
        {
            get
            {
                if (measures == null)
                {
                    measures = new ObservableCollection<string>()
                    {
                        translationService.Localize("strMeasureItem"),
                        translationService.Localize("strMeasureKilo"),
                        translationService.Localize("strMeasureLiter"),
                        translationService.Localize("strMeasureBox"),
                    };
                }
                
                return measures;
            }
            set => this.RaiseAndSetIfChanged(ref measures, value);
        }

        /// <summary>
        /// Gets or sets list with groups of partners.
        /// </summary>
        /// <date>31.05.2022.</date>
        public ObservableCollection<GroupModel> PartnersGroups
        {
            get => partnersGroups == null ? partnersGroups = new ObservableCollection<GroupModel>() : partnersGroups;
            set => this.RaiseAndSetIfChanged(ref partnersGroups, value);
        }

        /// <summary>
        /// Gets or sets group of partners that is selected by user.
        /// </summary>
        /// <date>31.05.2022.</date>
        public GroupModel SelectedPartnersGroup
        {
            get => selectedPartnersGroup;
            set => this.RaiseAndSetIfChanged(ref selectedPartnersGroup, value);
        }

        /// <summary>
        /// Gets or sets list with partners.
        /// </summary>
        /// <date>01.07.2022.</date>
        public ObservableCollection<PartnerModel> Partners
        {
            get => partners == null ? partners = new ObservableCollection<PartnerModel>() : partners;
            set => this.RaiseAndSetIfChanged(ref partners, value);
        }

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
        /// <param name="items">List of items.</param>
        /// <date>03.06.2022.</date>
        public void DeleteItems(System.Collections.IList items)
        {
            Task.Run(() =>
            {
                Items.Clear();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items">List of items.</param>
        /// <date>03.06.2022.</date>
        public void AddItems(System.Collections.IList items)
        {
            // TODO: initialize method
        }

        /// <summary>
        /// Sets partner of the operation when partner is selected by user.
        /// </summary>
        /// <param name="partner"></param>
        /// <date>03.06.2022.</date>
        public void AddPartner(PartnerModel partner)
        {
            OperationPartner = partner;
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
