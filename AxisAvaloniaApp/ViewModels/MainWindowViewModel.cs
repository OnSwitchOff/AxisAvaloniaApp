using AxisAvaloniaApp.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxisAvaloniaApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public List<string> Source => new List<string>() { "item 1", "item 2", "item 3" };


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
        }

    }
}
