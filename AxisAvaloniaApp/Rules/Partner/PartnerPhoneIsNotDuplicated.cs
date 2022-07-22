using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using DataBase.Repositories.Partners;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Partner
{
    public class PartnerPhoneIsNotDuplicated : AbstractStage
    {
        private readonly IPartnerRepository partnerRepository;
        private PartnerModel partner;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerPhoneIsNotDuplicated"/> class.
        /// </summary>
        /// <param name="partner">Data of partner.</param>
        public PartnerPhoneIsNotDuplicated(PartnerModel partner)
        {
            partnerRepository = Splat.Locator.Current.GetRequiredService<IPartnerRepository>();
            this.partner = partner;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>06.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (await partnerRepository.PartnerPhoneIsDuplicatedAsync(partner.Phone, partner.Id))
            {
                await loggerService.ShowDialog("msgPartnerPhoneIsDuplicated", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
