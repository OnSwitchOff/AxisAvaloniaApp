using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Logger;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public abstract class SaleOperationStage : ISaleStage
    {
        protected readonly ILoggerService loggerService;
        private ISaleStage nextStage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleOperationStage"/> class.
        /// </summary>
        public SaleOperationStage()
        {
            loggerService = Splat.Locator.Current.GetRequiredService<ILoggerService>();
        }

        /// <summary>
        /// Sets next stage to finalize operation of sale.
        /// </summary>
        /// <param name="nextStage">Next stage.</param>
        /// <returns>Returns next stage to finalize operation of sale.</returns>
        /// <date>23.06.2022.</date>
        public ISaleStage SetNext(ISaleStage nextStage)
        {
            this.nextStage = nextStage;

            return nextStage;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>23.06.2022.</date>
        public virtual Task<object> Invoke(object request)
        {
            if (this.nextStage != null)
            {
                return this.nextStage.Invoke(request);
            }
            else
            {
                return Task.FromResult<object>(-1);
            }
        }

        
    }
}
