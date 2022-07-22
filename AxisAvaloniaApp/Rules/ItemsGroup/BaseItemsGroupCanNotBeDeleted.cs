using AxisAvaloniaApp.Models;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.ItemsGroup
{
    public class BaseItemsGroupCanNotBeDeleted : AbstractStage
    {
        private GroupModel group;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseItemsGroupCanNotBeDeleted"/> class.
        /// </summary>
        /// <param name="group">Data of items group.</param>
        public BaseItemsGroupCanNotBeDeleted(GroupModel group)
        {
            this.group = group;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>07.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (group.Id == 0 || group.Id == 1)
            {
                await loggerService.ShowDialog("msgBaseItemGroupCanNotBeDeleted", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
