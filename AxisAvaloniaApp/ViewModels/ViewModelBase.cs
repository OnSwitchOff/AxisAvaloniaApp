using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AxisAvaloniaApp.ViewModels
{
    /// <summary>
    /// Class with base functionality of a ViewModel.
    /// </summary>
    public class ViewModelBase : ReactiveObject, INotifyDataErrorInfo
    {
        private string errorDescriptionKey;
        private List<string> validationPropertiesList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        public ViewModelBase()
        {
            validationPropertiesList = new List<string>();
        }

        /// <summary>
        /// Occurs when the error of property is changed.
        /// </summary>
        /// <date>13.06.2022.</date>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

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
