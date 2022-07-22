using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using DataBase.Repositories.ItemsGroups;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.ItemsGroup
{
    public class ItemsGroupNameIsNotDuplicated : AbstractStage
    {
        private readonly IItemsGroupsRepository itemsGroupRepository;
        private GroupModel group;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsGroupNameIsNotDuplicated"/> class.
        /// </summary>
        /// <param name="group">Data of items group.</param>
        public ItemsGroupNameIsNotDuplicated(GroupModel group)
        {
            itemsGroupRepository = Splat.Locator.Current.GetRequiredService<IItemsGroupsRepository>();
            this.group = group;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>04.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (await itemsGroupRepository.ItemsGroupNameIsDuplicatedAsync(group.Name, group.Id))
            {
                await loggerService.ShowDialog("msgGroupNameIsDuplicated", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
