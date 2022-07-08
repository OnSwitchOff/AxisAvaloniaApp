using AxisAvaloniaApp.Rules.Common;

namespace AxisAvaloniaApp.Rules.Item
{
    public class UserAgreesToDeleteItem : UserAgreesToDeleteNomenclature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgreesToDeleteItem"/> class.
        /// </summary>
        public UserAgreesToDeleteItem() : base("msgDoYouWantDeleteItem")
        {
        }
    }
}
