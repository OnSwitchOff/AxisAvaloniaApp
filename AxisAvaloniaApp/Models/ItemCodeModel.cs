using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Translation;
using ReactiveUI;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data of additional code of item.
    /// </summary>
    public class ItemCodeModel : BaseModel
    {
        private readonly ITranslationService translationService;
        private int id;
        private string code;
        private string measure;
        private double multiplier;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCodeModel"/> class.
        /// </summary>
        public ItemCodeModel()
        {
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();

            this.id = 0;
            this.code = string.Empty;
            this.measure = translationService.Localize("strMeasureItem");
            this.multiplier = 1.0;
        }

        /// <summary>
        /// Gets or sets id of additional code of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public int Id
        {
            get => this.id;
            set => this.RaiseAndSetIfChanged(ref this.id, value);
        }

        /// <summary>
        /// Gets or sets name of additional code of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Code
        {
            get => this.code;
            set => this.RaiseAndSetIfChanged(ref this.code, value);
        }

        /// <summary>
        /// Gets or sets measure for current code.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Measure
        {
            get => this.measure;
            set => this.RaiseAndSetIfChanged(ref this.measure, value);
        }

        /// <summary>
        /// Gets or sets exchange rate between main measure and current measure.
        /// </summary>
        /// <date>14.03.2022.</date>
        public double Multiplier
        {
            get => this.multiplier;
            set => this.RaiseAndSetIfChanged(ref this.multiplier, value);
        }

        /// <summary>
        /// Casts ItemCode to ItemCodeModel.
        /// </summary>
        /// <param name="productCode">Data received from the database.</param>
        /// <date>15.03.2022.</date>
        public static implicit operator ItemCodeModel(DataBase.Entities.ItemsCodes.ItemCode productCode)
        {
            ItemCodeModel itemCode = new ItemCodeModel();

            if (productCode != null)
            {
                itemCode.id = productCode.Id;
                itemCode.code = productCode.Code;
                itemCode.measure = productCode.Measure;
                itemCode.multiplier = productCode.Multiplier;
            }

            return itemCode;
        }

        /// <summary>
        /// Casts ItemCodeModel to ItemCode.
        /// </summary>
        /// <param name="itemCode">Data of item code.</param>
        /// <date>15.03.2022.</date>
        public static implicit operator DataBase.Entities.ItemsCodes.ItemCode(ItemCodeModel itemCode)
        {
            DataBase.Entities.ItemsCodes.ItemCode productCode = DataBase.Entities.ItemsCodes.ItemCode.Create(
                itemCode.Code,
                null,
                itemCode.Measure,
                itemCode.Multiplier);
            productCode.Id = itemCode.Id;

            return productCode;
        }

        /// <summary>
        /// Creates clone of ItemCodeModel object.
        /// </summary>
        /// <returns>New ItemCodeModel object.</returns>
        /// <date>23.06.2022.</date>
        public ItemCodeModel Clone()
        {
            ItemCodeModel newItemCode = new ItemCodeModel();
            newItemCode.Clone(this);

            return newItemCode;
        }
    }
}
