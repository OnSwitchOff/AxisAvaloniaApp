using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Models;
using AxisAvaloniaApp.Rules;
using DataBase.Repositories.OperationHeader;
using Microinvest.CommonLibrary.Enums;
using System;
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
            if (Math.Round(oldPrice, 3) != Math.Round(item.Price, 3))
            {
                DataBase.Entities.OperationHeader.OperationHeader header = DataBase.Entities.OperationHeader.OperationHeader.Create(
                                                    EOperTypes.Revaluation,
                                                    await headerRepository.GetNextAcctAsync(EOperTypes.Revaluation),
                                                    DateTime.Now,
                                                    "",
                                                    null,
                                                    null,
                                                    "",
                                                    EECCheckTypes.Unknown,
                                                    0);
                header.OperationDetails.Add(DataBase.Entities.OperationDetails.OperationDetail.Create(
                    header,
                    (DataBase.Entities.Items.Item)item,
                    0,
                    0,
                    (decimal)item.Price,
                    (decimal)(item.Price - (item.Price / (1 + item.VATGroup.Value / 100)))));

                if (await headerRepository.AddNewRecordAsync(header) == 0)
                {
                    await loggerService.ShowDialog("msgErrorDuringSavingRevaluationData", "strWarning", UserControls.MessageBox.EButtonIcons.Error);
                }
            }
            
            return await base.Invoke(request);
        }
    }
}
