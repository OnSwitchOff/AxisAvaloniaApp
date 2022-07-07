using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Common
{
    public class UserAgreesToDeleteNomenclature : AbstractStage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAgreesToDeleteNomenclature"/> class.
        /// </summary>
        public UserAgreesToDeleteNomenclature()
        {

        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>04.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            //if (string.IsNullOrEmpty(itemName))
            //{
            //    await loggerService.ShowDialog("msgItemNameIsEmpty", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
            //    return await Task.FromResult<object>(-1);
            //}

            return await base.Invoke(request);
        }
    }
}
