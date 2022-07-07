using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.ItemsGroup
{
    public class ItemsGroupNameIsNotEmpty : AbstractStage
    {
        private string itemsGroupName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsGroupNameIsNotEmpty"/> class.
        /// </summary>
        /// <param name="itemsGroupName">Name of items group.</param>
        public ItemsGroupNameIsNotEmpty(string itemsGroupName)
        {
            this.itemsGroupName = itemsGroupName;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>04.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (string.IsNullOrEmpty(itemsGroupName))
            {
                await loggerService.ShowDialog("msgGroupNameIsEmpty", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
