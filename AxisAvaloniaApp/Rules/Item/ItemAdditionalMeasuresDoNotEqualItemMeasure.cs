using AxisAvaloniaApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Item
{
    public class ItemMeasuresAreNotDuplicated : AbstractStage
    {
        private ItemModel item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMeasuresAreNotDuplicated"/> class.
        /// </summary>
        /// <param name="item">Data of item.</param>
        public ItemMeasuresAreNotDuplicated(ItemModel item)
        {
            this.item = item;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>04.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (item.Codes.GroupBy(i => i.Measure).Where(c => c.Count() > 1 || c.Key.Equals(item.Measure)).FirstOrDefault() != null)
            {
                await loggerService.ShowDialog("msgDuplicateAdditionalMeasures", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
