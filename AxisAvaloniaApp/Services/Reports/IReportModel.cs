namespace AxisAvaloniaApp.Services.Reports
{
    public interface IReportModel
    {
        /// <summary>
        /// Gets a value indicating whether the row includes summarized data.
        /// </summary>
        /// <date>07.06.2022.</date>
        bool IsTotalRow { get; }     
    }
}
