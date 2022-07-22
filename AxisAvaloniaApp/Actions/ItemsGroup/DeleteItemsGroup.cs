using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.ItemsGroups;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.ItemsGroup
{
    public class DeleteItemsGroup : AbstractStage
    {
        private readonly IItemsGroupsRepository itemsGroupsRepository;
        private GroupModel group;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteItemsGroup"/> class.
        /// </summary>
        /// <param name="group">Data of items group.</param>
        public DeleteItemsGroup(GroupModel group)
        {
            itemsGroupsRepository = Splat.Locator.Current.GetRequiredService<IItemsGroupsRepository>();
            this.group = group;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>07.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (await itemsGroupsRepository.DeleteGroupAsync(group.Id))
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during deleting the items group data from the database!", nameof(DeleteItemsGroup.Invoke));
                await loggerService.ShowDialog("msgErrorDuringDeletingGroup", "strWarning", UserControls.MessageBoxes.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
