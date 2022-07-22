using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Partners;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Partner
{
    public class DeletePartner : AbstractStage
    {
        private readonly IPartnerRepository partnerRepository;
        private PartnerModel partner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePartner"/> class.
        /// </summary>
        /// <param name="partner">Data of item.</param>
        public DeletePartner(PartnerModel partner)
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
            if (await partnerRepository.DeletePartnerAsync(partner.Id))
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during deleting the partner data from the database!", nameof(DeletePartner.Invoke));
                await loggerService.ShowDialog("msgErrorDuringDeletingPartner", "strWarning", UserControls.MessageBoxes.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
