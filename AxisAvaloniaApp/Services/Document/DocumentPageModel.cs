namespace AxisAvaloniaApp.Services.Document
{
    /// <summary>
    /// Describes properties of page of document.
    /// </summary>
    public class DocumentPageModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
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
        public double LeftMargin { get; set; }

        /// <summary>
        /// Gets or sets top indent.
        /// </summary>
        /// <date>18.03.2022.</date>
        public double TopMargin { get; set; }

        /// <summary>
        /// Gets or sets right indent.
        /// </summary>
        /// <date>18.03.2022.</date>
        public double RightMargin { get; set; }

        /// <summary>
        /// Gets or sets bottom indent.
        /// </summary>
        /// <date>18.03.2022.</date>
        public double BottomMargin { get; set; }

        /// <summary>
        /// Gets or sets page orientation.
        /// </summary>
        /// <date>18.03.2022.</date>
        public Microinvest.PDFCreator.Enums.EPageOrientations PageOrientation { get; set; }

        /// <summary>
        /// Gets or sets page format.
        /// </summary>
        /// <date>18.03.2022.</date>
        public Microinvest.PDFCreator.Enums.EPageFormats PageFormat { get; set; }
    }
}
