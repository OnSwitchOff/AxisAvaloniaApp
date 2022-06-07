using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Reports;
using AxisAvaloniaApp.ViewModels;
using System;
using System.ComponentModel;

namespace AxisAvaloniaApp.Views
{
    public partial class ReportsView : UserControl
    {
        private DataGrid reportGrid;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsView"/> class.
        /// </summary>
        public ReportsView()
        {
            InitializeComponent();

            this.DataContext = Splat.Locator.Current.GetRequiredService<ReportsViewModel>();
            if (this.DataContext is ReportsViewModel reportsView)
            {
                reportsView.ViewClosing += ReportsView_ViewClosing;
                reportsView.PropertyChanged += DataContext_PropertyChanged;
            }

            IsPlaceholderVisible = true;
            reportGrid = this.FindControl<DataGrid>("ReportGrid");
        }

        public static readonly StyledProperty<bool> IsPlaceholderVisibleProperty =
           AvaloniaProperty.Register<ReportsView, bool>(nameof(IsPlaceholderVisible));

        /// <summary>
        /// Gets or sets a value indicating whether placeholder is visible.
        /// </summary>
        /// <date>07.06.2022.</date>
        public bool IsPlaceholderVisible
        {
            get => GetValue(IsPlaceholderVisibleProperty);
            set => SetValue(IsPlaceholderVisibleProperty, value);
        }

        /// <summary>
        /// Sets style to row with total data when rows of DataGrid are loading.
        /// </summary>
        /// <param name="sender">DataGrid.</param>
        /// <param name="e">DataGridRowEventArgs.</param>
        /// <date>07.06.2022.</date>
        private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            if (e.Row.DataContext is IReportModel model && model.IsTotalRow && e.Row.IsInitialized)
            {
                e.Row.Background = Avalonia.Media.Brushes.LightSeaGreen;
                e.Row.FontWeight = Avalonia.Media.FontWeight.Bold;
            }
        }

        /// <summary>
        /// Unsubscribes from events when UserControl is closing.
        /// </summary>
        /// <param name="viewId">Id of the current UserControl.</param>
        /// <date>07.06.2022.</date>
        private void ReportsView_ViewClosing(string viewId)
        {
            if (this.DataContext is ReportsViewModel reportsView)
            {
                reportsView.ViewClosing -= ReportsView_ViewClosing;
                reportsView.PropertyChanged -= DataContext_PropertyChanged;
            }
        }

        /// <summary>
        /// Change dependencies properties when base property is changed.
        /// </summary>
        /// <param name="sender">ReportsViewModel.</param>
        /// <param name="e">PropertyChangedEventArgs.</param>
        /// <date>07.06.2022.</date>
        private void DataContext_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ReportsViewModel.ColumnsData):
                    IsPlaceholderVisible = false;

                    reportGrid.Columns.Clear();

                    if (this.DataContext is ReportsViewModel reportsView)
                    {
                        DataGridTextColumn textColumn;
                        foreach (ReportDataModel dataModel in reportsView.ColumnsData)
                        {
                            textColumn = new DataGridTextColumn()
                            {
                                Header = new UserControls.Extensions.AxisTextBlock()
                                {
                                    LocalizeTextKey = dataModel.HeaderKey,
                                },
                                Binding = new Binding(dataModel.DataKey),
                                Width = Double.IsNaN(dataModel.ColumnWidth) ? new DataGridLength(1, DataGridLengthUnitType.Star) : new DataGridLength(dataModel.ColumnWidth),
                                MinWidth = 80,
                                CellStyleClasses = new Classes(dataModel.HorizontalAlignment.ToString()),

                            };

                            reportGrid.Columns.Add(textColumn);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Initialize of the components.
        /// </summary>
        /// <date>07.06.2022.</date>
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
