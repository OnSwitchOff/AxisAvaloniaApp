using AxisAvaloniaApp.UserControls.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class ExchangeViewModel : OperationViewModelBase
    {
        private bool isImportDirection;
        private ObservableCollection<ComboBoxItemModel> importFromItems;
        private ComboBoxItemModel selectedImportFromItem;
        private ObservableCollection<ComboBoxItemModel> exportToItems;
        private ComboBoxItemModel selectedExportToItem;
        private bool isSaleOperation;
        private string numberFrom;
        private string numberTo;
        private DateTime dateFrom;
        private DateTime dateTo;

        public bool IsImportDirection { get => isImportDirection; set { this.RaiseAndSetIfChanged(ref isImportDirection, value); this.RaisePropertyChanged("IsAnotherApplication");}  }
        public ObservableCollection<ComboBoxItemModel> ImportFromItems
        {
            get => importFromItems;
            set => this.RaiseAndSetIfChanged(ref importFromItems, value);
        }
        public ComboBoxItemModel SelectedImportFromItem { get => selectedImportFromItem; set { this.RaiseAndSetIfChanged(ref selectedImportFromItem, value); this.RaisePropertyChanged("IsAnotherApplication"); } }
        public ObservableCollection<ComboBoxItemModel> ExportToItems
        {
            get => exportToItems;
            set => this.RaiseAndSetIfChanged(ref exportToItems, value);
        }
        public ComboBoxItemModel SelectedExportToItem { get => selectedExportToItem; set { this.RaiseAndSetIfChanged(ref selectedExportToItem, value);  this.RaisePropertyChanged("IsAnotherApplication"); } }
        public bool IsSaleOperation { 
            get => isSaleOperation;
            set => this.RaiseAndSetIfChanged(ref isSaleOperation, value);
        }
        public bool IsAnotherApplication { get => (IsImportDirection && SelectedImportFromItem != null &&  SelectedImportFromItem.Key == "item1") || (!IsImportDirection && SelectedExportToItem != null && SelectedExportToItem.Key == "item1"); }
        public string NumberFrom { get => numberFrom; set => this.RaiseAndSetIfChanged(ref numberFrom, value); }
        public string NumberTo { get => numberTo; set => this.RaiseAndSetIfChanged(ref numberTo, value); }
        public DateTime DateFrom { get => dateFrom; set => this.RaiseAndSetIfChanged(ref dateFrom, value); }
        public DateTime DateTo { get => dateTo; set => this.RaiseAndSetIfChanged(ref dateTo, value); }

        #region Commands
        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
        #endregion


        public ExchangeViewModel()
        {
            ExecuteCommand = ReactiveCommand.Create(Execute);
            ClearCommand = ReactiveCommand.Create(Clear);

            ImportFromItems = GetImportFromItemsCollection();
            ExportToItems = GetExportToItemsCollection();
            DateTo = DateTime.Now;
            DateFrom = DateTo.AddDays(-5);


        }

        private ObservableCollection<ComboBoxItemModel> GetImportFromItemsCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            for (int i = 0; i < 3; i++)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Key = "item" + i;
                item.Value = item.Key;
                result.Add(item);
            }
            
            return result;
        }
        private ObservableCollection<ComboBoxItemModel> GetExportToItemsCollection()
        {
            ObservableCollection<ComboBoxItemModel> result = new ObservableCollection<ComboBoxItemModel>();

            for (int i = 0; i < 3; i++)
            {
                ComboBoxItemModel item = new ComboBoxItemModel();
                item.Key = "item" + i;
                item.Value = item.Key;
                result.Add(item);
            }

            return result;
        }

        private void Execute()
        {
        
        }

        private void Clear()
        {

        }
    }
}
