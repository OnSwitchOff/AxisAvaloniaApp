using AxisAvaloniaApp.Helpers;
using DataBase.Repositories.Items;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Item
{
    public class ItemNameIsNotDuplicate : AbstractStage
    {
        private readonly IItemRepository itemRepository;
        private string itemName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNameIsNotDuplicate"/> class.
        /// </summary>
        /// <param name="itemName">Name of item.</param>
        public ItemNameIsNotDuplicate(string itemName)
        {
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
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
            if (await itemRepository.ItemNameIsDuplicated(itemName))
            {
                await loggerService.ShowDialog("msgDuplicateItemName", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                return Task.FromResult<object>(-1);
            }

            return base.Invoke(request);
        }
    }
}
