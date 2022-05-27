using Avalonia.Controls;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Explanation;
using AxisAvaloniaApp.Services.Navigation;
using AxisAvaloniaApp.UserControls.NavigationView;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace AxisAvaloniaApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly INavigationService navigationService;
        private readonly IExplanationService explanationService;
        private NavigationViewItem selectedItem;
        private ObservableCollection<INavigationViewItem> activeSales;
        private INavigationViewItem selectedSale;
        private IControl content;
        private string explanation;
        private string licenseData = "Some license data!";
        private readonly IControl WelcomeView;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            navigationService = Splat.Locator.Current.GetRequiredService<INavigationService>();
            explanationService = Splat.Locator.Current.GetRequiredService<IExplanationService>();
            explanationService.PropertyChanged += Explanation_PropertyChanged;
            activeSales = new ObservableCollection<INavigationViewItem>();
            GoToYouTubeCommand = ReactiveCommand.Create(GoToYouTubeAsync);

            IsPointerPressedActivation = false;

            WelcomeView = new Views.WelcomeView();

            this.PropertyChanged += MainWindowViewModel_PropertyChanged;
            this.PropertyChanging += MainWindowViewModel_PropertyChanging;
        }

        /// <summary>
        /// Gets or sets item of menu selected by user.
        /// </summary>
        /// <date>23.05.2022.</date>
        public NavigationViewItem SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the SelectedItem is set from code.
        /// </summary>
        /// <date>25.05.2022.</date>
        public bool IsPointerPressedActivation { get; set; }

        /// <summary>
        /// Gets or sets list with active sales.
        /// </summary>
        /// <date>23.05.2022.</date>
        public ObservableCollection<INavigationViewItem> ActiveSales
        {
            get => activeSales;
            set => this.RaiseAndSetIfChanged(ref activeSales, value);
        }

        /// <summary>
        /// Gets or sets active sale selected by user.
        /// </summary>
        /// <date>23.05.2022.</date>
        public INavigationViewItem SelectedSale
        {
            get => selectedSale;
            set => this.RaiseAndSetIfChanged(ref selectedSale, value);
        }

        /// <summary>
        /// Gets or sets control with operation data.
        /// </summary>
        /// <date>23.05.2022.</date>
        public IControl Content
        {
            get => content;
            private set => this.RaiseAndSetIfChanged(ref content, value);
        }

        /// <summary>
        /// Gets or sets string with explanation of the control.
        /// </summary>
        /// <date>23.05.2022.</date>
        public string Explanation
        {
            get => explanation;
            set => this.RaiseAndSetIfChanged(ref explanation, value);
        }

        /// <summary>
        /// Gets commant to go to YouTube.
        /// </summary>
        /// <date>23.05.2022.</date>
        public IReactiveCommand GoToYouTubeCommand { get; }

        /// <summary>
        /// Gets string with the current date and time.
        /// </summary>
        /// <date>23.05.2022.</date>
        public IObservable<string> AppDateTime { get; } = Observable.Timer(DateTimeOffset.Now, new TimeSpan(0, 0, 0, 1)).Select(_ => System.DateTime.Now.ToString());

        /// <summary>
        /// Gets or sets string with data of license.
        /// </summary>
        /// <date>23.05.2022.</date>
        public string LicenseData
        {
            get => licenseData;
            set => this.RaiseAndSetIfChanged(ref licenseData, value);
        }

        /// <summary>
        /// Change dependencies properties when base property is changing.
        /// </summary>
        /// <param name="sender">MainWindowViewModel.</param>
        /// <param name="e">PropertyChangingEventArgs.</param>
        /// <date>26.05.2022.</date>
        private void MainWindowViewModel_PropertyChanging(object? sender, PropertyChangingEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedItem):
                    if (SelectedItem != null)
                    {
                        SelectedItem.IsSelected = false;
                        navigationService.NavigateTo(
                        SelectedItem.GetValue(NavigationExtensions.NavigateToProperty),
                        Content);
                    }
                    break;
                case nameof(SelectedSale):
                    if (SelectedSale != null)
                    {
                        SelectedSale.IsSelected = false;
                    }
                    break;
                case nameof(Content):
                    if (Content != null && Content.DataContext != null && Content.DataContext is ViewModelBase viewModel)
                    {
                        viewModel.ViewTitleChanging -= View_PageTitleChanging;
                        viewModel.ViewClosing -= View_ViewClosing;
                    }
                    break;
            }
        }

        /// <summary>
        /// Change dependencies properties when base property is changed.
        /// </summary>
        /// <param name="sender">MainWindowViewModel.</param>
        /// <param name="e">PropertyChangedEventArgs.</param>
        /// <date>26.05.2022.</date>
        private void MainWindowViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.PropertyName);
            switch (e.PropertyName)
            {
                case nameof(SelectedItem):
                    if (SelectedItem == null)
                    {
                        if (SelectedSale == null)
                        {
                            Content = WelcomeView;
                        }
                    }
                    else
                    {
                        if (SelectedSale != null)
                        {
                            SelectedSale.IsSelected = false;
                            SelectedSale = null;
                        }

                        Content = navigationService.NavigateTo(
                        SelectedItem.GetValue(NavigationExtensions.NavigateToProperty),
                        Content);
                    }
                    break;
                case nameof(SelectedSale):
                    if (SelectedSale == null)
                    {
                        Content = WelcomeView;
                    }
                    else
                    {
                        if (selectedItem != null)
                        {
                            SelectedItem.IsSelected = false;
                            SelectedItem = null;
                        }

                        SelectedSale.IsSelected = true;
                        Content = SelectedSale.Content;
                    }
                    break;
                case nameof(Content):
                    if (Content != null && Content.DataContext != null && Content.DataContext is ViewModelBase viewModel)
                    {
                        viewModel.ViewTitleChanging += View_PageTitleChanging;
                        viewModel.ViewClosing += View_ViewClosing;

                        if (Content is Views.SaleView saleView && SelectedItem != null)
                        {
                            if (IsPointerPressedActivation)
                            {
                                ActiveSales.Add(new UserControls.Models.NavigationViewItemModel()
                                {
                                    Text = SelectedItem.Text,
                                    IsSelected = true,
                                    Content = Content,
                                });

                                viewModel.Title = SelectedItem.Text;
                            }

                            SelectedSale = ActiveSales[ActiveSales.Count - 1];
                            IsPointerPressedActivation = false;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Update text of NavigationViewItem if title of view is updated.
        /// </summary>
        /// <param name="newTitle">New title of view.</param>
        /// <date>26.05.2022.</date>
        private void View_PageTitleChanging(string newTitle)
        {
            if (SelectedSale != null)
            {
                SelectedSale.Text = newTitle;
            }
        }

        /// <summary>
        /// Remove view from the list with active views and set next view when the current view was closed.
        /// </summary>
        /// <param name="viewId">Id os the current view.</param>
        /// <date>26.05.2022.</date>
        private void View_ViewClosing(string viewId)
        {
            INavigationViewItem closingView = ActiveSales.FirstOrDefault(i => (i.Content.DataContext as ViewModelBase).ViewId == viewId);
            if (closingView != null)
            {
                ActiveSales.Remove(closingView);
            }

            if (ActiveSales.Count > 0)
            {
                SelectedSale = ActiveSales[ActiveSales.Count - 1];
                Content = SelectedSale.Content;
            }
            else
            {
                Content = WelcomeView;
            }

            SelectedItem = null;
        }

        /// <summary>
        /// Go to YouTube.
        /// </summary>
        /// <date>23.05.2022.</date>
        private async void GoToYouTubeAsync()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                string URL = "https://www.youtube.com/channel/UCgHHhDj1hskaBTWwQU4rRWQ/playlists";
                if (Uri.IsWellFormedUriString(URL, UriKind.RelativeOrAbsolute))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = URL,
                        UseShellExecute = true,
                    });
                }
            });
        }

        /// <summary>
        /// Show or hide string with explanation.
        /// </summary>
        /// <param name="sender">ExplanationService.</param>
        /// <param name="e">PropertyChangedEventArgs.</param>
        /// <date>23.05.2022.</date>
        private void Explanation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Explanation = explanationService.ExplanationStr;
        }
    }
}
