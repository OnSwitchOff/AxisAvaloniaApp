using AxisAvaloniaApp.Rules;
using DataBase.Repositories.Items;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Item
{
    public class FixNullItemsGroups : AbstractStage
    {
        private readonly IItemRepository itemRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixNullItemsGroups"/> class.
        /// </summary>
        /// <param name="itemRepository">Repository to update data in the database.</param>
        public FixNullItemsGroups(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step.</returns>
        /// <date>08.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            await itemRepository.SetDefaultGroup();
            return await base.Invoke(request);
        }
    }
}
