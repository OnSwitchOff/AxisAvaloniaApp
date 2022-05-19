using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using System;

namespace AxisAvaloniaApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.MainWindowViewModel>();
            ListBox listBox = new ListBox();
            listBox.PointerPressed += fgfg;

            Button button = new Button();
            button.Click += ButtonClick;
        }

        private void ButtonClick(object? sender, RoutedEventArgs e)
        {
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
                settings.AppLanguage = Microinvest.CommonLibrary.Enums.ELanguages.Bulgarian;
                Services.Payment.Device.RealDevice realDevice = new Services.Payment.Device.RealDevice(settings);
                paymentService.SetPaymentTool(realDevice);

                var memory = paymentService.FiscalDevice.FiscalPrinterMemoryNumber;
                System.Collections.ObjectModel.ObservableCollection<OperationItemModel> items = new System.Collections.ObjectModel.ObservableCollection<OperationItemModel>();
                items.Add(new OperationItemModel()
                {
                    Item = new ItemModel()
                    {
                        Name = "Test item",
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

        private void fgfg(object? sender, PointerPressedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MainMenuItem_PointerEnter(object? sender, PointerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void MainMenuItem_PointerLeave(object? sender, PointerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void OnCloseClicked(object sender, EventArgs args)
        {
            Close();
        }

    }
}
