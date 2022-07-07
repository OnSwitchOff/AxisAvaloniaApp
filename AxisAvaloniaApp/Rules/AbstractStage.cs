using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Logger;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules
{
    public abstract class AbstractStage : IStage
    {
        protected readonly ILoggerService loggerService;
        private IStage nextStage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractStage"/> class.
        /// </summary>
        public AbstractStage()
        {
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();
        }

        /// <summary>
        /// Sets next stage to finalize operation of sale.
        /// </summary>
        /// <param name="nextStage">Next stage.</param>
        /// <returns>Returns next stage to finalize operation of sale.</returns>
        /// <date>23.06.2022.</date>
        public IStage SetNext(IStage nextStage)
        {
            this.nextStage = nextStage;

            return nextStage;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if it exists; otherwise returns "1".</returns>
        /// <date>23.06.2022.</date>
        public virtual async Task<object> Invoke(object request)
        {
            if (this.nextStage != null)
            {
                return await this.nextStage.Invoke(request);
            }
            else
            {
                return await Task.FromResult<object>(1);
            }
        }
    }
}
