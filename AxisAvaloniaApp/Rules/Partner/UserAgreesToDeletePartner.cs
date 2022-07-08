using AxisAvaloniaApp.Rules.Common;

namespace AxisAvaloniaApp.Rules.Partner
{
    public class UserAgreesToDeletePartner : UserAgreesToDeleteNomenclature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgreesToDeleteItemsGroup"/> class.
        /// </summary>
        public UserAgreesToDeletePartner() : base("msgDoYouWantDeletePartner")
        {
        }
    }
}
