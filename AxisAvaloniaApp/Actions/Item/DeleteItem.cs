using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Items;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Item
{
    public class DeleteItem : AbstractStage
    {
        private readonly IItemRepository itemRepository;
        private ItemModel item;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteItem"/> class.
        /// </summary>
        /// <param name="item">Data of item.</param>
        public DeleteItem(ItemModel item)
        {
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
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
            if (await itemRepository.DeleteItemAsync(item.Id))
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during deleting the item data from the database!", nameof(DeleteItem.Invoke));
                await loggerService.ShowDialog("msgErrorDuringDeletingItem", "strWarning", UserControls.MessageBoxes.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
