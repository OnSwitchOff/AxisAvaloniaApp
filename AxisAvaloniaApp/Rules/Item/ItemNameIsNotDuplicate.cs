using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using DataBase.Repositories.Items;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Item
{
    public class ItemNameIsNotDuplicate : AbstractStage
    {
        private readonly IItemRepository itemRepository;
        private ItemModel item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNameIsNotDuplicate"/> class.
        /// </summary>
        /// <param name="item">Data of item.</param>
        public ItemNameIsNotDuplicate(ItemModel item)
        {
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
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
            if (await itemRepository.ItemNameIsDuplicatedAsync(item.Name, item.Id))
            {
                await loggerService.ShowDialog("msgDuplicateItemName", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
