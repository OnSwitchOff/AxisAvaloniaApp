using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Services.Logger;
using AxisAvaloniaApp.Services.Payment;
using AxisAvaloniaApp.Services.Serialization;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.Services.Validation;
using AxisAvaloniaApp.UserControls.Models;
using DataBase.Entities.Interfaces;
using DataBase.Entities.PartnersGroups;
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace AxisAvaloniaApp.ViewModels
{
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
        private OperationItemModel selectedOrderRecord;
        private double amountToPay;
        private double change;

        private bool isNomenclaturePanelVisible;
        private ObservableCollection<GroupModel> itemsGroups;
        private GroupModel selectedItemsGroup;
        private ObservableCollection<ItemModel> items;
        private ItemModel selectedItem;
        private string itemString;
        private ObservableCollection<VATGroupModel> vATGroups;
        private ObservableCollection<ComboBoxItemModel> itemsTypes;
        private ObservableCollection<string> measures;
        private ObservableCollection<PartnerModel> partners;
        private PartnerModel selectedPartner;
        private ObservableCollection<GroupModel> partnersGroups;
        private GroupModel selectedPartnersGroup;

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

            itemsGroupsRepository = Splat.Locator.Current.GetRequiredService<IItemsGroupsRepository>();
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
            partnersGroupsRepository = Splat.Locator.Current.GetRequiredService<IPartnersGroupsRepository>();
            partnerRepository = Splat.Locator.Current.GetRequiredService<IPartnerRepository>();
            vATsRepository = Splat.Locator.Current.GetRequiredService<IVATsRepository>();

            IsCached = false;

            IsMainContentVisible = true;
            IsSaleTitleReadOnly = true;
            IsNomenclaturePanelVisible = true;

            DeserializeData();

            searchPartnerTimer = new Avalonia.Threading.DispatcherTimer();
            searchPartnerTimer.Interval = TimeSpan.FromSeconds(1.5);
            searchPartnerTimer.Tick += FilteredPartners;

            searchItemsTimer = new Avalonia.Threading.DispatcherTimer();
            searchItemsTimer.Interval = TimeSpan.FromSeconds(1.5);
            searchItemsTimer.Tick += FilteredItems;

            this.PropertyChanged += SaleViewModel_PropertyChanged;
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
        /// Downloads data from the database.
        /// </summary>
        /// <date>20.06.2022.</date>
        private async void DeserializeData()
        {
            IsChoiceOfPartnerEnabled = (bool)serializationService[ESerializationKeys.TbPartnerEnabled];
            if ((int)serializationService[ESerializationKeys.TbPartnerID] > 0)
            {
                //OperationPartner = partnerRepository.GetPartnerByIdAsync((int)serializationService[ESerializationKeys.TbPartnerID]).GetAwaiter().GetResult();
                //OperationPartnerString = OperationPartner.Name;
            }

            USN = paymentService.FiscalDevice.ReceiptNumber;
            TotalAmount = 0.ToString(settingsService.PriceFormat);
            Order.Add(new OperationItemModel());

            SelectedItemsGroup = GetGroups(
                ItemsGroups[0].SubGroups,
                itemsGroupsRepository.GetItemsGroupsAsync().GetAwaiter().GetResult(),
                (GroupModel)itemsGroupsRepository.GetGroupByIdAsync((int)serializationItems[ESerializationKeys.SelectedGroupId]).GetAwaiter().GetResult(),
                ItemsGroups[0]);

            await foreach (var item in itemRepository.GetItemsAsync())
            {
                Items.Add((ItemModel)item);
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

            await foreach (var partner in partnerRepository.GetParnersAsync())
            {
                Partners.Add(partner);
            }

            await foreach (var vAT in vATsRepository.GetVATGroupsAsync())
            {
                VATGroups.Add(vAT);
            }
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
        private void SaleViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
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
                    // TODO
                    break;
                case nameof(SelectedPartnersGroup):
                    // TODO
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
        private Avalonia.Threading.DispatcherTimer partnerTimer = new Avalonia.Threading.DispatcherTimer();

        /// <summary>
        /// Cancel edit of nomenclature and restore base state.
        /// </summary>
        /// <param name="key">Key to search partner.</param>
        /// <date>03.06.2022.</date>
        public void FindPartner(string key)
        {
            // TODO: initialize method

            System.Diagnostics.Debug.WriteLine("FindPartner");
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
        /// 
        /// </summary>
        /// <param name="newPartner"></param>
        /// <date>21.06.2022.</date>
        private void InitPartnerCreation(PartnerModel newPartner)
        {
            
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
        /// Pay the order.
        /// </summary>
        /// <param name="paymentType">Type of payment.</param>
        /// <date>30.05.2022.</date>
        public void PaymentSale(EPaymentTypes paymentType)
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
