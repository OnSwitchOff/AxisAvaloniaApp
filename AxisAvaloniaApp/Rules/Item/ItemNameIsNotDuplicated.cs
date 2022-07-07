using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using DataBase.Repositories.Items;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Item
{
    public class ItemNameIsNotDuplicated : AbstractStage
    {
        private readonly IItemRepository itemRepository;
        private ItemModel item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNameIsNotDuplicated"/> class.
        /// </summary>
        /// <param name="item">Data of item.</param>
        public ItemNameIsNotDuplicated(ItemModel item)
        {
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
            this.item = item;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
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
