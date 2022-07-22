using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.Translation;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Partner
{
    public class CreateNewPartner : AbstractStage
    {
        private readonly ITranslationService translationService;
        private Models.PartnerModel partner;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNewPartner"/> class.
        /// </summary>
        /// <param name="partner">Data of new partner.</param>
        public CreateNewPartner(Models.PartnerModel partner)
        {
            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.partner = partner;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns new ItemModel object if even one of partner field is not empty; otherwise returns next method to invoke.</returns>
        /// <date>20.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (string.IsNullOrEmpty(partner.Name) && string.IsNullOrEmpty(partner.TaxNumber) && string.IsNullOrEmpty(partner.VATNumber) && string.IsNullOrEmpty(partner.Email))
            {
                return await base.Invoke(request);
            }
            else
            {
                if (string.IsNullOrEmpty(partner.Name))
                {
                    partner.Name = translationService.Localize("strUnidentifiedImportedPartner");
                }

                return await Task.FromResult(partner);
            }
        }
    }
}
