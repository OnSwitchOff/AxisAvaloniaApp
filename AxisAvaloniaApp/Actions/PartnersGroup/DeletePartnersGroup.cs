using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.PartnersGroups;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.PartnersGroup
{
    public class DeletePartnersGroup : AbstractStage
    {
        private readonly IPartnersGroupsRepository partnersGroupsRepository;
        private GroupModel group;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePartnersGroup"/> class.
        /// </summary>
        /// <param name="group">Data of partners group.</param>
        public DeletePartnersGroup(GroupModel group)
        {
            partnersGroupsRepository = Splat.Locator.Current.GetRequiredService<IPartnersGroupsRepository>();
            this.group = group;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>06.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (await partnersGroupsRepository.DeleteGroupAsync(group.Id))
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during deleting the partners group data from the database!", nameof(DeletePartnersGroup.Invoke));
                await loggerService.ShowDialog("msgErrorDuringDeletingGroup", "strWarning", UserControls.MessageBoxes.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
