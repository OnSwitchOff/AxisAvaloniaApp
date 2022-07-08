using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Partners;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Partner
{
    public class FixNullPartnersGroups : AbstractStage
    {
        private readonly IPartnerRepository partnerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixNullPartnersGroups"/> class.
        /// </summary>
        /// <param name="partnerRepository">Repository to update data in the database.</param>
        public FixNullPartnersGroups(IPartnerRepository partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step.</returns>
        /// <date>08.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            await partnerRepository.SetDefaultGroup();
            return await base.Invoke(request);
        }
    }
}
