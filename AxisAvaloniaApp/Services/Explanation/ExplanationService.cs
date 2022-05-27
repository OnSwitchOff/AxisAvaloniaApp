using System.ComponentModel;

namespace AxisAvaloniaApp.Services.Explanation
{
    public class ExplanationService : IExplanationService
    {
        private string explanationStr;

        /// <summary>
        /// Gets or sets string with explanation data.
        /// </summary>
        /// <date>25.05.2022.</date>
        public string ExplanationStr
        {
            get => explanationStr;
            set
            {
                this.explanationStr = value;
                this.RaisePropertyChanged(new PropertyChangedEventArgs(nameof(ExplanationStr)));
            }
        }

        /// <summary>
        /// PropertyChanged event.
        /// </summary>
        /// <date>25.05.2022.</date>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// PropertyChanging event.
        /// </summary>
        /// <date>25.05.2022.</date>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// Raise PropertyChanged event.
        /// </summary>
        /// <param name="args">PropertyChangedEventArgs.</param>
        /// <date>25.05.2022.</date>
        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Raise PropertyChanging event.
        /// </summary>
        /// <param name="args">PropertyChangingEventArgs.</param>
        /// <date>25.05.2022.</date>
        public void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            PropertyChanging?.Invoke(this, args);
        }
    }
}
