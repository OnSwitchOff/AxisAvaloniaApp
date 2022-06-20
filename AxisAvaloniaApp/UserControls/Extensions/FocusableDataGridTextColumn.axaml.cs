using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AxisAvaloniaApp.UserControls.Extensions
{
    public partial class FocusableDataGridTextColumn : DataGridTextColumn
    {
        private IControl editingItem;
        public FocusableDataGridTextColumn()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void Test(DataGridCell cell, object dataItem)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                {
                    IControl control = this.GenerateEditingElementDirect(cell, dataItem);
                    control.Focus();
                });
            });

            //return editingItem;
        }

        protected override object PrepareCellForEdit(IControl editingElement, RoutedEventArgs editingEventArgs)
        {
            Avalonia.Input.MouseDevice mouseDevice = new Avalonia.Input.MouseDevice();
            EventRoute eventRoute = new EventRoute(new RoutedEvent("", RoutingStrategies.Direct, typeof(string), typeof(DataGridCell)));
            DataGridCell cell = new DataGridCell();
            //eventRoute.RaiseEvent(cell, new RoutedEventArgs(new PointerPressedEventArgs()));
            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }

        protected override IControl GenerateEditingElementDirect(DataGridCell cell, object dataItem)
        {
            editingItem = base.GenerateEditingElementDirect(cell, dataItem);
            return editingItem;
        }
    }
}
