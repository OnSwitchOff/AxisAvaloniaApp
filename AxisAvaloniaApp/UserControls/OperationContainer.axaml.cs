using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Document;
using AxisAvaloniaApp.Services.Printing;
using AxisAvaloniaApp.Services.Validation;
using Microinvest.PDFCreator.Enums;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.UserControls
{
    public class OperationContainer : TemplatedControl
    {
        private readonly IPrintService printService;
        private readonly IValidationService validationService;
        private ItemsRepeater listPages;
        private TextBox textBoxPagesToPrint;
        private double basePageWidth;
        private Image selectedPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationContainer"/> class.
        /// </summary>
        public OperationContainer()
        {
            printService = Splat.Locator.Current.GetRequiredService<IPrintService>();
            validationService = Splat.Locator.Current.GetRequiredService<IValidationService>();

            BackToOperationContentCommand = ReactiveCommand.Create(() =>
            {
               PrintContentVisible = false;
            });

            PrintCommand = ReactiveCommand.Create(() => PrintDocumentAsync(
                SelectedPrintingMode.Type, 
                PagesToPrint, 
                Pages, 
                ActivePage != null ? AvaloniaPages.IndexOf(ActivePage) : -1,
                SelectedPrinter));

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filters.Add(new FileDialogFilter() { Name = "PDF", Extensions = { "pdf" } });
            ExportCommand = ReactiveCommand.Create(async () =>
            {
                string? filePath = await dialog.ShowAsync(App.MainWindow);

                if (!string.IsNullOrEmpty(filePath))
                {
                    DocumentService.SaveDocument(filePath);
                }
            });

            Printers = new ObservableCollection<string>();
            foreach (string printer in printService.GetPrinters())
            {
                Printers.Add(printer);
            }
            SelectedPrinter = (new System.Drawing.Printing.PrinterSettings()).PrinterName;
            SelectedPrintingMode = PrintingModes[0];
            SelectedSortingMode = SortingModes[0];

            SelectedDocumentVersion = DocumentVersions[0];

            basePageWidth = PageWidth;
        }
        
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            listPages = e.NameScope.Find<ItemsRepeater>("ListPages");
            listPages.PointerPressed += ItemsRepeater_PointerPressed;
            textBoxPagesToPrint = e.NameScope.Find<TextBox>("TextBoxPagesToPrint");
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            if (listPages != null)
            {
                listPages.PointerPressed -= ItemsRepeater_PointerPressed;
                listPages.PointerPressed += ItemsRepeater_PointerPressed;
            }
            ConvertImageToAvaloniaImage();
            base.OnAttachedToVisualTree(e);
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
                int index = AvaloniaPages.IndexOf(bitmap);
                ActivePage = AvaloniaPages[index];
            }
        }

        public static readonly StyledProperty<IDocumentService> DocumentServiceProperty =
            AvaloniaProperty.Register<OperationContainer, IDocumentService>(nameof(DocumentService));

        /// <summary>
        /// Gets or sets service to generate documents.
        /// </summary>
        /// <date>24.06.2022.</date>
        public IDocumentService DocumentService
        {
            get => GetValue(DocumentServiceProperty);
            set => SetValue(DocumentServiceProperty, value);
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

        public static readonly StyledProperty<ObservableCollection<System.Drawing.Image>> PagesProperty =
            AvaloniaProperty.Register<OperationContainer, ObservableCollection<System.Drawing.Image>>(nameof(Pages));

        /// <summary>
        /// Gets or sets list with pages to print.
        /// </summary>
        /// <date>24.06.2022.</date>
        public ObservableCollection<System.Drawing.Image> Pages
        {
            get => GetValue(PagesProperty);
            set => SetValue(PagesProperty, value);
        }

        public static readonly StyledProperty<ObservableCollection<Bitmap>> AvaloniaPagesProperty =
            AvaloniaProperty.Register<OperationContainer, ObservableCollection<Bitmap>>(nameof(AvaloniaPages), new ObservableCollection<Bitmap>());

        /// <summary>
        /// Gets or sets list with pages to show user.
        /// </summary>
        /// <date>25.05.2022.</date>
        public ObservableCollection<Bitmap> AvaloniaPages
        {
            get => GetValue(AvaloniaPagesProperty);
            set => SetValue(AvaloniaPagesProperty, value);
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

        public static readonly StyledProperty<IReactiveCommand> ExportCommandProperty =
            AvaloniaProperty.Register<OperationContainer, IReactiveCommand>(nameof(ExportCommand));

        /// <summary>
        /// Gets or sets a command to учзщке pages.
        /// </summary>
        /// <date>08.07.2022.</date>
        private IReactiveCommand ExportCommand
        {
            get => GetValue(ExportCommandProperty);
            set => SetValue(ExportCommandProperty, value);
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
           AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<EPrintingModes>>>(
               nameof(PrintingModes), 
               new ObservableCollection<PrintModel<EPrintingModes>>()
               {
                   new PrintModel<EPrintingModes>(EPrintingModes.AllPages, "/Assets/Icons/papers_All.png", "strPrintAllPages"),
                   new PrintModel<EPrintingModes>(EPrintingModes.CurrentPage, "/Assets/Icons/papers_Current.png", "strPrintCurrentPage"),
                   new PrintModel<EPrintingModes>(EPrintingModes.CustomPrint, "/Assets/Icons/papers_Custom.png", "strPrintCustom"),
                   new PrintModel<EPrintingModes>(EPrintingModes.OddPages, "/Assets/Icons/papers_Odd.png", "strPrintOddPages"),
                   new PrintModel<EPrintingModes>(EPrintingModes.EvenPages, "/Assets/Icons/papers_Even.png", "strPrintEvenPages"),
               });

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
           AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<ESortingPages>>>(
               nameof(SortingModes), 
               new ObservableCollection<PrintModel<ESortingPages>>()
               {
                   new PrintModel<ESortingPages>(ESortingPages.Collated, "/Assets/Icons/pages_Collated.png", "strCollated", "1,2,3   1,2,3   1,2,3"),
                   new PrintModel<ESortingPages>(ESortingPages.Collated, "/Assets/Icons/pages_Uncollated.png", "strUncollated", "1,1,1   2,2,2   3,3,3"),
               });

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
           AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<EPageOrientations>>>(
               nameof(Orientations), 
               new ObservableCollection<PrintModel<EPageOrientations>>()
               {
                   new PrintModel<EPageOrientations>(EPageOrientations.Portrait, "/Assets/Icons/orientationPortrait.jpg", "strPortraitOrientation"),
                   new PrintModel<EPageOrientations>(EPageOrientations.Landscape, "/Assets/Icons/orientationLandscape.jpg", "strLandscapeOrientation"),
               });

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
          AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<EPageFormats>>>(
              nameof(PageFormats), 
              new ObservableCollection<PrintModel<EPageFormats>>()
              {
                  new PrintModel<EPageFormats>(EPageFormats.Letter, "/Assets/Icons/pageFormat.png", EPageFormats.Letter.ToString(), "21,59 cm x 27,94 cm"),
                  new PrintModel<EPageFormats>(EPageFormats.Legal, "/Assets/Icons/pageFormat.png", EPageFormats.Legal.ToString(), "21,59 cm x 35,56 cm"),
                  new PrintModel<EPageFormats>(EPageFormats.A5, "/Assets/Icons/pageFormat.png", EPageFormats.A5.ToString(), "14,8 cm x 21 cm"),
                  new PrintModel<EPageFormats>(EPageFormats.B5, "/Assets/Icons/pageFormat.png", EPageFormats.B5.ToString(), "18,2 cm x 25,7 cm"),
                  new PrintModel<EPageFormats>(EPageFormats.A4, "/Assets/Icons/pageFormat.png", EPageFormats.A4.ToString(), "21 cm x 29.7 cm"),
                  new PrintModel<EPageFormats>(EPageFormats.A3, "/Assets/Icons/pageFormat.png", EPageFormats.A3.ToString(), "29,7 cm x 42 cm"),
              });

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

        public static readonly StyledProperty<ObservableCollection<PrintModel<EDocumentVersionsPrinting>>> DocumentVersionsProperty =
          AvaloniaProperty.Register<OperationContainer, ObservableCollection<PrintModel<EDocumentVersionsPrinting>>>(
              nameof(DocumentVersions), 
              new ObservableCollection<PrintModel<EDocumentVersionsPrinting>>()
              {
                  new PrintModel<EDocumentVersionsPrinting>(EDocumentVersionsPrinting.Original, "/Assets/Icons/pageFormat.png", "strOriginal"),
                  new PrintModel<EDocumentVersionsPrinting>(EDocumentVersionsPrinting.Copy, "/Assets/Icons/pageFormat.png", "strDuplicate"),
                  new PrintModel<EDocumentVersionsPrinting>(EDocumentVersionsPrinting.OriginalAndCopy, "/Assets/Icons/pageFormat.png", "strOriginalAndDuplicate"),
                  new PrintModel<EDocumentVersionsPrinting>(EDocumentVersionsPrinting.OriginalAndTwoCopies, "/Assets/Icons/pageFormat.png", "strOriginalAndTwoDuplicates"),
              });

        /// <summary>
        /// Gets or sets list with versions of document.
        /// </summary>
        /// <date>08.07.2022.</date>
        public ObservableCollection<PrintModel<EDocumentVersionsPrinting>> DocumentVersions
        {
            get => GetValue(DocumentVersionsProperty);
            set => SetValue(DocumentVersionsProperty, value);
        }

        public static readonly StyledProperty<PrintModel<EDocumentVersionsPrinting>> SelectedDocumentVersionProperty =
           AvaloniaProperty.Register<OperationContainer, PrintModel<EDocumentVersionsPrinting>>(nameof(SelectedDocumentVersion));

        /// <summary>
        /// Gets or sets version of document is selected by user.
        /// </summary>
        /// <date>08.07.2022.</date>
        public PrintModel<EDocumentVersionsPrinting> SelectedDocumentVersion
        {
            get => GetValue(SelectedDocumentVersionProperty);
            set => SetValue(SelectedDocumentVersionProperty, value);
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

        public static readonly RoutedEvent<RoutedEventArgs> DocumentWasPrintedEvent =
            RoutedEvent.Register<OperationContainer, RoutedEventArgs>(
                nameof(DocumentWasPrinted),
                RoutingStrategies.Bubble);

        /// <summary>
        /// Occurs when pages were printed or exported.
        /// </summary>
        /// <date>11.07.2022.</date>
        public event EventHandler<RoutedEventArgs> DocumentWasPrinted
        {
            add => AddHandler(DocumentWasPrintedEvent, value);
            remove => RemoveHandler(DocumentWasPrintedEvent, value);
        }

        /// <summary>
        /// Updates dependent property when main property was changed.
        /// </summary>
        /// <typeparam name="T">Type of changed property.</typeparam>
        /// <param name="change">PropertyChangedEventArgs.</param>
        /// <date>09.06.2022.</date>
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            switch (change.Property.Name)
            {
                case nameof(SelectedPrintingMode):
                    if (SelectedPrintingMode.Type == EPrintingModes.CustomPrint && !textBoxPagesToPrint.IsFocused)
                    {
                        Task.Run(() =>
                        {
                            System.Threading.Thread.Sleep(100);
                            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() => textBoxPagesToPrint.Focus());

                        });
                    }
                    break;
                case nameof(Scale):
                    PageWidth = basePageWidth * (Scale / 100);
                    break;
                case nameof(DocumentService):
                    if (DocumentService != null)
                    {
                        SelectedOrientation = Orientations.Where(o => o.Type.Equals(DocumentService.PageParameters.PageOrientation)).FirstOrDefault();
                        SelectedPageFormat = PageFormats.Where(pf => pf.Type.Equals(DocumentService.PageParameters.PageFormat)).FirstOrDefault();
                    }
                    break;
                case nameof(SelectedOrientation):
                case nameof(SelectedDocumentVersion):
                case nameof(SelectedPageFormat):
                    if (DocumentService != null && PrintContentVisible)
                    {
                        DocumentService.PageParameters.PageOrientation = SelectedOrientation.Type;
                        DocumentService.PageParameters.PageFormat = SelectedPageFormat.Type;

                        DocumentService.GenerateDocument(
                            DocumentService.DocumentType,
                            SelectedDocumentVersion.Type, 
                            DocumentService.PaymentType);
                        Pages = DocumentService.ConvertDocumentToImageList().Clone();
                    }
                    break;
                case nameof(PrintContentVisible):
                    break;
                case nameof(Pages):
                    ConvertImageToAvaloniaImage();
                    break;
            }
        }

        /// <summary>
        /// Converts System.Drawing.Image to Avalonia.Media.Imaging.Bitmap.
        /// </summary>
        /// <date>07.07.2022.</date>
        private void ConvertImageToAvaloniaImage()
        {
            if (Pages != null && Pages.Count > 0)
            {
                AvaloniaPages.Clear();

                foreach (System.Drawing.Image page in Pages)
                {
                    using (System.IO.Stream stream = new System.IO.MemoryStream())
                    {
                        page.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        stream.Position = 0;

                        AvaloniaPages.Add(new Bitmap(stream));
                    }
                }
            }
        }

        /// <summary>
        /// Prints pages.
        /// </summary>
        /// <param name="printingMode">Mode to select pages to print.</param>
        /// <param name="pagesRange">Pages that was selected by user to print.</param>
        /// <param name="pagesList">List with pages.</param>
        /// <param name="activePageIndex">Index of the active page.</param>
        /// <param name="selectedPrinter">Name of the selected printer.</param>
        /// <returns>Returns Task to print pages.</returns>
        /// <date>11.07.2022.</date>
        private Task PrintDocumentAsync(EPrintingModes printingMode, string pagesRange, ObservableCollection<System.Drawing.Image> pagesList, int activePageIndex, string selectedPrinter)
        {
            return Task.Run(() =>
            {
                List<System.Drawing.Image> pagesToPrint = new List<System.Drawing.Image>();
                switch (printingMode)
                {
                    case EPrintingModes.AllPages:
                        foreach (System.Drawing.Image image in pagesList)
                        {
                            pagesToPrint.Add(image);
                        }
                        break;
                    case EPrintingModes.CurrentPage:
                        if (activePageIndex > -1 && activePageIndex < pagesList.Count)
                        {
                            pagesToPrint.Add(pagesList[activePageIndex]);
                        }
                        break;
                    case EPrintingModes.CustomPrint:
                        int pageIndex, startIndex, endOfRange;
                        string strToParse = string.Empty;
                        string tmpStrToParse = string.Empty;
                        string strToCheck = string.Empty;
                        List<int> indexes = new List<int>();
                        for (int i = 0; i < pagesRange.Length; i++)
                        {
                            // если текущий символ является цифрой
                            if (validationService.IsDigit(pagesRange[i]))
                            {
                                // добаляем его к предыдущим цифровым символам
                                strToParse += pagesRange[i].ToString();
                            }
                            else
                            {
                                // если строка с цифрами не пустая
                                if (!string.IsNullOrEmpty(strToParse))
                                {
                                    // добавляем запрошенную страницу в список страниц для печати 
                                    pageIndex = int.Parse(strToParse) - 1;
                                    indexes.Add(pageIndex);
                                    if (pageIndex >= 0 && pageIndex < pagesList.Count)
                                    {
                                        pagesToPrint.Add(pagesList[pageIndex]);
                                    }
                                }
                                // очищаем строку с цифрами
                                strToParse = string.Empty;

                                // если текущий символ "-"
                                if (pagesRange[i] == '-')
                                {
                                    // смещаемся на следующий символ
                                    if (i < pagesRange.Length - 1)
                                    {
                                        i++;
                                    }

                                    // смещаемся вперед до тех пор, пока не появится первый реальный символ (не пробел)
                                    while (i < pagesRange.Length && pagesRange[i].Equals(' '))
                                    {
                                        i++;
                                    }

                                    // получаем строку после "-"
                                    tmpStrToParse = pagesRange.Substring(i);
                                    int tmpIndex = 0;
                                    // определяем длину строки с цифрами
                                    while (i < pagesRange.Length && validationService.IsDigit(tmpStrToParse[tmpIndex]))
                                    {
                                        tmpIndex++;
                                        i++;
                                    }

                                    // если определённая строка является числом
                                    if (int.TryParse(tmpStrToParse.Substring(0, tmpIndex), out endOfRange))
                                    {
                                        endOfRange -= 1;
                                        startIndex = endOfRange;
                                        // получаем строку с диапазоном страниц для печати
                                        strToCheck = pagesRange.Substring(0, pagesRange.IndexOf(tmpStrToParse));
                                        // устанавливаем начало диапазона в зависимости от того, это реальный диапазон или просто опечатка
                                        for (int backIndex = strToCheck.Length - 1; backIndex >= 0; backIndex--)
                                        {
                                            if (strToCheck[backIndex].Equals(' ') || strToCheck[backIndex].Equals('-'))
                                            {
                                                continue;
                                            }

                                            if (validationService.IsDigit(strToCheck[backIndex]))
                                            {
                                                startIndex = indexes.Count == 0 ? endOfRange : (indexes[indexes.Count - 1] + 1);
                                            }

                                            break;
                                        }

                                        // добавляем запрошенные страницы в список страниц для печати 
                                        indexes.Add(endOfRange);
                                        for (pageIndex = Math.Min(startIndex, endOfRange); pageIndex <= Math.Max(startIndex, endOfRange); pageIndex++)
                                        {
                                            if (pageIndex >= 0 && pageIndex < pagesList.Count)
                                            {
                                                pagesToPrint.Add(pagesList[pageIndex]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case EPrintingModes.OddPages:
                        for (int i = 0; i < pagesList.Count; i++)
                        {
                            if (i + 1 % 2 != 0)
                            {
                                pagesToPrint.Add(pagesList[i]);
                            }
                        }
                        break;
                    case EPrintingModes.EvenPages:
                        for (int i = 0; i < pagesList.Count; i++)
                        {
                            if (i + 1 % 2 == 0)
                            {
                                pagesToPrint.Add(pagesList[i]);
                            }
                        }
                        break;
                }

                if (pagesToPrint.Count > 0)
                {
                    if (printService.PrintImageList(selectedPrinter, pagesToPrint))
                    {
                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            this.RaiseEvent(
                                new RoutedEventArgs()
                                {
                                    RoutedEvent = DocumentWasPrintedEvent,
                                });
                        });
                    }
                }
            });
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
