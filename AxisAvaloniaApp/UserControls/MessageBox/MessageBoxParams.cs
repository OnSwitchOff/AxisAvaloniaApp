using Avalonia.Media.Imaging;

namespace AxisAvaloniaApp.UserControls.MessageBox
{
    public class MessageBoxParams
    {
        public MessageBoxParams()
        {
            CanResize = false;
            ShowInCenter = true;
            ContentTitle = string.Empty;
            ContentHeader = null;
            ContentHeaderKey = string.Empty;
            ContentMessage = string.Empty;
            ContentMessageKey = string.Empty;
            MaxWidth = null;
            ButtonDefinitions = EButtons.Ok;
            Icon = EButtonIcons.Avalonia;
            WindowIcon = null;
            Style = EMessageBoxStyles.None;
        }

        public bool CanResize { get; set; }

        public bool ShowInCenter { get; set; }

        public string ContentTitle { get; set; }

        public string ContentHeader { get; set; }

        public string ContentHeaderKey { get; set; }

        public string ContentMessage { get; set; }

        public string ContentMessageKey { get; set; }

        public int? MaxWidth { get; set; }

        public EButtons ButtonDefinitions { get; set; }

        public EButtonIcons Icon { get; set; }

        public Bitmap WindowIcon { get; set; } 

        public EMessageBoxStyles Style { get; set; }

        public MessageBox Window { get; set; }
    }
}
