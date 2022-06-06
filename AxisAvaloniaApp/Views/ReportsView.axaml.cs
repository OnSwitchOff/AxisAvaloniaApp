using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.Views
{
    public partial class ReportsView : UserControl
    {
        private List<ViewModel> ViewModels = new List<ViewModel>() { new ViewModel("1", "1.1"), new ViewModel("2", "2.1"), } ;
        public ReportsView()
        {
            InitializeComponent();

            this.DataContext = Splat.Locator.Current.GetRequiredService<ViewModels.ReportsViewModel>();

            DataGrid grid = this.FindControl<DataGrid>("ReportGrid");
            //foreach (var column in Columns)
            //{
            //    grid.Columns.Add(column);
            //}
            
            //grid.Items = ViewModels;
            //DataGridTextColumn textColumn = new DataGridTextColumn();
            //textColumn.Header = new TextBlock()
            //{ 
            //    Text = "Item 1",
            //};
            //textColumn.Binding = new Binding("Title");

            //grid.Columns.Add(textColumn);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public ObservableCollection<DataGridTextColumn> Columns { get; private set; } = new ObservableCollection<DataGridTextColumn>()
        {
            new DataGridTextColumn() {Header = "Header 1"},
            new DataGridTextColumn() {Header = "Header 2"},
        };

    }

    public class ViewModel
    {
        public ViewModel(string t, string tt)
        {
            Title = t;
            Title2 = tt;
        }
        public string Title { get; set; }
        public string Title2 { get; set; }
    }
}
