using AxisAvaloniaApp.Rules;
using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Common
{
    public class PrepareView : AbstractStage
    {
        private Action prepareViewAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareView"/> class.
        /// </summary>
        /// <param name="prepareViewAction">Action to prepare view for new sale.</param>
        public PrepareView(Action prepareViewAction)
        {
            this.prepareViewAction = prepareViewAction;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>23.06.2022.</date>
        public override async Task<object> Invoke(object request)
        {
            prepareViewAction.Invoke();

            return await base.Invoke(request);
        }
    }
}
