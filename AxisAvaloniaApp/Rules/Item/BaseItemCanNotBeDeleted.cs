using AxisAvaloniaApp.Models;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Item
{
    public class BaseItemCanNotBeDeleted : AbstractStage
    {
        private ItemModel item;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseItemCanNotBeDeleted"/> class.
        /// </summary>
        /// <param name="item">Data of item.</param>
        public BaseItemCanNotBeDeleted(ItemModel item)
        {
            this.item = item;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>07.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (item.Id == 1)
            {
                await loggerService.ShowDialog("msgBaseItemCanNotBeDeleted", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
