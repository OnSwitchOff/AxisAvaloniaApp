using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

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
    /// Class with base functionality of a ViewModel.
    /// </summary>
    public class ViewModelBase : ReactiveObject, INotifyDataErrorInfo
    {
        private string title;
        private string errorDescriptionKey;
        private List<string> validationPropertiesList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        public ViewModelBase()
        {
            IsCached = true;
            ViewId = Guid.NewGuid().ToString();
            validationPropertiesList = new List<string>();

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
        /// Occurs when the error of property is changed.
        /// </summary>
        /// <date>13.06.2022.</date>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

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
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        /// <date>13.06.2022.</date>
        public bool HasErrors => errorDescriptionKey != null;

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors for; or null or System.String.Empty, 
        /// to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        /// <date>13.06.2022.</date>
        public IEnumerable GetErrors(string? propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && validationPropertiesList.Contains(propertyName))
            {
                return new[] { errorDescriptionKey };
            }
            else
            {
                return null;
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


        /// <summary>
        /// Registers logic to validate property.
        /// </summary>
        /// <typeparam name="T">The type of the view model on which the property is to be validated.</typeparam>
        /// <typeparam name="P">The type the property is to be validated.</typeparam>
        /// <param name="viewModel">The view model on which the property is to be validated.</param>
        /// <param name="propName">The name the property is to be validated.</param>
        /// <param name="invalidData">Method that returns true if property is not valid.</param>
        /// <param name="errorDescriptionKey">The key to get description of the error from the dictionaty.</param>
        /// <date>13.06.2022.</date>
        protected void RegisterValidationData<T, P>(T viewModel, string propName, Func<bool> invalidData, string errorDescriptionKey) where T : ViewModelBase
        {
            ParameterExpression arg = Expression.Parameter(typeof(T), propName);
            MemberExpression property = Expression.Property(arg, propName);
            UnaryExpression conv = Expression.Convert(property, typeof(P));
            Expression<Func<T, P>> expr = Expression.Lambda<Func<T, P>>(conv, new ParameterExpression[] { arg });

            if (!validationPropertiesList.Contains(propName))
            {
                validationPropertiesList.Add(propName);
            }

            viewModel.WhenAnyValue(expr).Subscribe(_ =>
            {
                if (invalidData.Invoke())
                {
                    if (this.errorDescriptionKey == null)
                    {
                        this.errorDescriptionKey = errorDescriptionKey;
                        ErrorsChanged?.Invoke(viewModel, new DataErrorsChangedEventArgs(propName));
                    }
                }
                else
                {
                    this.errorDescriptionKey = null;
                    ErrorsChanged?.Invoke(viewModel, new DataErrorsChangedEventArgs(propName));
                }
            });
        }
    }
}
