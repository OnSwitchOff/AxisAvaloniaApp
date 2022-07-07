using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Partners;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Partner
{
    public class SavePartner : AbstractStage
    {
        private readonly IPartnerRepository partnerRepository;
        private PartnerModel partner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePartner"/> class.
        /// </summary>
        /// <param name="partner">Data of item.</param>
        public SavePartner(PartnerModel partner)
        {
            partnerRepository = Splat.Locator.Current.GetRequiredService<IPartnerRepository>();
            this.partner = partner;
        }

        /// <summary>
        /// Gets value indicating whether the partner is new.
        /// </summary>
        /// <date>06.07.2022.</date>
        public bool IsNewPartner { get; private set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>06.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            IsNewPartner = partner.Id == 0;
            bool isSuccess;
            switch (partner.Id)
            {
                case 0:
                    partner.Id = await partnerRepository.AddPartnerAsync((DataBase.Entities.Partners.Partner)partner);
                    isSuccess = partner.Id > 0;
                    break;
                default:
                    isSuccess = await partnerRepository.UpdatePartnerAsync((DataBase.Entities.Partners.Partner)partner);
                    break;
            }

            if (isSuccess)
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during writing/updating the partner data in the database!", nameof(SavePartner.Invoke));
                await loggerService.ShowDialog("msgErrorDuringSavingOrUpdatingPartner", "strWarning", UserControls.MessageBox.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
