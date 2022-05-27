using ReactiveUI;
using System;

namespace AxisAvaloniaApp.ViewModels
{
    /// <summary>
    /// Describes structure of the PageClosing event.
    /// </summary>
    /// <param name="viewId">Id of the page.</param>
    /// <date>26.05.2022.</date>
    public delegate void ViewClosingDelegate(string viewId);

    /// <summary>
    /// Describes structure of PageTitleChanged event.
    /// </summary>
    /// <param name="newTitle">New title of a page.</param>
    /// <date>26.05.2022.</date>
    public delegate void ViewTitleChangingDelegate(string newTitle);


    public class ViewModelBase : ReactiveObject
    {
        public ViewModelBase()
        {
            IsCached = true;
            ViewId = Guid.NewGuid().ToString();

            CloseViewCommand = ReactiveCommand.Create(CloseView);
        }

        public event ViewClosingDelegate ViewClosing;
        public event ViewTitleChangingDelegate ViewTitleChanging;

        public bool IsCached { get; protected set; }
        public string ViewId { get; }

        public IReactiveCommand CloseViewCommand { get; }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);

                ViewTitleChanging?.Invoke(title);
            }
        }

        protected virtual void CloseView()
        {
            IsCached = false;

            ViewClosing?.Invoke(ViewId);
        }
    }
}
