using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.UserControls.Models;
using System;
using System.Collections.Generic;

namespace AxisAvaloniaApp.Views
{
    public partial class DocumentView : UserControl
    {
        public DocumentView()
        {
            this.Resources.Add("SaleColumnWidth", new DataGridLength(100));
            InitializeComponent();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }


}
