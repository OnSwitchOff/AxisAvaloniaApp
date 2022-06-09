using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.UserControls
{
    public class OperationContainer : TemplatedControl
    {
        private ListBox listPages;
        int index = 0;
        public OperationContainer()
        {
            BackToOperationContentCommand = ReactiveCommand.Create(() =>
            {
               PrintContentVisible = false;
            });

            //Pages = new ObservableCollection<Bitmap>();
            ////Pages.Add(new Bitmap(@"C:\Users\serhii.rozniuk\Desktop\woolf.jpg"));
            ////Pages.Add(new Bitmap(@"C:\Users\serhii.rozniuk\Desktop\kak-pozdravyt-s-8-marta.jpg"));
            ////Pages.Add(new Bitmap(@"C:\Users\serhii.rozniuk\Desktop\woolf.jpg"));
            ////PrintContentHideCommand = ReactiveCommand.Create(() =>
            //{
            //    listPages.ScrollIntoView(index);
            //    index++;
            //    //ActivePage = Pages[1];
            //    //PrintContentVisible = false;
            //});

            //ViewCloseCommand = ReactiveCommand.Create(() =>
            //{
            //    PrintContentVisible = true;
            //});

            Printers = new ObservableCollection<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                Printers.Add(printer);
            }

            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
            SelectedPrinter = settings.PrinterName;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            listPages = e.NameScope.Find<ListBox>("ListPages");
        }

        public static readonly StyledProperty<object> TitleContentProperty =
            AvaloniaProperty.Register<OperationContainer, object>(nameof(TitleContent));

        /// <summary>
        /// Gets or sets content with data of title.
        /// </summary>
        /// <date>25.05.2022.</date>
        public object TitleContent
        {
            get => GetValue(TitleContentProperty);
            set => SetValue(TitleContentProperty, value);
        }

        public static readonly StyledProperty<IReactiveCommand> ViewCloseCommandProperty =
            AvaloniaProperty.Register<OperationContainer, IReactiveCommand>(nameof(ViewCloseCommand));

        /// <summary>
        /// Gets or sets a command that invoked if button "X" is pressed.
        /// </summary>
        /// <date>25.05.2022.</date>
        public IReactiveCommand ViewCloseCommand
        {
            get => GetValue(ViewCloseCommandProperty);
            set => SetValue(ViewCloseCommandProperty, value);
        }

        public static readonly StyledProperty<object> WorkContentProperty =
           AvaloniaProperty.Register<OperationContainer, object>(nameof(WorkContent));

        /// <summary>
        /// Gets or sets content with data of operation.
        /// </summary>
        /// <date>25.05.2022.</date>
        public object WorkContent
        {
            get => GetValue(WorkContentProperty);
            set => SetValue(WorkContentProperty, value);
        }

        public static readonly StyledProperty<object> AdditionalContentProperty =
           AvaloniaProperty.Register<OperationContainer, object>(nameof(AdditionalContent));

        /// <summary>
        /// Gets or sets content with additional data.
        /// </summary>
        /// <date>25.05.2022.</date>
        public object AdditionalContent
        {
            get => GetValue(AdditionalContentProperty);
            set => SetValue(AdditionalContentProperty, value);
        }

        public static readonly StyledProperty<bool> PrintContentVisibleProperty =
            AvaloniaProperty.Register<OperationContainer, bool>(nameof(PrintContentVisible), false);

        /// <summary>
        /// Gets or sets a value indicating whether content with print data is visible.
        /// </summary>
        /// <date>25.05.2022.</date>
        public bool PrintContentVisible
        {
            get => GetValue(PrintContentVisibleProperty);
            set => SetValue(PrintContentVisibleProperty, value);
        }

        public static readonly StyledProperty<IReactiveCommand> BackToOperationContentCommandProperty =
            AvaloniaProperty.Register<OperationContainer, IReactiveCommand>(nameof(BackToOperationContentCommand));

        /// <summary>
        /// Gets or sets a command that invoked if button "X" of the print content is pressed.
        /// </summary>
        /// <date>25.05.2022.</date>
        private IReactiveCommand BackToOperationContentCommand
        {
            get => GetValue(BackToOperationContentCommandProperty);
            set => SetValue(BackToOperationContentCommandProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<Bitmap>> PagesProperty =
            AvaloniaProperty.Register<OperationContainer, ObservableCollection<Bitmap>>(nameof(Pages));

        /// <summary>
        /// Gets or sets list with pages to print.
        /// </summary>
        /// <date>25.05.2022.</date>
        public ObservableCollection<Bitmap> Pages
        {
            get => GetValue(PagesProperty);
            set => SetValue(PagesProperty, value);
        }

        public static readonly StyledProperty<Bitmap> ActivePageProperty =
            AvaloniaProperty.Register<OperationContainer, Bitmap>(nameof(ActivePage));

        /// <summary>
        /// Gets or sets list with pages to print.
        /// </summary>
        /// <date>25.05.2022.</date>
        public Bitmap ActivePage
        {
            get => GetValue(ActivePageProperty);
            set => SetValue(ActivePageProperty, value);
        }

        public static readonly StyledProperty<IReactiveCommand> PrintCommandProperty =
            AvaloniaProperty.Register<OperationContainer, IReactiveCommand>(nameof(PrintCommand));

        /// <summary>
        /// Gets or sets a command to print pages.
        /// </summary>
        /// <date>25.05.2022.</date>
        private IReactiveCommand PrintCommand
        {
            get => GetValue(PrintCommandProperty);
            set => SetValue(PrintCommandProperty, value);
        }

        public static readonly StyledProperty<double> CountOfCopiesProperty =
           AvaloniaProperty.Register<OperationContainer, double>(nameof(CountOfCopies), 1);

        /// <summary>
        /// Gets or sets count of copies to print.
        /// </summary>
        /// <date>25.05.2022.</date>
        public double CountOfCopies
        {
            get => GetValue(CountOfCopiesProperty);
            set => SetValue(CountOfCopiesProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<string>> PrintersProperty =
           AvaloniaProperty.Register<OperationContainer, ObservableCollection<string>>(nameof(Printers));

        /// <summary>
        /// Gets or sets list with enabled printers.
        /// </summary>
        /// <date>08.06.2022.</date>
        public ObservableCollection<string> Printers
        {
            get => GetValue(PrintersProperty);
            set => SetValue(PrintersProperty, value);
        }

        public static readonly StyledProperty<string> SelectedPrinterProperty =
           AvaloniaProperty.Register<OperationContainer, string>(nameof(SelectedPrinter));

        /// <summary>
        /// Gets or sets printer is selected by user.
        /// </summary>
        /// <date>08.06.2022.</date>
        public string SelectedPrinter
        {
            get => GetValue(SelectedPrinterProperty);
            set => SetValue(SelectedPrinterProperty, value);
        }

        //public static readonly StyledProperty<object> PrintContentProperty =
        //   AvaloniaProperty.Register<OperationContainer, object>(nameof(PrintContent));

        ///// <summary>
        ///// Gets or sets content with print data.
        ///// </summary>
        ///// <date>25.05.2022.</date>
        //public object PrintContent
        //{
        //    get => GetValue(PrintContentProperty);
        //    set => SetValue(PrintContentProperty, value);
        //}
    }
}
