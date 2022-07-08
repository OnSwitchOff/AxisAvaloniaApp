using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.ItemsGroups;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.ItemsGroup
{
    public class SaveItemsGroup : AbstractStage
    {
        private readonly IItemsGroupsRepository itemsGroupsRepository;
        private GroupModel group;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveItemsGroup"/> class.
        /// </summary>
        /// <param name="group">Data of items group.</param>
        /// <param name="itemsGroupsRepository">Repository to update data in the database.</param>
        public SaveItemsGroup(GroupModel group, IItemsGroupsRepository itemsGroupsRepository)
        {
            this.itemsGroupsRepository = itemsGroupsRepository;
            this.group = group;
        }

        /// <summary>
        /// Gets value indicating whether the items group is new.
        /// </summary>
        /// <date>07.07.2022.</date>
        public bool IsNewItemsGroup { get; private set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>07.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            IsNewItemsGroup = group.Id == 0;
            bool isSuccess;
            switch (group.Id)
            {
                case 0:
                    group.SetPath();
                    group.Id = await itemsGroupsRepository.AddGroupAsync((DataBase.Entities.ItemsGroups.ItemsGroup)group);
                    isSuccess = group.Id > 0;
                    break;
                default:
                    isSuccess = await itemsGroupsRepository.UpdateGroupAsync((DataBase.Entities.ItemsGroups.ItemsGroup)group);
                    break;
            }

            if (isSuccess)
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during writing/updating the items group data in the database!", nameof(SaveItemsGroup.Invoke));
                await loggerService.ShowDialog("msgErrorDuringSavingOrUpdatingGroup", "strWarning", UserControls.MessageBox.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
