using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Items;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Item
{
    public class SaveItem : AbstractStage
    {
        private readonly IItemRepository itemRepository;
        private ItemModel item;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveItem"/> class.
        /// </summary>
        /// <param name="item">Data of item.</param>
        public SaveItem(ItemModel item)
        {
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
            this.item = item;
        }

        /// <summary>
        /// Gets value indicating whether the item is new.
        /// </summary>
        /// <date>06.07.2022.</date>
        public bool IsNewItem { get; private set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>06.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            IsNewItem = item.Id == 0;
            bool isSuccess;
            switch (item.Id)
            {
                case 0:
                    int newItemId = await itemRepository.AddItemAsync((DataBase.Entities.Items.Item)item);
                    item.Id = newItemId;
                    isSuccess = newItemId > 0;
                    break;
                default:
                    isSuccess = await itemRepository.UpdateItemAsync((DataBase.Entities.Items.Item)item);
                    break;
            }

            if (isSuccess)
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during writing/updating the item data in the database!", nameof(SaveItem.Invoke));
                await loggerService.ShowDialog("msgErrorDuringSavingOrUpdatingItem", "strWarning", UserControls.MessageBox.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
