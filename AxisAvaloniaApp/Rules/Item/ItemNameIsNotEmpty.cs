using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Item
{
    public class ItemNameIsNotEmpty : AbstractStage
    {
        private string itemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNameIsNotEmpty"/> class.
        /// </summary>
        /// <param name="itemName">Name of item.</param>
        public ItemNameIsNotEmpty(string itemName)
        {
            this.itemName = itemName;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>04.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                await loggerService.ShowDialog("msgItemNameIsEmpty", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                return Task.FromResult<object>(-1);
            }

            return base.Invoke(request);
        }
    }
}
