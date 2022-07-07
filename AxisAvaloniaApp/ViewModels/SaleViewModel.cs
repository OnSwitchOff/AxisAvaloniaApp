using AxisAvaloniaApp.Actions.Item;
using AxisAvaloniaApp.Actions.ItemsGroup;
using AxisAvaloniaApp.Actions.Partner;
using AxisAvaloniaApp.Actions.PartnersGroup;
using AxisAvaloniaApp.Actions.Sale;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Rules.Item;
using AxisAvaloniaApp.Rules.ItemsGroup;
using AxisAvaloniaApp.Rules.Partner;
using AxisAvaloniaApp.Rules.PartnersGroup;
using AxisAvaloniaApp.Rules.Sale;
using AxisAvaloniaApp.Services.Document;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.SearchNomenclatureData;
using AxisAvaloniaApp.Services.Serialization;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.Services.Validation;
using AxisAvaloniaApp.UserControls.Models;
using DataBase.Entities.Interfaces;
using DataBase.Entities.PartnersGroups;
using DataBase.Repositories.Items;
using DataBase.Repositories.ItemsGroups;
using DataBase.Repositories.OperationDetails;
using DataBase.Repositories.OperationHeader;
using DataBase.Repositories.Partners;
using DataBase.Repositories.PartnersGroups;
using DataBase.Repositories.VATGroups;
using Microinvest.CommonLibrary.Enums;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    /// <summary>
    /// Describes structure of InvalidOrderRecord event.
    /// </summary>
    /// <param name="invalidColumn">Index of invalid column.</param>
    /// <param name="invalidRow">Index of invalid row.</param>
    /// <date>23.06.2022.</date>
    public delegate void InvalidOrderRecordDelegate(int invalidColumn, int invalidRow);

    public class SaleViewModel : OperationViewModelBase
    {
        private readonly ISettingsService settingsService;
        private readonly ISerializationService serializationService;
        private readonly ISerializationService serializationItems;
        private readonly ISerializationService serializationPartners;
        private readonly IPaymentService paymentService;
        private readonly ITranslationService translationService;
        private readonly ILoggerService loggerService;
        private readonly IValidationService validationService;
        private readonly ISearchData searchService;

        private readonly IPartnerRepository partnerRepository;
        private readonly IItemsGroupsRepository itemsGroupsRepository;
        private readonly IItemRepository itemRepository;
        private readonly IPartnersGroupsRepository partnersGroupsRepository;
        private readonly IVATsRepository vATsRepository;
        private readonly IOperationHeaderRepository headerRepository;
        private readonly IOperationDetailsRepository detailsRepository;

        private bool isMainContentVisible;
        private bool isSaleTitleReadOnly;
        private bool isChoiceOfPartnerEnabled;
        private bool isPaymentPanelVisible;
        private string operationPartnerString;
        private PartnerModel operationPartner;
        private string uSN;
        private string totalAmount;
        private ObservableCollection<OperationItemModel> order;
        private OperationItemModel selectedOrderRecord;
        private double amountToPay;
        private double change;

        private bool isNomenclaturePanelVisible;
        private ObservableCollection<GroupModel> itemsGroups;
        private GroupModel selectedItemsGroup;
        private GroupModel? editableItemsGroup;
        private ObservableCollection<ItemModel> items;
        private ItemModel selectedItem;
        private ItemModel? editableItem;
        private string itemString;
        private ObservableCollection<VATGroupModel> vATGroups;
        private ObservableCollection<ComboBoxItemModel> itemsTypes;
        private ObservableCollection<string> measures;
        private ObservableCollection<PartnerModel> partners;
        private PartnerModel selectedPartner;
        private PartnerModel? editablePartner;
        private ObservableCollection<GroupModel> partnersGroups;
        private GroupModel selectedPartnersGroup;
        private GroupModel? editablePartnersGroup;

        private ObservableCollection<System.Drawing.Image> pages;

        private Avalonia.Threading.DispatcherTimer searchPartnerTimer;
        private Avalonia.Threading.DispatcherTimer searchItemsTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleViewModel"/> class.
        /// </summary>
        public SaleViewModel()
        {
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();

            serializationService = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            serializationService.InitSerializationData(ESerializationGroups.Sale);
            serializationItems = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            serializationItems.InitSerializationData(ESerializationGroups.ItemsNomenclature);
            serializationPartners = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            serializationPartners.InitSerializationData(ESerializationGroups.PartnersNomenclature);

            paymentService = Splat.Locator.Current.GetRequiredService<IPaymentService>();
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            translationService.LanguageChanged += Application_PropertyChanged;
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();
            validationService = Splat.Locator.Current.GetRequiredService<IValidationService>();
            searchService = Splat.Locator.Current.GetRequiredService<ISearchData>();
            DocumentService = Splat.Locator.Current.GetRequiredService<IDocumentService>();

            itemsGroupsRepository = Splat.Locator.Current.GetRequiredService<IItemsGroupsRepository>();
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
            partnersGroupsRepository = Splat.Locator.Current.GetRequiredService<IPartnersGroupsRepository>();
            partnerRepository = Splat.Locator.Current.GetRequiredService<IPartnerRepository>();
            vATsRepository = Splat.Locator.Current.GetRequiredService<IVATsRepository>();
            headerRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
            detailsRepository = Splat.Locator.Current.GetRequiredService<IOperationDetailsRepository>();

            IsCached = false;

            IsMainContentVisible = true;
            IsSaleTitleReadOnly = true;
            IsNomenclaturePanelVisible = true;
            IsPaymentPanelVisible = false;

            DeserializeData();

            searchPartnerTimer = new Avalonia.Threading.DispatcherTimer();
            searchPartnerTimer.Interval = TimeSpan.FromSeconds(1);
            searchPartnerTimer.Tick += FilteredPartners;

            searchItemsTimer = new Avalonia.Threading.DispatcherTimer();
            searchItemsTimer.Interval = TimeSpan.FromSeconds(1);
            searchItemsTimer.Tick += FilteredItems;

            RegisterValidationData<SaleViewModel, double>(
                this,
                nameof(AmountToPay),
                () =>
                {
                    return Math.Round(double.Parse(TotalAmount), 3) > Math.Round(AmountToPay, 3);
                },
                "msgNotEnoughMoney");
        }
        
        /// <summary>
        /// Gets service to serialize/deserialize visual data of the main content (work area).
        /// </summary>
        /// <date>17.06.2022.</date>
        public ISerializationService SerializationService => serializationService;

        /// <summary>
        /// Gets service to serialize/deserialize visual data of the panel with items.
        /// </summary>
        /// <date>20.06.2022.</date>
        public ISerializationService SerializationItems => serializationItems;

        /// <summary>
        /// Gets service to serialize/deserialize visual data of the panel with partners.
        /// </summary>
        /// <date>20.06.2022.</date>
        public ISerializationService SerializationPartners => serializationPartners;

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
            set => this.RaiseAndSetIfChanged(ref isChoiceOfPartnerEnabled, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether panel to pay is visible.
        /// </summary>
        /// <date>30.05.2022.</date>
        public bool IsPaymentPanelVisible
        {
            get => isPaymentPanelVisible;
            set => this.RaiseAndSetIfChanged(ref isPaymentPanelVisible, value);
        }

        /// <summary>
        /// Occurs when button Payment is pressed and order list has invalid records. 
        /// </summary>
        /// <date>23.06.2022.</date>
        public event InvalidOrderRecordDelegate InvalidOrderRecord;

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
            set => this.RaiseAndSetIfChanged(ref operationPartner, value);
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
            get
            {
                if (order == null)
                {
                    order = new ObservableCollection<OperationItemModel>();
                    order.CollectionChanged += OrderList_CollectionChanged;
                }

                return order;
            }
            set => this.RaiseAndSetIfChanged(ref order, value);
        }

        /// <summary>
        /// Gets or sets item that is selected by user.
        /// </summary>
        /// <date>27.05.2022.</date>
        public OperationItemModel SelectedOrderRecord
        {
            get => selectedOrderRecord;
            set => this.RaiseAndSetIfChanged(ref selectedOrderRecord, value);
        }

        /// <summary>
        /// Occurs when property of item in order list was changed.
        /// </summary>
        /// <date>22.06.2022.</date>
        public event Action OrderChanged;

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
            get => itemsGroups == null ?
                itemsGroups = new ObservableCollection<GroupModel>()
                {
                    new GroupModel()
                    {
                        Name = translationService.Localize("strAll"),
                        Path = "-2",
                        IsExpanded = true,
                    },
                } :
                itemsGroups;
            set => this.RaiseAndSetIfChanged(ref itemsGroups, value);
        }

        /// <summary>
        /// Gets or sets group of items that is selected by user.
        /// </summary>
        /// <date>31.05.2022.</date>
        public GroupModel SelectedItemsGroup
        {
            get => selectedItemsGroup == null ? selectedItemsGroup = ItemsGroups[0] : selectedItemsGroup;
            set => this.RaiseAndSetIfChanged(ref selectedItemsGroup, value);
        }

        /// <summary>
        /// Gets or sets group of items to edit or create.
        /// </summary>
        /// <date>22.06.2022.</date>
        public GroupModel? EditableItemsGroup
        {
            get => editableItemsGroup == null ? editableItemsGroup = new GroupModel() : editableItemsGroup;
            set => this.RaiseAndSetIfChanged(ref editableItemsGroup, value);
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
        /// Gets or sets item to edit or create is selected by user.
        /// </summary>
        /// <date>22.06.2022.</date>
        public ItemModel? EditableItem
        {
            get => editableItem == null ? editableItem = new ItemModel() : editableItem;
            set => this.RaiseAndSetIfChanged(ref editableItem, value);
        }

        /// <summary>
        /// Gets or sets key to filter items.
        /// </summary>
        /// <date>21.06.2022.</date>
        public string ItemString
        {
            get => itemString;
            set => this.RaiseAndSetIfChanged(ref itemString, value);
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
            get => partnersGroups == null ?
                partnersGroups = new ObservableCollection<GroupModel>()
                {
                    new GroupModel()
                    {
                        Name = translationService.Localize("strAll"),
                        Path = "-2",
                        IsExpanded = true,
                    },
                } :
                partnersGroups;
            set => this.RaiseAndSetIfChanged(ref partnersGroups, value);
        }

        /// <summary>
        /// Gets or sets group of partners that is selected by user.
        /// </summary>
        /// <date>31.05.2022.</date>
        public GroupModel SelectedPartnersGroup
        {
            get => selectedPartnersGroup == null ? selectedPartnersGroup = PartnersGroups[0] : selectedPartnersGroup;
            set => this.RaiseAndSetIfChanged(ref selectedPartnersGroup, value);
        }

        /// <summary>
        /// Gets or sets group of partners to edit or create.
        /// </summary>
        /// <date>22.06.2022.</date>
        public GroupModel? EditablePartnersGroup
        {
            get => editablePartnersGroup == null ? editablePartnersGroup = new GroupModel() : editablePartnersGroup;
            set => this.RaiseAndSetIfChanged(ref editablePartnersGroup, value);
        }

        /// <summary>
        /// Gets or sets list with partners.
        /// </summary>
        /// <date>01.06.2022.</date>
        public ObservableCollection<PartnerModel> Partners
        {
            get => partners == null ? partners = new ObservableCollection<PartnerModel>() : partners;
            set => this.RaiseAndSetIfChanged(ref partners, value);
        }

        /// <summary>
        /// Gets or sets partner that is selected by user.
        /// </summary>
        /// <date>01.06.2022.</date>
        public PartnerModel SelectedPartner
        {
            get => selectedPartner;
            set => this.RaiseAndSetIfChanged(ref selectedPartner, value);
        }

        /// <summary>
        /// Gets or sets partner to edit or create is selected by user.
        /// </summary>
        /// <date>22.06.2022.</date>
        public PartnerModel? EditablePartner
        {
            get => editablePartner == null ? editablePartner = new PartnerModel() : editablePartner;
            set => this.RaiseAndSetIfChanged(ref editablePartner, value);
        }

        /// <summary>
        /// Gets service to generate receipt.
        /// </summary>
        /// <date>22.06.2022.</date>
        public IDocumentService DocumentService { get; private set; }

        /// <summary>
        /// Gets or sets list with images to show receipt.
        /// </summary>
        /// <date>22.06.2022.</date>
        public ObservableCollection<System.Drawing.Image> Pages
        {
            get => pages == null ? pages = new ObservableCollection<System.Drawing.Image>() : pages;
            set => this.RaiseAndSetIfChanged(ref pages, value);
        }

        /// <summary>
        /// Downloads data from the database.
        /// </summary>
        /// <date>20.06.2022.</date>
        private async void DeserializeData()
        {
            IsChoiceOfPartnerEnabled = (bool)serializationService[ESerializationKeys.TbPartnerEnabled];
            if ((int)serializationService[ESerializationKeys.TbPartnerID] > 0)
            {
                OperationPartner = partnerRepository.GetPartnerByIdAsync((int)serializationService[ESerializationKeys.TbPartnerID]).GetAwaiter().GetResult();
                OperationPartnerString = OperationPartner.Name;
            }

            USN = paymentService.FiscalDevice.ReceiptNumber;
            TotalAmount = 0.ToString(settingsService.PriceFormat);
            Order.Add(new OperationItemModel());

            SelectedItemsGroup = GetGroups(
                ItemsGroups[0].SubGroups,
                itemsGroupsRepository.GetItemsGroupsAsync().GetAwaiter().GetResult(),
                (GroupModel)itemsGroupsRepository.GetGroupByIdAsync((int)serializationItems[ESerializationKeys.SelectedGroupId]).GetAwaiter().GetResult(),
                ItemsGroups[0]);

            ItemModel itemModel;
            await foreach (var item in itemRepository.GetItemsAsync())
            {
                itemModel = (ItemModel)item;
                itemModel.Price = await headerRepository.GetItemPriceAsync(itemModel.Id);
                Items.Add(itemModel);
            }

            foreach (string measure in await itemRepository.GetMeasuresAsync())
            {
                if (!Measures.Contains(measure))
                {
                    Measures.Add(measure);
                }
            }

            SelectedPartnersGroup = GetGroups<PartnersGroup>(
                PartnersGroups[0].SubGroups,
                partnersGroupsRepository.GetPartnersGroupsAsync().GetAwaiter().GetResult(),
                (GroupModel)partnersGroupsRepository.GetGroupByIdAsync((int)serializationPartners[ESerializationKeys.SelectedGroupId]).GetAwaiter().GetResult(),
                PartnersGroups[0]);

            await foreach (var partner in partnerRepository.GetParnersAsync(SelectedPartnersGroup.Path, string.Empty))
            {
                Partners.Add(partner);
            }

            await foreach (var vAT in vATsRepository.GetVATGroupsAsync())
            {
                VATGroups.Add(vAT);
            }

            // !!! данная строка размещена здесь, чтобы уменьшить кол-во обращений к базе при изменении свойств модели 
            this.PropertyChanged += SaleViewModel_PropertyChanged;
        }

        /// <summary>
        /// Fills list with groups.
        /// </summary>
        /// <typeparam name="T">Type of collection.</typeparam>
        /// <param name="groupsList">List of groups to fill.</param>
        /// <param name="groups">List with groups from the database.</param>
        /// <param name="selectedGroup">Group selected by user last time.</param>
        /// <param name="parentGroup">Parent group.</param>
        /// <param name="pathLength">Lenght of inserted group.</param>
        /// <param name="startIndex">Index to start iterate of list with groups from the database.</param>
        /// <param name="parentGroupPath">Path of the parent group.</param>
        /// <returns>Returns selected group if it exists; otherwise returns null.</returns>
        /// <date>20.06.2022.</date>
        private GroupModel GetGroups<T>(ObservableCollection<GroupModel> groupsList, List<T> groups, GroupModel selectedGroup, GroupModel parentGroup = null, int pathLength = 3, int startIndex = 0, string parentGroupPath = "")
            where T : INomenclaturesGroup
        {
            GroupModel group = null;

            for (; startIndex < groups.Count; startIndex++)
            {
                if ((groups[startIndex].Path.Length == pathLength || groups[startIndex].Path.Equals("-1")) && (string.IsNullOrEmpty(parentGroupPath) || groups[startIndex].Path.StartsWith(parentGroupPath)))
                {
                    group = new GroupModel()
                    {
                        Id = groups[startIndex].Id,
                        Name = groups[startIndex].Name,
                        Path = groups[startIndex].Path,
                        ParentGroup = parentGroup,
                        IsExpanded = selectedGroup != null && selectedGroup.Path.StartsWith(groups[startIndex].Path),
                    };

                    groupsList.Add(group);

                    GroupModel tmpGroup = GetGroups(group.SubGroups, groups, selectedGroup, group, pathLength + 3, startIndex + 1, group.Path);
                    if (tmpGroup != null)
                    {
                        group = tmpGroup;
                    }
                }
            }

            return (group != null && selectedGroup != null && group.Id == selectedGroup.Id) ? group : null;
        }

        /// <summary>
        /// Localizes some fields when language of property is changed.
        /// </summary>
        /// <date>30.05.2022.</date>
        private void Application_PropertyChanged()
        {
            Measures[0] = translationService.Localize("strMeasureItem");
            Measures[1] = translationService.Localize("strMeasureKilo");
            Measures[2] = translationService.Localize("strMeasureLiter");
            Measures[3] = translationService.Localize("strMeasureBox");

            ItemsGroups[0].Name = translationService.Localize("strAll");
            PartnersGroups[0].Name = translationService.Localize("strAll");
        }

        /// <summary>
        /// Updates dependent property when main property was changed.
        /// </summary>
        /// <param name="sender">SaleViewModel</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        /// <date>21.06.2022.</date>
        private async void SaleViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsChoiceOfPartnerEnabled):
                    if (!IsChoiceOfPartnerEnabled && OperationPartner == null)
                    {
                        OperationPartner = partnerRepository.GetPartnerByIdAsync(1).GetAwaiter().GetResult();
                    }
                    break;
                case nameof(OperationPartner):
                    if (OperationPartner == null)
                    {
                        OperationPartnerString = string.Empty;
                    }
                    else
                    {
                        OperationPartnerString = operationPartner.Name;
                        OrderChanged?.Invoke();
                    }

                    if (searchPartnerTimer != null && searchPartnerTimer.IsEnabled)
                    {
                        searchPartnerTimer.Stop();
                    }
                    break;
                case nameof(OperationPartnerString):
                    if (searchPartnerTimer != null &&
                        !searchPartnerTimer.IsEnabled &&
                        (OperationPartner == null || !OperationPartner.Name.Equals(operationPartnerString)))
                    {
                        searchPartnerTimer.Start();
                    }
                    break;
                case nameof(ItemString):
                    if (searchItemsTimer != null &&
                        !searchItemsTimer.IsEnabled &&
                        !string.IsNullOrEmpty(ItemString) &&
                        (SelectedOrderRecord == null || !SelectedOrderRecord.Name.Equals(ItemString)))
                    {
                        searchItemsTimer.Start();
                    }
                    break;
                case nameof(SelectedItemsGroup):
                    if (SelectedItemsGroup != null)
                    {
                        Items.Clear();

                        await foreach (ItemModel item in itemRepository.GetItemsAsync(SelectedItemsGroup.Path, string.Empty))
                        {
                            Items.Add(item);
                        }
                    }
                    break;
                case nameof(SelectedPartnersGroup):
                    if (SelectedPartnersGroup != null)
                    {
                        Partners.Clear();

                        await foreach (PartnerModel partner in partnerRepository.GetParnersAsync(SelectedPartnersGroup.Path, string.Empty))
                        {
                            Partners.Add(partner);
                        }
                    }
                    break;
                case nameof(TotalAmount):
                case nameof(AmountToPay):
                    Change = AmountToPay - double.Parse(TotalAmount);
                    break;
            }
        }

        /// <summary>
        /// Sets number of row when order list is changing.
        /// </summary>
        /// <param name="sender">ObservableCollection<OperationItemModel></param>
        /// <param name="e">NotifyCollectionChangedEventArgs.</param>
        /// <date>21.06.2022.</date>
        private void OrderList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (OperationItemModel itemModel in e.NewItems)
                    {
                        itemModel.RecordId = Order.Count;

                        itemModel.PropertyChanged -= OrderItem_PropertyChanged;
                        itemModel.PropertyChanged += OrderItem_PropertyChanged;
                    }

                    TotalAmount = Order.Sum(i => i.Amount).ToString(settingsService.PriceFormat);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (OperationItemModel itemModel in e.OldItems)
                    {
                        itemModel.PropertyChanged -= OrderItem_PropertyChanged;
                    }


                    for (int i = e.OldStartingIndex; i < Order.Count; i++)
                    {
                        Order[i].RecordId = i + 1;
                    }

                    TotalAmount = Order.Sum(i => i.Amount).ToString(settingsService.PriceFormat);
                    break;
            }
        }

        /// <summary>
        /// Sets total amount when amount by record is changed.
        /// </summary>
        /// <param name="sender">OperationItemModel.</param>
        /// <param name="e">PropertyChangedEventArgs</param>
        /// <date>22.06.2022.</date>
        private void OrderItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OperationItemModel.Amount):
                    TotalAmount = Order.Sum(i => i.Amount).ToString(settingsService.PriceFormat);
                    break;
            }
        }

        /// <summary>
        /// Filters list with partners in according to key.
        /// </summary>
        /// <param name="sender">DispatcherTimer.</param>
        /// <param name="e">EventArgs</param>
        /// <date>21.06.2022.</date>
        private async void FilteredPartners(object? sender, EventArgs e)
        {
            Partners.Clear();

            await foreach (var partner in partnerRepository.GetParnersAsync(OperationPartnerString))
            {
                Partners.Add(partner);
            }
        }

        /// <summary>
        /// Filters list with items in according to key.
        /// </summary>
        /// <param name="sender">DispatcherTimer.</param>
        /// <param name="e">EventArgs</param>
        /// <date>21.06.2022.</date>
        private async void FilteredItems(object? sender, EventArgs e)
        {
            Items.Clear();

            await foreach (var item in itemRepository.GetItemsAsync(ItemString))
            {
                Items.Add((ItemModel)item);
            }
        }

        /// <summary>
        /// Serializes visual data.
        /// </summary>
        /// <date>30.05.2022.</date>
        protected override void CloseView()
        {
            translationService.LanguageChanged -= Application_PropertyChanged;
            this.PropertyChanged -= SaleViewModel_PropertyChanged;

            serializationService[ESerializationKeys.TbPartnerEnabled].Value = IsChoiceOfPartnerEnabled.ToString();
            if (OperationPartner != null)
            {
                serializationService[ESerializationKeys.TbPartnerID].Value = OperationPartner.Id.ToString();
            }
            serializationService.Update();

            serializationItems[ESerializationKeys.SelectedGroupId].Value = SelectedItemsGroup.Id.ToString();
            serializationItems.Update();

            serializationPartners[ESerializationKeys.SelectedGroupId].Value = SelectedPartnersGroup.Id.ToString();
            serializationPartners.Update();

            DocumentService.Dispose();

            base.CloseView();
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
        /// Deletes selected items from order list.
        /// </summary>
        /// <param name="items">List of items.</param>
        /// <date>03.06.2022.</date>
        public void DeleteItemsFromOrder(System.Collections.IList items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i] is OperationItemModel operationItem && operationItem.Item.Id > 0)
                {
                    Order.Remove(operationItem);
                }
            }
        }

        /// <summary>
        /// Adds items to order list.
        /// </summary>
        /// <param name="items">List of items.</param>
        /// <date>03.06.2022.</date>
        public void AddItems(System.Collections.IList items)
        {
            foreach(ItemModel item in items)
            {
                OperationItemModel? tmpItem = Order.Contains(item);
                if (tmpItem != null)
                {
                    tmpItem.Qty++;
                    OrderChanged?.Invoke();
                }
                else
                {
                    if (SelectedOrderRecord != null)
                    {
                        SelectedOrderRecord.Item = item;
                    }
                    else
                    {
                        Order[Order.Count - 1].Item = item;                        
                    }

                    if (Order[Order.Count - 1].Item.Id != 0)
                    {
                        Order.Add(new OperationItemModel());
                    }
                }
            }

            if (searchItemsTimer != null && searchItemsTimer.IsEnabled)
            {
                searchItemsTimer.Stop();
            }
        }

        /// <summary>
        /// Sets partner of the operation when partner is selected by user.
        /// </summary>
        /// <param name="partner"></param>
        /// <date>03.06.2022.</date>
        public void AddPartner(PartnerModel? partner)
        {
            if (partner == null || partner.Id == 0)
            {
                InitPartnerCreation(partner);
            }
            else
            {
                OperationPartner = partner;
            }
        }

        /// <summary>
        /// Finds partner data on the Microinvest club by tax or VAT number.
        /// </summary>
        /// <param name="key">Key to search partner.</param>
        /// <date>03.06.2022.</date>
        public async void FindPartner(string key)
        {
            if (!string.IsNullOrEmpty(key) && (SelectedPartner == null || SelectedPartner.Id == 0))
            {
                EditablePartner = await searchService.GetPartnerData(key);
            }
        }

        /// <summary>
        /// Finds partner by number of card.
        /// </summary>
        /// <param name="cardNumber">Number of a card of partner.</param>
        /// <date>21.06.2022.</date>
        public async void FindPartnerByCardNumber(string cardNumber)
        {
            PartnerModel partner = await partnerRepository.GetPartnerByDiscountCardAsync(cardNumber);
            if (partner == null)
            {
                partner = new PartnerModel();
            }

            if (partner.Id == 0)
            {
                partner.DiscountCardNumber = cardNumber;
            }
            
            AddPartner(partner);
        }

        /// <summary>
        /// Creates a new partner if the user agrees. 
        /// </summary>
        /// <param name="newPartner">Data of new partner.</param>
        /// <date>21.06.2022.</date>
        private async void InitPartnerCreation(PartnerModel newPartner)
        {
            if (await loggerService.ShowDialog("msgPartnerNotFound", "strAttention", UserControls.MessageBox.EButtonIcons.Info, UserControls.MessageBox.EButtons.YesNo) == UserControls.MessageBox.EButtonResults.Yes)
            {
                if (newPartner == null)
                {
                    newPartner = new PartnerModel();
                }
                
                EditablePartner = newPartner;
                IsNomenclaturePanelVisible = false;
            }
        }

        /// <summary>
        /// Finds item data on the Microinvest club by barcode.
        /// </summary>
        /// <param name="barcode">Key to search item.</param>
        /// <date>04.07.2022.</date>
        public async void FindItem(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode) && (SelectedItem == null || SelectedItem.Id == 0))
            {
                ItemModel result = await searchService.GetItemData(barcode);
                if (result != null)
                {
                    EditableItem = result;
                }
            }
        }

        /// <summary>
        /// Deletes the nomenclature if all conditions are met.
        /// </summary>
        /// <param name="nomenclature">Nomenclature to delete.</param>
        /// <param name="agreeMessage">Key for localize message to confirm whether user agrees to delete nomenclature</param>
        /// <param name="warningCondition">Condition if nomenclature can't be deleted</param>
        /// <param name="warningMessage">Key for localize message</param>
        /// <param name="databaseAction">
        /// Method to delete nomenclature from the database. 
        /// The method returns true if nomenclature was deleted; otherwise returns false. 
        /// </param>
        /// <param name="viewModelAction">Method to update ViewModel data.</param>
        /// <param name="logEvents">Error sign when deleting an item.</param>
        /// <param name="deleteErrorMessage">Key for localized message if deleting an item is unsuccess.</param>
        /// <date>22.06.2022.</date>
        private async void DeleteNomenclatureAsync(
            object nomenclature,
            string agreeMessage,
            Func<bool> warningCondition,
            string warningMessage,
            Func<Task<bool>> databaseAction,
            Action viewModelAction,
            DataBase.Enums.EApplicationLogEvents logEvents,
            string deleteErrorMessage)
        {
            if (await loggerService.ShowDialog(agreeMessage, "strAttention", UserControls.MessageBox.EButtonIcons.Info, UserControls.MessageBox.EButtons.YesNo) == UserControls.MessageBox.EButtonResults.Yes)
            {
                if (nomenclature != null)
                {
                    if (warningCondition.Invoke())
                    {
                        await loggerService.ShowDialog(warningMessage, "strWarning", UserControls.MessageBox.EButtonIcons.Warning);
                    }
                    else
                    {
                        if (await databaseAction.Invoke())
                        {
                            viewModelAction.Invoke();
                        }
                        else
                        {
                            loggerService.RegisterError(logEvents, translationService.Localize(deleteErrorMessage));
                            await loggerService.ShowDialog(deleteErrorMessage, "strWarning", UserControls.MessageBox.EButtonIcons.Warning);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add new nomenclature to database.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>31.05.2022.</date>
        public void AddNewNomenclature(ENomenclatures nomenclature)
        {
            switch (nomenclature)
            {
                case ENomenclatures.Items:
                    EditableItem = new ItemModel();
                    if (SelectedItemsGroup.Path.Equals("-2"))
                    {
                        EditableItem.Group.Clone(SelectedItemsGroup.SubGroups[0]);
                    }
                    else
                    {
                        EditableItem.Group.Clone(SelectedItemsGroup);
                    }                    
                    EditableItem.VATGroup = VATGroups[0];

                    EditableItem.RegisterValidationData<ItemModel, string>(
                        nameof(ItemModel.Barcode),
                        () =>
                        {
                            return !string.IsNullOrEmpty(EditableItem.Barcode) && !validationService.IsBarcode(EditableItem.Barcode);
                        },
                        "msgInvalidBarcode");
                    break;
                case ENomenclatures.Partners:
                    EditablePartner = new PartnerModel();
                    if (SelectedPartnersGroup.Path.Equals("-2"))
                    {
                        EditablePartner.Group.Clone(SelectedPartnersGroup.SubGroups[0]);
                    }
                    else
                    {
                        EditablePartner.Group.Clone(SelectedPartnersGroup);
                    }

                    EditablePartner.RegisterValidationData<PartnerModel, string>(
                        nameof(PartnerModel.TaxNumber), 
                        () => 
                        {
                            return !string.IsNullOrEmpty(EditablePartner.TaxNumber) && !validationService.IsTaxNumber(EditablePartner.TaxNumber);
                        }, 
                        "msgInvalidTaxNumber");
                    EditablePartner.RegisterValidationData<PartnerModel, string>(
                        nameof(PartnerModel.VATNumber),
                        () =>
                        {
                            return !string.IsNullOrEmpty(EditablePartner.VATNumber) && !validationService.IsVATNumber(EditablePartner.VATNumber);
                        },
                        "msgInvalidVATNumber");
                    break;
                case ENomenclatures.ItemsGroups:
                    EditableItemsGroup = new GroupModel();
                    if (SelectedItemsGroup.Path.Equals("-1"))
                    {
                        EditableItemsGroup.ParentGroup = ItemsGroups[0];
                    }
                    else
                    {
                        EditableItemsGroup.ParentGroup = SelectedItemsGroup;
                    }
                    break;
                case ENomenclatures.PartnersGroups:
                    EditablePartnersGroup = new GroupModel();
                    if (SelectedPartnersGroup.Path.Equals("-1"))
                    {
                        EditablePartnersGroup.ParentGroup = PartnersGroups[0];
                    }
                    else
                    {
                        EditablePartnersGroup.ParentGroup = SelectedPartnersGroup;
                    }                    
                    break;
            }

            IsNomenclaturePanelVisible = false;
        }

        /// <summary>
        /// Update nomenclature in the database.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>31.05.2022.</date>
        public void UpdateNomenclature(ENomenclatures nomenclature)
        {
            switch (nomenclature)
            {
                case ENomenclatures.Items:
                    if (SelectedItem == null)
                    {
                        loggerService.ShowDialog("msgItemToEditNotChoosen", "strAttention", UserControls.MessageBox.EButtonIcons.Info);
                    }
                    else
                    {
                        EditableItem?.Clone(SelectedItem);
                        EditableItem?.RegisterValidationData<ItemModel, string>(
                            nameof(ItemModel.Barcode),
                            () =>
                            {
                                return !string.IsNullOrEmpty(EditableItem.Barcode) && !validationService.IsBarcode(EditableItem.Barcode);
                            },
                            "msgInvalidBarcode");

                        IsNomenclaturePanelVisible = false;
                    }
                    break;
                case ENomenclatures.Partners:
                    if (SelectedPartner == null)
                    {
                        loggerService.ShowDialog("msgPartnerToEditNotChoosen", "strAttention", UserControls.MessageBox.EButtonIcons.Info);
                    }
                    else
                    {
                        EditablePartner?.Clone(SelectedPartner);
                        EditablePartner?.RegisterValidationData<PartnerModel, string>(
                        nameof(PartnerModel.TaxNumber),
                        () =>
                        {
                            return !string.IsNullOrEmpty(EditablePartner.TaxNumber) && !validationService.IsTaxNumber(EditablePartner.TaxNumber);
                        },
                        "msgInvalidTaxNumber");
                        EditablePartner?.RegisterValidationData<PartnerModel, string>(
                            nameof(PartnerModel.VATNumber),
                            () =>
                            {
                                return !string.IsNullOrEmpty(EditablePartner.VATNumber) && !validationService.IsVATNumber(EditablePartner.VATNumber);
                            },
                            "msgInvalidVATNumber");

                        IsNomenclaturePanelVisible = false;
                    }
                    break;
                case ENomenclatures.ItemsGroups:
                    if (SelectedItemsGroup == null)
                    {
                        loggerService.ShowDialog("msgGroupToEditNotChoosen", "strAttention", UserControls.MessageBox.EButtonIcons.Info);
                    }
                    else
                    {
                        if (!SelectedItemsGroup.Path.Equals("-2"))
                        {
                            EditableItemsGroup?.Clone(SelectedItemsGroup);
                            IsNomenclaturePanelVisible = false;
                        }
                    }
                    break;
                case ENomenclatures.PartnersGroups:
                    if (SelectedPartnersGroup == null)
                    {
                        loggerService.ShowDialog("msgGroupToEditNotChoosen", "strAttention", UserControls.MessageBox.EButtonIcons.Info);
                    }
                    else
                    {
                        if (!SelectedPartnersGroup.Path.Equals("-2"))
                        {
                            EditablePartnersGroup?.Clone(SelectedPartnersGroup);
                            IsNomenclaturePanelVisible = false;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Delete nomenclature from the database.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>31.05.2022.</date>
        public void DeleteNomenclature(ENomenclatures nomenclature)
        {
            switch (nomenclature)
            {
                case ENomenclatures.Items:
                    DeleteNomenclatureAsync(
                        SelectedItem,
                        "msgDoYouWantDeleteItem",
                        () =>
                        {
                            return SelectedItem.Id == 1;
                        },
                        "msgBaseItemCanNotBeDeleted",
                        async () =>
                        {
                            return await itemRepository.DeleteItemAsync(SelectedItem.Id);
                        },
                        () =>
                        {
                            Items.Remove(SelectedItem);
                            SelectedItem = null;
                        },
                        DataBase.Enums.EApplicationLogEvents.DeleteItem,
                        "msgErrorDuringDeletingItem");
                    break;
                case ENomenclatures.Partners:
                    DeleteNomenclatureAsync(
                        SelectedPartner,
                        "msgDoYouWantDeletePartner",
                        () =>
                        {
                            return SelectedPartner.Id == 1;
                        },
                        "msgBasePartnerCanNotBeDeleted",
                        async () =>
                        {
                            return await partnerRepository.DeletePartnerAsync(SelectedPartner.Id);
                        },
                        () =>
                        {
                            Partners.Remove(SelectedPartner);
                            SelectedPartner = null;
                        },
                        DataBase.Enums.EApplicationLogEvents.DeletePartner,
                        "msgErrorDuringDeletingPartner");
                    break;
                case ENomenclatures.ItemsGroups:
                    DeleteNomenclatureAsync(
                        SelectedItemsGroup,
                        "msgDoYouWantDeleteItemGroup",
                        () =>
                        {
                            return SelectedItemsGroup.Id == 1;
                        },
                        "msgBaseItemGroupCanNotBeDeleted",
                        async () =>
                        {
                            return await itemsGroupsRepository.DeleteGroupAsync(SelectedItemsGroup.Id);
                        },
                        () =>
                        {
                            ItemsGroups.Remove(SelectedItemsGroup);
                            SelectedItemsGroup = null;
                        },
                        DataBase.Enums.EApplicationLogEvents.DeleteItemGroup,
                        "msgErrorDuringDeletingGroup");
                    break;
                case ENomenclatures.PartnersGroups:
                    DeleteNomenclatureAsync(
                        SelectedPartnersGroup,
                        "msgDoYouWantDeletePartnerGroup",
                        () =>
                        {
                            return SelectedPartnersGroup.Id == 1;
                        },
                        "msgBasePartnerGroupCanNotBeDeleted",
                        async () =>
                        {
                            return await partnersGroupsRepository.DeleteGroupAsync(SelectedPartnersGroup.Id);
                        },
                        () =>
                        {
                            PartnersGroups.Remove(SelectedPartnersGroup);
                            SelectedPartnersGroup = null;
                        },
                        DataBase.Enums.EApplicationLogEvents.DeletePartnerGroup,
                        "msgErrorDuringDeletingGroup");
                    break;
            }
        }

        /// <summary>
        /// Add new code to list with additional codes of item.
        /// </summary>
        /// <date>31.05.2022.</date>
        public async void AddAdditionalItemCode()
        {
            int nextItemCode = await itemRepository.GetNextItemCodeAsync();

            if (EditableItem != null && EditableItem.Codes != null)
            {
                foreach (ItemCodeModel itemCode in EditableItem.Codes)
                {
                    if (int.TryParse(itemCode.Code, out int code) && nextItemCode <= code)
                    {
                        nextItemCode = code + 1;
                    }
                }
            }

            EditableItem?.Codes?.Add(
                new ItemCodeModel()
                { 
                    Code = nextItemCode.ToString(),
                });
        }

        /// <summary>
        /// Delete code from list with additional codes of item.
        /// <param name="itemCode">Code to delete.</param>
        /// </summary>
        /// <date>31.05.2022.</date>
        public void DeleteAdditionalItemCode(ItemCodeModel itemCode)
        {
            EditableItem?.Codes.Remove(itemCode);
        }

        /// <summary>
        /// Save editable nomenclature to database.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>03.06.2022.</date>
        public async void SaveNomenclature(ENomenclatures nomenclature)
        {
            switch (nomenclature)
            {
                case ENomenclatures.Items:
                    if (EditableItem != null)
                    {
                        IStage itemNameIsNotEmpty = new ItemNameIsNotEmpty(EditableItem.Name);
                        IStage itemNameIsNotDuplicate = new ItemNameIsNotDuplicated(EditableItem);
                        IStage itemBarcodeIsNotDuplicate = new ItemBarcodeIsNotDuplicated(EditableItem);
                        IStage itemCodesAreNotDuplicated = new ItemCodesAreNotDuplicated(EditableItem);
                        IStage itemMeasureIsNotEmpty = new ItemMeasureIsNotEmpty(EditableItem.Measure);
                        IStage itemMeasuresAreNotDuplicated = new ItemMeasuresAreNotDuplicated(EditableItem);
                        SaveItem saveItem = new SaveItem(EditableItem);
                        IStage addRevaluationData = new AddRevaluationData(EditableItem, EditableItem.Id == 0 ? 0 : SelectedItem.Price);

                        itemNameIsNotEmpty.
                            SetNext(itemNameIsNotDuplicate).
                            SetNext(itemBarcodeIsNotDuplicate).
                            SetNext(itemCodesAreNotDuplicated).
                            SetNext(itemMeasureIsNotEmpty).
                            SetNext(itemMeasuresAreNotDuplicated).
                            SetNext(saveItem).
                            SetNext(addRevaluationData);

                        if (await itemNameIsNotEmpty.Invoke(new object()) is object obj && 
                            obj != null && 
                            int.TryParse(obj.ToString(), out int result) && 
                            result == 1)
                        {
                            if (saveItem.IsNewItem)
                            {
                                if (SelectedItemsGroup != null &&
                                    (SelectedItemsGroup.Path.Equals("-2") ||
                                    EditableItem.Group.Path.StartsWith(SelectedItemsGroup.Path)))
                                {
                                    Items.Add(EditableItem);
                                    SelectedItem = Items[Items.Count - 1];
                                }
                            }
                            else
                            {
                                SelectedItem.Clone(EditableItem);
                            }
                            
                            IsNomenclaturePanelVisible = true;
                        }
                    }
                    break;
                case ENomenclatures.ItemsGroups:
                    if (EditableItemsGroup != null)
                    {
                        IStage itemsGroupNameIsNotEmpty = new ItemsGroupNameIsNotEmpty(EditableItemsGroup.Name);
                        IStage itemsGroupNameIsNotDuplicated = new ItemsGroupNameIsNotDuplicated(EditableItemsGroup);
                        SaveItemsGroup saveItemsGroup = new SaveItemsGroup(EditableItemsGroup);

                        itemsGroupNameIsNotEmpty.
                            SetNext(itemsGroupNameIsNotDuplicated).
                            SetNext(saveItemsGroup);

                        if (await itemsGroupNameIsNotEmpty.Invoke(new object()) is object obj &&
                            obj != null &&
                            int.TryParse(obj.ToString(), out int result) &&
                            result == 1)
                        {
                            if (saveItemsGroup.IsNewItemsGroup)
                            {
                                if (EditableItemsGroup.Path.Length == 3)
                                {
                                    ItemsGroups[0].SubGroups.Add(EditableItemsGroup);
                                }
                                else
                                {
                                    SelectedItemsGroup.SubGroups.Add(EditableItemsGroup);
                                    SelectedItemsGroup.IsExpanded = true;
                                    SelectedItemsGroup = EditableItemsGroup;
                                }
                            }
                            else
                            {
                                SelectedPartnersGroup.Clone(EditableItemsGroup);
                            }

                            IsNomenclaturePanelVisible = true;
                        }
                    }
                    break;
                case ENomenclatures.Partners:
                    if (EditablePartner != null)
                    {
                        IStage partnerNameIsNotEmpty = new PartnerNameIsNotEmpty(EditablePartner.Name);
                        IStage partnerNameIsNotDuplicated = new PartnerNameIsNotDuplicated(EditablePartner);
                        IStage partnerTaxNumberIsNotDuplicated = new PartnerTaxNumberIsNotDuplicated(EditablePartner);
                        IStage partnerVATNumberIsNotDuplicated = new PartnerVATNumberIsNotDuplicated(EditablePartner);
                        IStage partnerPhoneIsNotDuplicated = new PartnerPhoneIsNotDuplicated(EditablePartner);
                        IStage partnerEmailIsNotDuplicated = new PartnerEmailIsNotDuplicated(EditablePartner);
                        SavePartner savePartner = new SavePartner(EditablePartner);

                        partnerNameIsNotEmpty.
                            SetNext(partnerNameIsNotDuplicated).
                            SetNext(partnerTaxNumberIsNotDuplicated).
                            SetNext(partnerVATNumberIsNotDuplicated).
                            SetNext(partnerPhoneIsNotDuplicated).
                            SetNext(partnerEmailIsNotDuplicated).
                            SetNext(savePartner);

                        if (await partnerNameIsNotEmpty.Invoke(new object()) is object obj &&
                            obj != null &&
                            int.TryParse(obj.ToString(), out int result) &&
                            result == 1)
                        {
                            if (savePartner.IsNewPartner)
                            {
                                if (selectedPartnersGroup != null &&
                                    (selectedPartnersGroup.Path.Equals("-2") ||
                                    EditablePartner.Group.Path.StartsWith(selectedPartnersGroup.Path)))
                                {
                                    Partners.Add(EditablePartner);
                                    SelectedPartner = Partners[Partners.Count - 1];
                                }
                            }
                            else
                            {
                                SelectedPartner.Clone(EditablePartner);
                            }

                            IsNomenclaturePanelVisible = true;
                        }
                    }
                    break;
                case ENomenclatures.PartnersGroups:
                    if (EditablePartnersGroup != null)
                    {
                        IStage partnersGroupNameIsNotEmpty = new PartnersGroupNameIsNotEmpty(EditablePartnersGroup.Name);
                        IStage partnersGroupNameIsNotDuplicated = new PartnersGroupNameIsNotDuplicated(EditablePartnersGroup);
                        SavePartnersGroup savePartnersGroup = new SavePartnersGroup(EditablePartnersGroup);

                        partnersGroupNameIsNotEmpty.
                            SetNext(partnersGroupNameIsNotDuplicated).
                            SetNext(savePartnersGroup);

                        if (await partnersGroupNameIsNotEmpty.Invoke(new object()) is object obj &&
                            obj != null &&
                            int.TryParse(obj.ToString(), out int result) &&
                            result == 1)
                        {
                            if (savePartnersGroup.IsNewPartnersGroup)
                            {
                                if (EditablePartnersGroup.Path.Length == 3)
                                {
                                    PartnersGroups[0].SubGroups.Add(EditablePartnersGroup);
                                }
                                else
                                {
                                    SelectedPartnersGroup.SubGroups.Add(EditablePartnersGroup);
                                    SelectedPartnersGroup.IsExpanded = true;
                                    SelectedPartnersGroup = EditablePartnersGroup;
                                }
                            }
                            else
                            {
                                SelectedPartnersGroup.Clone(EditablePartnersGroup);
                            }

                            IsNomenclaturePanelVisible = true;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Cancel edit of nomenclature and restore base state.
        /// </summary>
        /// <param name="nomenclature">Type of nomenclature.</param>
        /// <date>03.06.2022.</date>
        public void CancelNomenclature(ENomenclatures nomenclature)
        {
            switch (nomenclature)
            {
                case ENomenclatures.Items:
                    EditableItem = null;
                    break;
                case ENomenclatures.ItemsGroups:
                    EditableItemsGroup = null;
                    break;
                case ENomenclatures.Partners:
                    EditablePartner = null;
                    break;
                case ENomenclatures.PartnersGroups:
                    EditablePartnersGroup = null;
                    break;
            }

            IsNomenclaturePanelVisible = true;
        }

        /// <summary>
        /// Changes the visibility property of the panel to pay.
        /// </summary>
        /// <date>27.05.2022.</date>
        public async void ChangeIsPaymentPanelVisible()
        {
            if (Order.Count == 1 && Order[0].Item.Id == 0)
            {
                await loggerService.ShowDialog("msgEmptyTable", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                InvalidOrderRecord?.Invoke(-1, -1);
                return;
            }

            for (int i = 0; i < Order.Count; i++)
            {
                if (Order[i].Item.Id != 0)
                {
                    if (Order[i].Price == 0)
                    {
                        await loggerService.ShowDialog("msgEmptyPrice", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                        InvalidOrderRecord?.Invoke(6, i);
                        return;
                    }

                    if (Order[i].Qty == 0)
                    {
                        await loggerService.ShowDialog("msgEmptyQtty", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                        InvalidOrderRecord?.Invoke(5, i);
                        return;
                    }
                }
            }

            IsPaymentPanelVisible = !IsPaymentPanelVisible;
        }

        /// <summary>
        /// Pay the order.
        /// </summary>
        /// <param name="paymentType">Type of payment.</param>
        /// <date>30.05.2022.</date>
        public async void PaymentSale(EPaymentTypes paymentType)
        {
            IStage sumToPayIsEnough = new SumToPayIsEnough(AmountToPay, paymentType);
            IStage paymentStage = new PaymentOrder(Order, paymentType);
            SaveOrder writeToDatabaseStage = new SaveOrder(paymentService.FiscalDevice.ReceiptNumber, Order, OperationPartner, paymentType);
            CreateReceipt createReceipt = new CreateReceipt(Order, DocumentService);
            IStage prepareViewStage = new PrepareView(() =>
            {
                Order.Clear();
                Order.Add(new OperationItemModel());

                USN = paymentService.FiscalDevice.ReceiptNumber;
                IsPaymentPanelVisible = false;

                Pages = createReceipt.Pages.Clone();
                IsMainContentVisible = Pages.Count == 0;
            });

            sumToPayIsEnough.
                SetNext(paymentStage).
                SetNext(writeToDatabaseStage).
                SetNext(createReceipt).
                SetNext(prepareViewStage);
            
            await sumToPayIsEnough.Invoke(TotalAmount);
        }
    }
}
