using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Partners;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Partner
{
    public class SearchPartnerIntoDatabaseByKey : AbstractStage
    {
        private readonly IPartnerRepository partnerRepository;
        private string[] keys;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPartnerIntoDatabaseByKey"/> class.
        /// </summary>
        /// <param name="keys">Tax number, VAT number and e-mail to search partner.</param>
        /// <param name="partnerRepository">Repository to search partner in the database.</param>
        public SearchPartnerIntoDatabaseByKey(IPartnerRepository partnerRepository, params string[] keys)
        {
            this.partnerRepository = partnerRepository;
            this.keys = keys;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if partner wasn't searched; otherwise returns partner.</returns>
        /// <date>20.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            foreach (string key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    var result = await partnerRepository.GetPartnerByKeyAsync(key);

                    if (result != null)
                    {
                        return (Models.PartnerModel)result;
                    }
                }
            }

            return await base.Invoke(request);
        }
    }
}
