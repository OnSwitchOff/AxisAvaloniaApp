using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media.Imaging;
using AxisAvaloniaApp.Enums;
using Microinvest.PDFCreator.Enums;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.UserControls
{
    public class OperationContainer : TemplatedControl
    {
        private ItemsRepeater listPages;
        private TextBox textBoxPagesToPrint;
        private double basePageWidth;
        private Image selectedPage;
        int index = 0;
        public OperationContainer()
        {
            BackToOperationContentCommand = ReactiveCommand.Create(() =>
            {
               PrintContentVisible = false;
            });

            Pages = new ObservableCollection<Bitmap>();
 
            //PrintContentHideCommand = ReactiveCommand.Create(() =>
            //{
            //    listPages.ScrollIntoView(index);
            //    index++;
            //    //ActivePage = Pages[1];
            //    //PrintContentVisible = false;
            //});

            Printers = new ObservableCollection<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                Printers.Add(printer);
            }
            SelectedPrinter = (new System.Drawing.Printing.PrinterSettings()).PrinterName;

            PrintingModes = new ObservableCollection<PrintModel<EPrintingModes>>()
            {
                new PrintModel<EPrintingModes>(EPrintingModes.AllPages, "/Assets/Icons/papers_All.png", "strPrintAllPages"),
                new PrintModel<EPrintingModes>(EPrintingModes.CurrentPage, "/Assets/Icons/papers_Current.png", "strPrintCurrentPage"),
                new PrintModel<EPrintingModes>(EPrintingModes.CustomPrint, "/Assets/Icons/papers_Custom.png", "strPrintCustom"),
                new PrintModel<EPrintingModes>(EPrintingModes.OddPages, "/Assets/Icons/papers_Odd.png", "strPrintOddPages"),
                new PrintModel<EPrintingModes>(EPrintingModes.EvenPages, "/Assets/Icons/papers_Even.png", "strPrintEvenPages"),
            };
            SelectedPrintingMode = PrintingModes[0];

            SortingModes = new ObservableCollection<PrintModel<ESortingPages>>()
            {
                new PrintModel<ESortingPages>(ESortingPages.Collated, "/Assets/Icons/pages_Collated.png", "strCollated", "1,2,3   1,2,3   1,2,3"),
                new PrintModel<ESortingPages>(ESortingPages.Collated, "/Assets/Icons/pages_Uncollated.png", "strUncollated", "1,1,1   2,2,2   3,3,3"),
            };
            SelectedSortingMode = SortingModes[0];

            Orientations = new ObservableCollection<PrintModel<EPageOrientations>>()
            {
                new PrintModel<EPageOrientations>(EPageOrientations.Portrait, "/Assets/Icons/orientationPortrait.jpg", "strPortraitOrientation"),
                new PrintModel<EPageOrientations>(EPageOrientations.Landscape, "/Assets/Icons/orientationLandscape.jpg", "strLandscapeOrientation"),
            };
            SelectedOrientation = Orientations[1];

            PageFormats = new ObservableCollection<PrintModel<EPageFormats>>()
            {
                new PrintModel<EPageFormats>(EPageFormats.Letter, "/Assets/Icons/pageFormat.png", EPageFormats.Letter.ToString(), "21,59 cm x 27,94 cm"),
                new PrintModel<EPageFormats>(EPageFormats.Legal, "/Assets/Icons/pageFormat.png", EPageFormats.Legal.ToString(), "21,59 cm x 35,56 cm"),
                new PrintModel<EPageFormats>(EPageFormats.A5, "/Assets/Icons/pageFormat.png", EPageFormats.A5.ToString(), "14,8 cm x 21 cm"),
                new PrintModel<EPageFormats>(EPageFormats.B5, "/Assets/Icons/pageFormat.png", EPageFormats.B5.ToString(), "18,2 cm x 25,7 cm"),
                new PrintModel<EPageFormats>(EPageFormats.A4, "/Assets/Icons/pageFormat.png", EPageFormats.A4.ToString(), "21 cm x 29.7 cm"),
                new PrintModel<EPageFormats>(EPageFormats.A3, "/Assets/Icons/pageFormat.png", EPageFormats.A3.ToString(), "29,7 cm x 42 cm"),
            };
            SelectedPageFormat = PageFormats[4];

            basePageWidth = PageWidth;
        }        

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            listPages = e.NameScope.Find<ItemsRepeater>("ListPages");
            listPages.PointerPressed += ItemsRepeater_PointerPressed;
            textBoxPagesToPrint = e.NameScope.Find<TextBox>("TextBoxPagesToPrint");
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            listPages.PointerPressed -= ItemsRepeater_PointerPressed;
            base.OnDetachedFromVisualTree(e);
        }


        private void ItemsRepeater_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (selectedPage != null && selectedPage.Parent is Border b)
            {
                b.Background = Avalonia.Media.Brushes.White;
            }

            switch (e.Source)
            {
                case Border border:
                    if (border.Child is Image borderImage)
                    {
                        selectedPage = borderImage;
                    }
                    break;
                case Image image:
                    selectedPage = image;
                    break;
            }

            if (selectedPage != null && selectedPage.DataContext is Bitmap bitmap)
            {
                int index = Pages.IndexOf(bitmap);
                ActivePage = Pages[index];
            }
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

        public static readonly StyledProperty<ObservableCollection<PrintModel<EPrintingModes>>> PrintingModesProperty =
           AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<EPrintingModes>>>(nameof(PrintingModes));

        /// <summary>
        /// Gets or sets list with modes to print.
        /// </summary>
        /// <date>08.06.2022.</date>
        public ObservableCollection<PrintModel<EPrintingModes>> PrintingModes
        {
            get => GetValue(PrintingModesProperty);
            set => SetValue(PrintingModesProperty, value);
        }

        public static readonly StyledProperty<PrintModel<EPrintingModes>> SelectedPrintingModeProperty =
           AvaloniaProperty.Register<OperationContainer, PrintModel<EPrintingModes>>(nameof(SelectedPrintingMode));

        /// <summary>
        /// Gets or sets printing mode is selected by user.
        /// </summary>
        /// <date>08.06.2022.</date>
        public PrintModel<EPrintingModes> SelectedPrintingMode
        {
            get => GetValue(SelectedPrintingModeProperty);
            set => SetValue(SelectedPrintingModeProperty, value);
        }

        public static readonly StyledProperty<string> PagesToPrintProperty =
           AvaloniaProperty.Register<OperationContainer, string>(nameof(PagesToPrint));

        /// <summary>
        /// Gets or sets pages to print.
        /// </summary>
        /// <date>08.06.2022.</date>
        public string PagesToPrint
        {
            get => GetValue(PagesToPrintProperty);
            set => SetValue(PagesToPrintProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<PrintModel<ESortingPages>>> SortingModesProperty =
           AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<ESortingPages>>>(nameof(SortingModes));

        /// <summary>
        /// Gets or sets list with modes to sort pages.
        /// </summary>
        /// <date>09.06.2022.</date>
        public ObservableCollection<PrintModel<ESortingPages>> SortingModes
        {
            get => GetValue(SortingModesProperty);
            set => SetValue(SortingModesProperty, value);
        }

        public static readonly StyledProperty<PrintModel<ESortingPages>> SelectedSortingModeProperty =
           AvaloniaProperty.Register<OperationContainer, PrintModel<ESortingPages>>(nameof(SelectedSortingMode));

        /// <summary>
        /// Gets or sets sorting mode is selected by user.
        /// </summary>
        /// <date>09.06.2022.</date>
        public PrintModel<ESortingPages> SelectedSortingMode
        {
            get => GetValue(SelectedSortingModeProperty);
            set => SetValue(SelectedSortingModeProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<PrintModel<EPageOrientations>>> OrientationsProperty =
           AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<EPageOrientations>>>(nameof(Orientations));

        /// <summary>
        /// Gets or sets list with orientations of page.
        /// </summary>
        /// <date>09.06.2022.</date>
        public ObservableCollection<PrintModel<EPageOrientations>> Orientations
        {
            get => GetValue(OrientationsProperty);
            set => SetValue(OrientationsProperty, value);
        }

        public static readonly StyledProperty<PrintModel<EPageOrientations>> SelectedOrientationProperty =
           AvaloniaProperty.Register<OperationContainer, PrintModel<EPageOrientations>>(nameof(SelectedOrientation));

        /// <summary>
        /// Gets or sets orientation of page is selected by user.
        /// </summary>
        /// <date>09.06.2022.</date>
        public PrintModel<EPageOrientations> SelectedOrientation
        {
            get => GetValue(SelectedOrientationProperty);
            set => SetValue(SelectedOrientationProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<PrintModel<EPageFormats>>> PageFormatsProperty =
          AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<EPageFormats>>>(nameof(PageFormats));

        /// <summary>
        /// Gets or sets list with formats of page.
        /// </summary>
        /// <date>09.06.2022.</date>
        public ObservableCollection<PrintModel<EPageFormats>> PageFormats
        {
            get => GetValue(PageFormatsProperty);
            set => SetValue(PageFormatsProperty, value);
        }

        public static readonly StyledProperty<PrintModel<EPageFormats>> SelectedPageFormatProperty =
           AvaloniaProperty.Register<OperationContainer, PrintModel<EPageFormats>>(nameof(SelectedPageFormat));

        /// <summary>
        /// Gets or sets format of page is selected by user.
        /// </summary>
        /// <date>09.06.2022.</date>
        public PrintModel<EPageFormats> SelectedPageFormat
        {
            get => GetValue(SelectedPageFormatProperty);
            set => SetValue(SelectedPageFormatProperty, value);
        }

        public static readonly StyledProperty<double> ScaleProperty =
           AvaloniaProperty.Register<OperationContainer, double>(nameof(Scale), 100);

        /// <summary>
        /// Gets or sets value of scale of the pages.
        /// </summary>
        /// <date>09.06.2022.</date>
        public double Scale
        {
            get => GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly StyledProperty<double> PageWidthProperty =
           AvaloniaProperty.Register<OperationContainer, double>(nameof(PageWidth), 690);

        /// <summary>
        /// Gets or sets width of the page.
        /// </summary>
        /// <date>09.06.2022.</date>
        public double PageWidth
        {
            get => GetValue(PageWidthProperty);
            set => SetValue(PageWidthProperty, value);
        }


        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            switch (change.Property.Name)
            {
                case nameof(SelectedPrintingMode):
                    if (SelectedPrintingMode.Type == EPrintingModes.CustomPrint && !textBoxPagesToPrint.IsFocused)
                    {
                        System.Threading.Tasks.Task.Run(() =>
                        {
                            System.Threading.Thread.Sleep(100);
                            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => textBoxPagesToPrint.Focus());

                        });
                    }
                    break;
                case nameof(Scale):
                    PageWidth = basePageWidth * (Scale / 100);
                    break;
            }
        }
    }

    public class PrintModel<T> where T : Enum
    {
        public PrintModel(T type, string imagePath, string localizeKey)
        {
            Type = type;
            ImagePath = imagePath;
            LocalizeKey = localizeKey;
            AdditionalData = string.Empty;
        }

        public PrintModel(T type, string imagePath, string localizeKey, string additionalData)
        {
            Type = type;
            ImagePath = imagePath;
            LocalizeKey = localizeKey;
            AdditionalData = additionalData;
        }

        public T Type { get; }
        public string ImagePath { get; }
        public string LocalizeKey { get; }
        public string AdditionalData { get; }
    }
}
