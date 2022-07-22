using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.PartnersGroup
{
    public class PartnersGroupNameIsNotEmpty : AbstractStage
    {
        private string partnersGroupName;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnersGroupNameIsNotEmpty"/> class.
        /// </summary>
        /// <param name="itemsGroupName">Name of partners group.</param>
        public PartnersGroupNameIsNotEmpty(string partnersGroupName)
        {
            this.partnersGroupName = partnersGroupName;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>04.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (string.IsNullOrEmpty(partnersGroupName))
            {
                await loggerService.ShowDialog("msgGroupNameIsEmpty", "strAttention", UserControls.MessageBoxes.EButtonIcons.Warning);
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
