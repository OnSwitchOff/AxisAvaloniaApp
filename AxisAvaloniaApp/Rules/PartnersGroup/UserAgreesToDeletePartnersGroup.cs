using AxisAvaloniaApp.Rules.Common;

namespace AxisAvaloniaApp.Rules.PartnersGroup
{
    public class UserAgreesToDeletePartnersGroup : UserAgreesToDeleteNomenclature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgreesToDeletePartnersGroup"/> class.
        /// </summary>
        public UserAgreesToDeletePartnersGroup() : base("msgDoYouWantDeletePartnerGroup")
        {
        }
    }
}
