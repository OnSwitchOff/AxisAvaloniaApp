using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.OperationHeader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Item
{
    public class AddRevaluationData : AbstractStage
    {
        private readonly IOperationHeaderRepository headerRepository;
        private ItemModel item;
        private double oldPrice;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddRevaluationData"/> class.
        /// </summary>
        /// <param name="item">Data of item.</param>
        /// <param name="oldPrice">Old item price.</param>
        public AddRevaluationData(ItemModel item, double oldPrice)
        {
            headerRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();
            this.item = item;
            this.oldPrice = oldPrice;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>06.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            bool isSuccess;
            //switch (item.Id)
            //{
            //    case 0:
            //        int newItemId = await itemRepository.AddItemAsync((DataBase.Entities.Items.Item)item);
            //        item.Id = newItemId;
            //        isSuccess = newItemId > 0;
            //        break;
            //    default:
            //        isSuccess = await itemRepository.UpdateItemAsync((DataBase.Entities.Items.Item)item);
            //        break;
            //}

            if (isSuccess)
            {
                return await base.Invoke(request);
            }
            else
            {
                loggerService.RegisterError(this, "An error occurred during writing/updating the item data in the database!", nameof(SaveItem.Invoke));
                await loggerService.ShowDialog("msgErrorDuringSavingOrUpdatingItem", "strWarning", UserControls.MessageBox.EButtonIcons.Error);
                return await Task.FromResult<object>(-1);
            }
        }
    }
}
