using AxisAvaloniaApp.Rules.Common;

namespace AxisAvaloniaApp.Rules.ItemsGroup
{
    public class UserAgreesToDeleteItemsGroup : UserAgreesToDeleteNomenclature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgreesToDeleteItemsGroup"/> class.
        /// </summary>
        public UserAgreesToDeleteItemsGroup() : base("msgDoYouWantDeleteItemGroup")
        {
        }
    }
}
