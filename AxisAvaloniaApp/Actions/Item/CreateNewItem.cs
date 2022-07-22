using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules;
using AxisAvaloniaApp.Services.Translation;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Item
{
    public class CreateNewItem : AbstractStage
    {
        private readonly ITranslationService translationService;
        private Models.ItemModel item;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNewItem"/> class.
        /// </summary>
        /// <param name="item">Data of new item.</param>
        public CreateNewItem(Models.ItemModel item)
        {
            this.translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            this.item = item;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns new ItemModel object if name or barcode is not empty; otherwise returns next method to invoke.</returns>
        /// <date>20.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (string.IsNullOrEmpty(item.Name) && string.IsNullOrEmpty(item.Barcode))
            {
                return await base.Invoke(request);
            }
            else
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    item.Name = translationService.Localize("strUnidentifiedImportedGoods");
                }
                return await Task.FromResult(item);
            }
        }
    }
}
