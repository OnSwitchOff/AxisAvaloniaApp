using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Items;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Item
{
    public class SearchItemIntoDatabaseByName : AbstractStage
    {
        private readonly IItemRepository itemRepository;
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchItemIntoDatabaseByName"/> class.
        /// </summary>
        /// <param name="name">Name to search item into database.</param>
        /// <param name="itemRepository">Repository to search data in the database.</param>
        public SearchItemIntoDatabaseByName(string name, IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
            this.name = name;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if item wasn't searched; otherwise returns item.</returns>
        /// <date>19.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var item = await itemRepository.GetItemByKeyAsync(name);
                if (item != null)
                {
                    return (Models.ItemModel)item;
                }
            }

            return await base.Invoke(request);
        }
    }
}
