using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using DataBase.Repositories.PartnersGroups;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.PartnersGroup
{
    public class PartnersGroupNameIsNotDuplicated : AbstractStage
    {
        private readonly IPartnersGroupsRepository partnersGroupsRepository;
        private GroupModel group;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnersGroupNameIsNotDuplicated"/> class.
        /// </summary>
        /// <param name="group">Data of partners group.</param>
        public PartnersGroupNameIsNotDuplicated(GroupModel group)
        {
            partnersGroupsRepository = Splat.Locator.Current.GetRequiredService<IPartnersGroupsRepository>();
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
            if (await partnersGroupsRepository.PartnersGroupNameIsDuplicatedAsync(group.Name, group.Id))
            {
                await loggerService.ShowDialog("msgGroupNameIsDuplicated", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
