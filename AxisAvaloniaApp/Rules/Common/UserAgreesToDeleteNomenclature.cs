using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Common
{
    public abstract class UserAgreesToDeleteNomenclature : AbstractStage
    {
        private string keyMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgreesToDeleteNomenclature"/> class.
        /// </summary>
        /// <param name="keyMessage">Key to get localized message to get confirmation from user.</param>
        public UserAgreesToDeleteNomenclature(string keyMessage)
        {
            this.keyMessage = keyMessage;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>07.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (await loggerService.ShowDialog(keyMessage, "strAttention", UserControls.MessageBox.EButtonIcons.Info, UserControls.MessageBox.EButtons.YesNo) == UserControls.MessageBox.EButtonResults.No)
            {
                return await Task.FromResult<object>(-1);
            }
            else
            {
                return await base.Invoke(request);
            }
        }
    }
}
