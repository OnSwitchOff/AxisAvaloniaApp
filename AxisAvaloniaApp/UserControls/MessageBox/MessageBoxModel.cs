using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using ReactiveUI;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.UserControls.MessageBox
{
    public class MessageBoxModel : ReactiveObject
    {
        private bool canResize;
        private bool hasHeader;
        private bool hasIcon;
        private string contentTitle;
        private string contentHeader;
        private string contentHeaderKey;
        private string contentMessage;
        private string contentMessageKey;
        private string imagePath;
        private int? maxWidth;
        private MessageBox _window;
        private bool isOkShowed;
        private bool isYesShowed;
        private bool isNoShowed;
        private bool isAbortShowed;
        private bool isIgnoreShowed;
        private bool isCancelShowed;
        private bool isRetryShowed;
        private WindowStartupLocation locationOfMyWindow;

        public MessageBoxModel(MessageBoxParams @params)
        {
            SetParams(@params);
        }

        public void UpdateParams(MessageBoxParams @params)
        {
            IsOkShowed = false;
            IsYesShowed = false;
            IsNoShowed = false;
            IsAbortShowed = false;
            IsIgnoreShowed = false;
            IsCancelShowed = false;
            IsRetryShowed = false;
            
            SetParams(@params);
        }

        private void SetParams(MessageBoxParams @params)
        {
            HasHeader = true;
            HasIcon = true;

            if (@params.Icon != EButtonIcons.None)
            {
                ImagePath = String.Format("/Assets/Icons/MessageBox/{0}.ico", @params.Icon.ToString().ToLower());
            }
            else
            {
                HasIcon = false;
            }

            MaxWidth = @params.MaxWidth;
            CanResize = @params.CanResize;
            ContentTitle = @params.ContentTitle;
            ContentHeader = @params.ContentHeader;
            ContentHeaderKey = @params.ContentHeaderKey;
            ContentMessage = @params.ContentMessage;
            ContentMessageKey = @params.ContentMessageKey;
            _window = @params.Window;
            SetButtons(@params.ButtonDefinitions);
            if (string.IsNullOrEmpty(ContentHeader) && string.IsNullOrEmpty(ContentHeaderKey))
            {
                HasHeader = false;
            }

            if (@params.ShowInCenter)
            {
                LocationOfMyWindow = WindowStartupLocation.CenterScreen;
            }
            else
            {
                LocationOfMyWindow = WindowStartupLocation.Manual;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <date>14.06.2022.</date>
        public bool CanResize
        { 
            get => canResize;
            private set => this.RaiseAndSetIfChanged(ref canResize, value);
        }

        public bool HasHeader
        {
            get => hasHeader;
            private set => this.RaiseAndSetIfChanged(ref hasHeader, value);
        }

        public bool HasIcon
        {
            get => hasIcon;
            private set => this.RaiseAndSetIfChanged(ref hasIcon, value);
        }

        public string ContentTitle
        {
            get => contentTitle;
            private set => this.RaiseAndSetIfChanged(ref contentTitle, value);
        }

        public string ContentHeaderKey
        {
            get => contentHeaderKey;
            private set => this.RaiseAndSetIfChanged(ref contentHeaderKey, value);
        }

        public string ContentHeader
        {
            get => contentHeader;
            private set => this.RaiseAndSetIfChanged(ref contentHeader, value);
        }

        public string ContentMessage
        {
            get => contentMessage;
            private set => this.RaiseAndSetIfChanged(ref contentMessage, value);
        }

        public string ContentMessageKey
        {
            get => contentMessageKey;
            private set => this.RaiseAndSetIfChanged(ref contentMessageKey, value);
        }

        public string ImagePath
        {
            get => imagePath;
            private set => this.RaiseAndSetIfChanged(ref imagePath, value);
        }

        public int? MaxWidth
        {
            get => maxWidth;
            private set => this.RaiseAndSetIfChanged(ref maxWidth, value);
        }

        public bool IsOkShowed
        {
            get => isOkShowed;
            private set => this.RaiseAndSetIfChanged(ref isOkShowed, value);
        }

        public bool IsYesShowed
        {
            get => isYesShowed;
            private set => this.RaiseAndSetIfChanged(ref isYesShowed, value);
        }

        public bool IsNoShowed
        {
            get => isNoShowed;
            private set => this.RaiseAndSetIfChanged(ref isNoShowed, value);
        }

        public bool IsAbortShowed
        {
            get => isAbortShowed;
            private set => this.RaiseAndSetIfChanged(ref isAbortShowed, value);
        }

        public bool IsIgnoreShowed
        {
            get => isIgnoreShowed;
            private set => this.RaiseAndSetIfChanged(ref isIgnoreShowed, value);
        }

        public bool IsCancelShowed
        {
            get => isCancelShowed;
            private set => this.RaiseAndSetIfChanged(ref isCancelShowed, value);
        }

        public bool IsRetryShowed
        {
            get => isRetryShowed;
            private set => this.RaiseAndSetIfChanged(ref isRetryShowed, value);
        }

        public WindowStartupLocation LocationOfMyWindow
        {
            get => locationOfMyWindow;
            private set => this.RaiseAndSetIfChanged(ref locationOfMyWindow, value);
        }

        private void SetButtons(EButtons paramsButtonDefinitions)
        {
            switch (paramsButtonDefinitions)
            {
                case EButtons.Ok:
                    IsOkShowed = true;
                    break;
                case EButtons.YesNo:
                    IsYesShowed = true;
                    IsNoShowed = true;
                    break;
                case EButtons.OkCancel:
                    IsOkShowed = true;
                    IsCancelShowed = true;
                    break;
                case EButtons.OkAbort:
                    IsOkShowed = true;
                    IsAbortShowed = true;
                    break;
                case EButtons.YesNoCancel:
                    IsYesShowed = true;
                    IsNoShowed = true;
                    IsCancelShowed = true;
                    break;
                case EButtons.YesNoAbort:
                    IsYesShowed = true;
                    IsNoShowed = true;
                    IsAbortShowed = true;
                    break;
                case EButtons.AbortRetryIgnore:
                    IsAbortShowed = true;
                    IsRetryShowed = true;
                    IsIgnoreShowed = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramsButtonDefinitions), paramsButtonDefinitions, null);
            }
        }

        public void ButtonClick(EButtonResults result)
        {
            _window.Result = result;
            _window.Close();
        }

        public async Task Copy()
        {
            await AvaloniaLocator.Current.GetService<IClipboard>().SetTextAsync(ContentMessage);
        }
    }
}
