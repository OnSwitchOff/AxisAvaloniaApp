using ReactiveUI;

namespace AxisAvaloniaApp.Services.Explanation
{
    public interface IExplanationService : IReactiveObject
    {
        /// <summary>
        /// Gets or sets string with explanation data.
        /// </summary>
        /// <date>25.05.2022.</date>
        string ExplanationStr { get; set; }
    }
}
