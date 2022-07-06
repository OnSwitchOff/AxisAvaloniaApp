using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Serialization;
using AxisAvaloniaApp.UserControls.Models;
using AxisAvaloniaApp.ViewModels;
using System;
using System.Collections.Generic;
using Splat;
using AxisAvaloniaApp.UserControls.Extensions;

namespace AxisAvaloniaApp.Views
{
    public partial class DocumentView : UserControl
    {
        private ContextMenu documentContextMenu;
        private readonly ISerializationService serializationService;

        public DocumentView()
        {


        }

        public DocumentView(ESerializationGroups documentType, OperationViewModelBase viewModel) : this()
        {
            serializationService = Splat.Locator.Current.GetRequiredService<ISerializationService>();
            serializationService.InitSerializationData(documentType);
            InitSerializedResources();
            InitializeComponent();
            LoadContextMenuValues();           

            this.DataContext = viewModel;
            viewModel.ViewClosing += DocumentView_ViewClosing;
        }      

        private void InitSerializedResources()
        {
            this.Resources.Add("ColAcctWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColAcctWidth]));
            this.Resources.Add("ColDateWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColDateWidth]));
            this.Resources.Add("ColCompanyWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColCompanyWidth]));
            this.Resources.Add("ColCityWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColCityWidth]));
            this.Resources.Add("ColAddressWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColAddressWidth]));
            this.Resources.Add("ColPhoneWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColPhoneWidth]));
            this.Resources.Add("ColSumWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColSumWidth]));
            this.Resources.Add("ColDocumentNumberWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColDocumentNumberWidth]));
            this.Resources.Add("ColDocumentDateWidth", new DataGridLength((double)serializationService[ESerializationKeys.ColDocumentDateWidth]));

            this.Resources.Add("AddColumns", new DataGridLength((double)serializationService[ESerializationKeys.AddColumns]));
            this.Resources.Add("SelectedPeriod", new DataGridLength((double)serializationService[ESerializationKeys.SelectedPeriod]));
            this.Resources.Add("DateFrom", serializationService[ESerializationKeys.DateFrom]);
            this.Resources.Add("DateTo", serializationService[ESerializationKeys.DateTo]);
        }

        private void DocumentView_ViewClosing(string viewId)
        {
            (this.DataContext as OperationViewModelBase).ViewClosing -= DocumentView_ViewClosing;

            DataGrid dg = this.FindControl<DataGrid>("dg");

            serializationService[ESerializationKeys.ColAcctWidth].Value = dg.Columns[0].ActualWidth.ToString();
            serializationService[ESerializationKeys.ColDateWidth].Value = dg.Columns[1].ActualWidth.ToString();
            serializationService[ESerializationKeys.ColCityWidth].Value = dg.Columns[3].ActualWidth.ToString();
            serializationService[ESerializationKeys.ColAddressWidth].Value = dg.Columns[4].ActualWidth.ToString();
            serializationService[ESerializationKeys.ColPhoneWidth].Value = dg.Columns[5].ActualWidth.ToString();
            serializationService[ESerializationKeys.ColSumWidth].Value = dg.Columns[6].ActualWidth.ToString();
            serializationService[ESerializationKeys.ColDocumentNumberWidth].Value = dg.Columns[7].ActualWidth.ToString();
            serializationService[ESerializationKeys.ColDocumentDateWidth].Value = dg.Columns[8].ActualWidth.ToString();

            SaveContextMenuValues();          

            serializationService.Update();
        }

        private void LoadContextMenuValues()
        {
            documentContextMenu = this.FindControl<ContextMenu>("DocumentContextMenu");
            if (documentContextMenu != null && !string.IsNullOrEmpty(serializationService[ESerializationKeys.AddColumns].Value))
            {
                var x = serializationService[ESerializationKeys.AddColumns];
                int addColumnsValue = int.Parse(serializationService[ESerializationKeys.AddColumns].Value);
                var test = (EAdditionalDocumentColumns)(serializationService[ESerializationKeys.AddColumns]);
                foreach (CheckedMenuItem item in documentContextMenu.Items)
                {
                    if (item.Tag != null && item.Tag is EAdditionalDocumentColumns column)
                    {
                        item.IsChecked = ((EAdditionalDocumentColumns)addColumnsValue & column) > 0;
                    }
                }
            }
        }

        private void SaveContextMenuValues()
        {
            EAdditionalDocumentColumns tableColumns = 0;
            if (documentContextMenu != null)
            {
                foreach (CheckedMenuItem item in documentContextMenu.Items)
                {
                    if (item.IsChecked && item.Tag != null && item.Tag is EAdditionalDocumentColumns column)
                    {
                        tableColumns = tableColumns | column;
                    }
                }
            }
            serializationService[ESerializationKeys.AddColumns].Value = ((int)tableColumns).ToString();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }


}
