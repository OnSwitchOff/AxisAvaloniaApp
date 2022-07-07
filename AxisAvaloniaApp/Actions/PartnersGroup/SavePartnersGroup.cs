using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.PartnersGroups;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.PartnersGroup
{
    public class SavePartnersGroup : AbstractStage
    {
        private readonly IPartnersGroupsRepository partnersGroupsRepository;
        private GroupModel group;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePartnersGroup"/> class.
        /// </summary>
        /// <param name="group">Data of partners group.</param>
        public SavePartnersGroup(GroupModel group)
        {
            partnersGroupsRepository = Splat.Locator.Current.GetRequiredService<IPartnersGroupsRepository>();
            this.group = group;
        }

        /// <summary>
        /// Gets value indicating whether the partners group is new.
        /// </summary>
        /// <date>06.07.2022.</date>
        public bool IsNewPartnersGroup { get; private set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>06.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            IsNewPartnersGroup = group.Id == 0;
            bool isSuccess;
            switch (group.Id)
            {
                case 0:
                    group.SetPath();
                    group.Id = await partnersGroupsRepository.AddGroupAsync((DataBase.Entities.PartnersGroups.PartnersGroup)group);
                    isSuccess = group.Id > 0;
                    break;
                default:
                    isSuccess = await partnersGroupsRepository.UpdateGroupAsync((DataBase.Entities.PartnersGroups.PartnersGroup)group);
                    break;
            }

            if (isSuccess)
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during writing/updating the partners group data in the database!", nameof(SavePartnersGroup.Invoke));
                await loggerService.ShowDialog("msgErrorDuringSavingOrUpdatingGroup", "strWarning", UserControls.MessageBox.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
