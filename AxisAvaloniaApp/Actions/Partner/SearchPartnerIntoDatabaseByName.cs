using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Partners;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Partner
{
    public class SearchPartnerIntoDatabaseByName : AbstractStage
    {
        private readonly IPartnerRepository partnerRepository;
        private string partnerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPartnerIntoDatabaseByName"/> class.
        /// </summary>
        /// <param name="partnerName">Name to search partner.</param>
        /// <param name="partnerRepository">Repository to search partner in the database.</param>
        public SearchPartnerIntoDatabaseByName(IPartnerRepository partnerRepository, string partnerName)
        {
            this.partnerRepository = partnerRepository;
            this.partnerName = partnerName;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if partner wasn't searched; otherwise returns partner.</returns>
        /// <date>20.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (!string.IsNullOrEmpty(partnerName))
            {
                var result = await partnerRepository.GetPartnerByNameAsync(partnerName);

                if (result != null)
                {
                    return (Models.PartnerModel)result;
                }
            }

            return await base.Invoke(request);
        }
    }
}
