using ReactiveUI;
using System;

namespace AxisAvaloniaApp.ViewModels
{
    public delegate void ViewClosingDelegate(string viewId);
    public class ViewModelBase : ReactiveObject
    {
        public ViewModelBase()
        {
            IsCached = true;
            ViewId = Guid.NewGuid().ToString();

            CloseViewCommand = ReactiveCommand.Create(CloseView);
        }

        public event ViewClosingDelegate ViewClosing;

        public bool IsCached { get; protected set; }
        public string ViewId { get; }

        public IReactiveCommand CloseViewCommand { get; }

        private void CloseView()
        {
            IsCached = false;

            ViewClosing?.Invoke(ViewId);
        }
    }
}
