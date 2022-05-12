using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data of additional code of item.
    /// </summary>
    public class ItemCodeModel : ObservableObject
    {
        private int id;
        private string code;
        private string measure;
        private double multiplier;
        private ObservableCollection<string> measures = new ObservableCollection<string> { "шт", "л" };

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCodeModel"/> class.
        /// </summary>
        public ItemCodeModel()
        {
            this.id = 0;
            this.code = string.Empty;
            this.measure = "item";
            this.multiplier = 1.0;
        }

        /// <summary>
        /// Gets or sets id of additional code of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public int Id
        {
            get => this.id;
            set => this.SetProperty(ref this.id, value);
        }

        /// <summary>
        /// Gets or sets name of additional code of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Code
        {
            get => this.code;
            set => this.SetProperty(ref this.code, value);
        }

        /// <summary>
        /// Gets or sets measure for current code.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Measure
        {
            get => this.measure;
            set => this.SetProperty(ref this.measure, value);
        }

        /// <summary>
        /// Gets or sets exchange rate between main measure and current measure.
        /// </summary>
        /// <date>14.03.2022.</date>
        public double Multiplier
        {
            get => this.multiplier;
            set => this.SetProperty(ref this.multiplier, value);
        }

        public ObservableCollection<string> Measures
        {
            get => measures;
            set => SetProperty(ref measures, value);
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
    }
}
