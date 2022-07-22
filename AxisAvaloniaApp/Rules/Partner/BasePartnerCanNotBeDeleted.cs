using AxisAvaloniaApp.Models;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Partner
{
    public class BasePartnerCanNotBeDeleted : AbstractStage
    {
        private PartnerModel partner;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePartnerCanNotBeDeleted"/> class.
        /// </summary>
        /// <param name="partner">Data of partner.</param>
        public BasePartnerCanNotBeDeleted(PartnerModel partner)
        {
            this.partner = partner;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>07.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (partner.Id == 1)
            {
                await loggerService.ShowDialog("msgBasePartnerCanNotBeDeleted", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
