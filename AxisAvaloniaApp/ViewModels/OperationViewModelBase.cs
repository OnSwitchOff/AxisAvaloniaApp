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

    /// <summary>
    /// Class with base functionality of a ViewModel for operation.
    /// </summary>
    public class OperationViewModelBase : ViewModelBase
    {
        private string title;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationViewModelBase"/> class.
        /// </summary>
        public OperationViewModelBase()
        {
            IsCached = true;
            ViewId = Guid.NewGuid().ToString();

            CloseViewCommand = ReactiveCommand.Create(CloseView);
        }

        /// <summary>
        /// Occurs when the View is closing.
        /// </summary>
        /// <date>26.05.2022.</date>
        public event ViewClosingDelegate ViewClosing;

        /// <summary>
        /// Occurs when the title of View is changing.
        /// </summary>
        /// <date>26.05.2022.</date>
        public event ViewTitleChangingDelegate ViewTitleChanging;

        /// <summary>
        /// Gets or sets a value indicating whether the current View Model data should be saved for restoring next time.
        /// </summary>
        /// <date>26.05.2022.</date>
        public bool IsCached { get; protected set; }

        /// <summary>
        /// Gets id of the View Model.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string ViewId { get; }

        /// <summary>
        /// Gets command that is called when the button "x" is pressed.
        /// </summary>
        /// <date>26.05.2022.</date>
        public IReactiveCommand CloseViewCommand { get; }

        /// <summary>
        /// Gets or sets title of the view.
        /// </summary>
        /// <date>26.05.2022.</date>
        public string Title
        {
            get => title;
            set
            {
                this.RaiseAndSetIfChanged(ref title, value);

                ViewTitleChanging?.Invoke(title);
            }
        }

        /// <summary>
        /// Offs cache mode and invokes View Closing event when view is closing.
        /// </summary>
        /// <date>26.05.2022.</date>
        protected virtual void CloseView()
        {
            IsCached = false;

            ViewClosing?.Invoke(ViewId);
        }
    }
}
