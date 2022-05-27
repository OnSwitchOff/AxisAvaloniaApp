using ReactiveUI;

namespace AxisAvaloniaApp.UserControls.Models
{
    public class NavigationViewItemModel : ReactiveObject, NavigationView.INavigationViewItem
    {
        private string iconPath;

        private string localizeKey;

        private string text;

        private Avalonia.Controls.IControl content;

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

        public string Text
        {
            get => text;
            set => this.RaiseAndSetIfChanged(ref text, value);
        }

        public Avalonia.Controls.IControl Content
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
