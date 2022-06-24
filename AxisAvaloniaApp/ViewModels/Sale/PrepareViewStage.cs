using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class PrepareViewStage : SaleOperationStage
    {
        private Action prepareViewAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareViewStage"/> class.
        /// </summary>
        /// <param name="prepareViewAction">Action to prepare view for new sale.</param>
        public PrepareViewStage(Action prepareViewAction)
        {
            this.prepareViewAction = prepareViewAction;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>23.06.2022.</date>
        public override Task<object> Invoke(object request)
        {
            prepareViewAction.Invoke();

            return base.Invoke(request);
        }
    }
}
