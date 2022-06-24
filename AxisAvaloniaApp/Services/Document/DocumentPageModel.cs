using ReactiveUI;

namespace AxisAvaloniaApp.Services.Document
{
    /// <summary>
    /// Describes properties of page of document.
    /// </summary>
    public class DocumentPageModel : ReactiveObject
    {
        private double leftMargin;
        private double topMargin;
        private double rightMargin;
        private double bottomMargin;
        private Microinvest.PDFCreator.Enums.EPageOrientations pageOrientation;
        private Microinvest.PDFCreator.Enums.EPageFormats pageFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentPageModel"/> class.
        /// </summary>
        public DocumentPageModel()
        {
            this.LeftMargin = 0.5;
            this.TopMargin = 0;
            this.RightMargin = 0;
            this.BottomMargin = 0;

            this.PageOrientation = Microinvest.PDFCreator.Enums.EPageOrientations.Portrait;
            this.PageFormat = Microinvest.PDFCreator.Enums.EPageFormats.A4;
        }

        /// <summary>
        /// Gets or sets left indent.
        /// </summary>
        /// <date>18.03.2022.</date>
        public double LeftMargin
        {
            get => leftMargin; 
            set => this.RaiseAndSetIfChanged(ref leftMargin, value);
        }

        /// <summary>
        /// Gets or sets top indent.
        /// </summary>
        /// <date>18.03.2022.</date>
        public double TopMargin
        { 
            get => topMargin; 
            set => this.RaiseAndSetIfChanged(ref topMargin, value);
        }

        /// <summary>
        /// Gets or sets right indent.
        /// </summary>
        /// <date>18.03.2022.</date>
        public double RightMargin 
        { 
            get => rightMargin; 
            set => this.RaiseAndSetIfChanged(ref rightMargin, value); 
        }

        /// <summary>
        /// Gets or sets bottom indent.
        /// </summary>
        /// <date>18.03.2022.</date>
        public double BottomMargin 
        { 
            get => bottomMargin; 
            set => this.RaiseAndSetIfChanged(ref bottomMargin, value); 
        }

        /// <summary>
        /// Gets or sets page orientation.
        /// </summary>
        /// <date>18.03.2022.</date>
        public Microinvest.PDFCreator.Enums.EPageOrientations PageOrientation
        {
            get => pageOrientation; 
            set => this.RaiseAndSetIfChanged(ref pageOrientation, value);
        }

        /// <summary>
        /// Gets or sets page format.
        /// </summary>
        /// <date>18.03.2022.</date>
        public Microinvest.PDFCreator.Enums.EPageFormats PageFormat 
        { 
            get => pageFormat; 
            set => this.RaiseAndSetIfChanged(ref pageFormat, value); 
        }
    }
}
