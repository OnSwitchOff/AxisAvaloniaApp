using AxisAvaloniaApp.Helpers;
using DataBase.Repositories.Items;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Item
{
    public class ItemBarcodeIsNotDuplicate : AbstractStage
    {
        private readonly IItemRepository itemRepository;
        private string barcode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemBarcodeIsNotDuplicate"/> class.
        /// </summary>
        /// <param name="barcode">Barcode of item.</param>
        public ItemBarcodeIsNotDuplicate(string barcode)
        {
            itemRepository = Splat.Locator.Current.GetRequiredService<IItemRepository>();
            this.barcode = barcode;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>04.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (!string.IsNullOrEmpty(barcode) &&  await itemRepository.ItemBarcodeIsDuplicated(barcode))
            {
                await loggerService.ShowDialog("msgDuplicateItemBarcode", "strAttention", UserControls.MessageBox.EButtonIcons.Warning);
                return Task.FromResult<object>(-1);
            }

            return base.Invoke(request);
        }
    }
}
