using ReactiveUI;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data of VAT group.
    /// </summary>
    public class VATGroupModel : BaseModel
    {
        private int id;
        private string name;
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="VATGroupModel"/> class.
        /// </summary>
        public VATGroupModel()
        {
            this.id = 0;
            this.name = string.Empty;
            this.value = 0;
        }

        /// <summary>
        /// Gets or sets id of VAT group.
        /// </summary>
        /// <date>14.03.2022.</date>
        public int Id
        {
            get => this.id;
            set => this.RaiseAndSetIfChanged(ref this.id, value);
        }

        /// <summary>
        /// Gets or sets name of VAT group.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        /// <summary>
        /// Gets or sets vAT value (absolute value, not percent).
        /// </summary>
        /// <date>14.03.2022.</date>
        public double Value
        {
            get => this.value;
            set => this.RaiseAndSetIfChanged(ref this.value, value);
        }

        /// <summary>
        /// Casts VATGroupModel object to VATGroup.
        /// </summary>
        /// <param name="vATGroup">Data of VAT group.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator DataBase.Entities.VATGroups.VATGroup(VATGroupModel vATGroup)
        {
            DataBase.Entities.VATGroups.VATGroup vatgroup = DataBase.Entities.VATGroups.VATGroup.Create(vATGroup.Name, (decimal)vATGroup.Value);
            vatgroup.Id = vATGroup.Id;

            return vatgroup;
        }

        /// <summary>
        /// Casts VATGroup object to VATGroupModel.
        /// </summary>
        /// <param name="vATGroup">Data of VAT group from database.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator VATGroupModel(DataBase.Entities.VATGroups.VATGroup vATGroup)
        {
            VATGroupModel vatgroup = new VATGroupModel()
            {
                Id = vATGroup.Id,
                Name = vATGroup.Name,
                Value = (double)vATGroup.VATValue,
            };

            return vatgroup;
        }
    }
}
