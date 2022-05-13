using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.UserControls.Models
{
    public class MainMenuItemModel : ReactiveObject
    {
        private string iconPath;

        private string localizeKey;

        private UserControl content;

        private bool isSelected = false;

        public string IconPath
        {
            get => iconPath;
            set => this.RaiseAndSetIfChanged(ref iconPath, value);
        }

        public string LocalizeKey
        {
            get => localizeKey;
            set => this.RaiseAndSetIfChanged(ref localizeKey, value);
        }

        public UserControl Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        public bool IsSelected
        {
            get => isSelected;
            set => this.RaiseAndSetIfChanged(ref isSelected, value);
        }
    }
}
