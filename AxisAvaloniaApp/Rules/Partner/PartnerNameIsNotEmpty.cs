using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Partner
{
    public class PartnerNameIsNotEmpty : AbstractStage
    {
        private string partnerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerNameIsNotEmpty"/> class.
        /// </summary>
        /// <param name="partnerName">Name of partner.</param>
        public PartnerNameIsNotEmpty(string partnerName)
        {
            this.partnerName = partnerName;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>06.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (string.IsNullOrEmpty(partnerName))
            {
                await loggerService.ShowDialog("msgPartnerNameIsEmpty", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
