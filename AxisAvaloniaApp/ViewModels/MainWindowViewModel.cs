using Avalonia.Controls;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.UserControls.Models;
using DataBase.Repositories.ApplicationLog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AxisAvaloniaApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private bool mainMenuExpanded = true;
        private double mainMenuWidth = 200;
        public bool MainMenuExpanded
        {
            get => mainMenuExpanded;
            set
            {
                mainMenuExpanded = value;

                MainMenuWidth = mainMenuExpanded ? 200 : 40;
            }
        }

        public double MainMenuWidth
        {
            get => mainMenuWidth;
            set => this.RaiseAndSetIfChanged(ref mainMenuWidth, value);
        }

        private ObservableCollection<MainMenuItemModel> mainMenuItems;

        public ObservableCollection<MainMenuItemModel> MainMenuItems
        {
            get => mainMenuItems;
            set => this.RaiseAndSetIfChanged(ref mainMenuItems, value);
        }

        private MainMenuItemModel selectedItem;
        public MainMenuItemModel SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != null)
                {
                    selectedItem.IsSelected = false;
                }
                if (value != null)
                {
                    value.IsSelected = true;
                }
                this.RaiseAndSetIfChanged(ref selectedItem, value);
            }
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

            MainMenuItems = new ObservableCollection<MainMenuItemModel>();
            MainMenuItems.Add(new MainMenuItemModel() { IconPath = "/Assets/Icons/sale.png", LocalizeKey = "strNewSale" });
            MainMenuItems.Add(new MainMenuItemModel() { IconPath = "/Assets/Icons/sale.png", LocalizeKey = "strNewSale" });
            MainMenuItems.Add(new MainMenuItemModel() { IconPath = "/Assets/Icons/sale.png", LocalizeKey = "strNewSale" });

            SelectedItem = MainMenuItems[2];
            //SelectedItem = null;
        }

    }
}
