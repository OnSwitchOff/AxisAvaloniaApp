using Avalonia.Controls;
using AxisAvaloniaApp.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxisAvaloniaApp.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> viewModels;
        private Dictionary<string, IControl> activeViews;
        private ViewLocator viewLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        public NavigationService()
        {
            viewModels = new Dictionary<string, Type>();
            Configure<SaleViewModel>();
            Configure<DocumentViewModel>();
            Configure<InvoiceViewModel>();
            Configure<ProformInvoiceViewModel>();
            Configure<DebitNoteViewModel>();
            Configure<CreditNoteViewModel>();
            Configure<CashRegisterViewModel>();
            Configure<ExchangeViewModel>();
            Configure<ReportsViewModel>();
            Configure<SettingsViewModel>();

            activeViews = new Dictionary<string, IControl>();
            viewLocator = new ViewLocator();
        }

        /// <summary>
        /// Get IControl to show to user.
        /// </summary>
        /// <param name="viewModel">ViewModel for the next control.</param>
        /// <param name="control">Current control to cache data.</param>
        /// <returns>IControl.</returns>
        /// <date>20.05.2022.</date>
        public IControl NavigateTo(string viewModel, IControl control)
        {
            Type viewType = GetViewType(viewModel);

            if (control != null && control.DataContext is OperationViewModelBase modelBase)
            {
                // получаем ViewModel текущего контрола, который мы планируем кешировать
                string key = control.DataContext.GetType().FullName ?? string.Empty;

                switch (modelBase.IsCached)
                {
                    // если стоит опция кешировать данные
                    case true:
                        // если данные уже ранее кешировались - обновляем их
                        if (activeViews.ContainsKey(key))
                        {
                            activeViews[key] = control;
                        }
                        else
                        {
                            // в противном случае сохраняем их
                            activeViews.Add(key, control);
                        }
                        break;
                    // если лпция кеширования отключена
                    case false:
                        // если данные есть в списке доступных - удаляем их из списка, чтобы в следующий раз они не восстанавливались, а создавались "с нуля"
                        if (activeViews.ContainsKey(key))
                        {
                            activeViews.Remove(key);
                        }
                        break;
                }
            }

            // если ViewModel следующего контрола есть в списке доступных - восстанавливаем её
            if (activeViews.ContainsKey(viewModel))
            {
                return activeViews[viewModel];
            }

            // иначе создаём новых пустой контрол
            return viewLocator.Build(Splat.Locator.Current.GetService(viewType));
        }

        /// <summary>
        /// Get Type from list with avaolable ViewModels.
        /// </summary>
        /// <param name="key">Key to get Type of ViewModel.</param>
        /// <returns>Type of ViewModel.</returns>
        /// <date>20.05.2022.</date>
        private Type GetViewType(string key)
        {
            Type? viewType;
            lock (viewModels)
            {
                if (!viewModels.TryGetValue(key, out viewType))
                {
                    throw new ArgumentException($"ViewModel has not found: {key}. You should to register the ViewModel in the constructor.");
                }
            }

            return viewType;
        }

        /// <summary>
        /// Add ViewModel to list with available ViewModels.
        /// </summary>
        /// <typeparam name="TViewModel">Type of ViewModel.</typeparam>
        /// <date>20.05.2022.</date>
        private void Configure<TViewModel>() where TViewModel : ReactiveObject
        {
            lock (viewModels)
            {
                var key = typeof(TViewModel).FullName ?? string.Empty;

                if (viewModels.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }

                var type = typeof(TViewModel);
                if (viewModels.Any(page => page.Value == type))
                {
                    throw new ArgumentException($"This type is already configured with key {viewModels.First(p => p.Value == type).Key}");
                }

                viewModels.Add(key, type);
            }
        }
    }
}
