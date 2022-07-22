using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Item
{
    internal class ItemMeasureIsNotEmpty : AbstractStage
    {
        private string itemMeasure;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMeasureIsNotEmpty"/> class.
        /// </summary>
        /// <param name="itemMeasure">Measure of item.</param>
        public ItemMeasureIsNotEmpty(string itemMeasure)
        {
            this.itemMeasure = itemMeasure;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>04.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (string.IsNullOrEmpty(itemMeasure))
            {
                await loggerService.ShowDialog("msgItemMeasureIsEmpty", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
