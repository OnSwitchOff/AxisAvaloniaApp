using Avalonia.Controls;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.UserControls.Models;
using AxisAvaloniaApp.UserControls.NavigationView;
using DataBase.Repositories.ApplicationLog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;

namespace AxisAvaloniaApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private bool mainMenuExpanded = false;
        private double mainMenuWidth = 200;
        public bool MainMenuExpanded
        {
            get => mainMenuExpanded;
            set
            {
                mainMenuExpanded = value;

                MainMenuWidth = mainMenuExpanded ? 40 : 200;
            }
        }

        public IReactiveCommand Command { get; }

        public double MainMenuWidth
        {
            get => mainMenuWidth;
            set => this.RaiseAndSetIfChanged(ref mainMenuWidth, value);
        }

        private ObservableCollection<INavigationViewItem> mainMenuItems;

        public ObservableCollection<INavigationViewItem> MainMenuItems
        {
            get => mainMenuItems;
            set => this.RaiseAndSetIfChanged(ref mainMenuItems, value);
        }

        private INavigationViewItem selectedItem;
        public INavigationViewItem SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        public string Greeting => "Welcome to Avalonia!";

        public List<string> Source => new List<string>() { "item 1", "item 2", "item 3" };

        public TreeViewModel SelectedNode { get; set; }

        public List<TreeViewModel> TreeViewNodes { get; set; } 
        private string _status;
        private string _testStr = "Default value";

        public string Status
        {
            get => _status;
            set => this.RaiseAndSetIfChanged(ref _status, value);
        }

        public string TestStr
        {
            get => _testStr;
            set => this.RaiseAndSetIfChanged(ref _testStr, value);
        }

        public void OnOpenClicked()
        {
            Status = $"Open clicked at {DateTime.Now}";

            TestStr = "Default value";
        }

        public MainWindowViewModel()
        {
            //Services.Scanning.IScanningData scanningData = Splat.Locator.Current.GetRequiredService<Services.Scanning.IScanningData>();
            //scanningData.StartCOMScanner("COM4");
            //scanningData.SendScannedDataEvent += ScanData;

            bool ex = false;
            Command = ReactiveCommand.Create(RunTheThing);
            Status = $"Application started at {DateTime.Now}";

            TreeViewNodes = new List<TreeViewModel>();
            TreeViewModel node = new TreeViewModel()
            {
                Name = "Item 1",
                Nodes = new List<TreeViewModel>() { new TreeViewModel() { Name = "Sub 1" }, new TreeViewModel() { Name = "Sub 2" } }
            };
            TreeViewNodes.Add(node);
            node = new TreeViewModel()
            {
                Name = "Item 2"
            };
            TreeViewNodes.Add(node);

            SelectedNode = TreeViewNodes[0].Nodes[1];

            MainMenuItems = new ObservableCollection<INavigationViewItem>();
            MainMenuItems.Add(new NavigationViewItemModel() { IconPath = "/Assets/Icons/sale.png", LocalizeKey = "strNewSale" });
            MainMenuItems.Add(new NavigationViewItemModel() { IconPath = "/Assets/Icons/sale.png", LocalizeKey = "strNewSale", });
            MainMenuItems.Add(new NavigationViewItemModel() { IconPath = "/Assets/Icons/sale.png", LocalizeKey = "strNewSale" });

            SelectedItem = MainMenuItems[2];
            //SelectedItem = null;
        }

        private void ScanData(string barcode)
        {
            System.Diagnostics.Debug.WriteLine(barcode);
        }

        private async void RunTheThing()
        {
            bool res1 = await Command.IsExecuting.Any();
            if (await Command.IsExecuting.Any())
            {
                try
                {
                    //List<System.Drawing.Image> images = new List<System.Drawing.Image>();

                    ////images.Add(System.Drawing.Image.FromFile(@"C:\Users\serhii.rozniuk\Desktop\New folder\NewPdf0.png"));
                    //images.Add(System.Drawing.Image.FromFile(@"C:\Users\serhii.rozniuk\Desktop\wolf.png"));

                    //Printing.PrintService printService = new Printing.PrintService();
                    //printService.PageFormat = Printing.Enums.PageFormat.A4;
                    //printService.PageOrientation = Printing.Enums.PageOrientation.Portrait;
                    //printService.Pages = images;
                    //printService.CountCopies = 1;
                    //printService.PrintStatusChanged += (status) =>
                    //{
                    //    System.Diagnostics.Debug.WriteLine(status.ToString());
                    //};

                    //foreach (string name in printService.InstalledPrinters)
                    //{
                    //    // fdgdgdf
                    //    if (name.Contains("dimitar"))
                    //    {
                    //        printService.SelectedPrinter = name;
                    //        break;
                    //    }
                    //}

                    //printService.Print();
                }
                catch (Exception ex)
                {

                }
                return;
            }

            try
            {
                Services.Payment.IPaymentService paymentService = Splat.Locator.Current.GetRequiredService<Services.Payment.IPaymentService>();
                Services.Settings.ISettingsService settings = Splat.Locator.Current.GetRequiredService<Services.Settings.ISettingsService>();
                //Services.Payment.Device.DeviceSettings deviceSettings = new Services.Payment.Device.DeviceSettings(settings);
                settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceManufacturer].Value = "Datecs";
                settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceModel].Value = "DatecsFP2000";
                settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceProtocol].Value = "Lan";
                settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPAddress].Value = "192.168.60.105";
                settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceBaudRate].Value = "115200";
                settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIPPort].Value = "9100";
                settings.FiscalPrinterSettings[Enums.ESettingKeys.DeviceIsUsed].Value = "true";
                settings.FiscalPrinterSettings[Enums.ESettingKeys.DevicePassword].Value = "0000";
                settings.AppLanguage = Microinvest.CommonLibrary.Enums.ELanguages.Bulgarian;
                Services.Payment.Device.RealDevice realDevice = new Services.Payment.Device.RealDevice(settings);
                paymentService.SetPaymentTool(realDevice);

                var memory = paymentService.FiscalDevice.FiscalPrinterMemoryNumber;
                System.Collections.ObjectModel.ObservableCollection<OperationItemModel> items = new System.Collections.ObjectModel.ObservableCollection<OperationItemModel>();
                items.Add(new OperationItemModel()
                {
                    Item = new ItemModel()
                    {
                        Id = 1,
                        Name = "Test item",
                        VATGroup = new VATGroupModel()
                        {
                            Name = "A",
                            Value = 10,
                        }
                    },
                    Price = 4,
                    Qty = 5,
                    Discount = 0,

                });
                var res = paymentService.FiscalDevice.PayOrderAsync(items, Microinvest.CommonLibrary.Enums.EPaymentTypes.Cash, 20);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
