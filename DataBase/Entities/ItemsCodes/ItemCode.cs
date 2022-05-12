namespace DataBase.Entities.ItemsCodes
{
    public class ItemCode : Entity
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public Items.Item Item { get; set; }
        public string Measure { get; set; } = null!;
        public double Multiplier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCode"/> class.
        /// </summary>
        internal ItemCode() : this("", null, "", 1.0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemCode"/> class.
        /// </summary>
        /// <param name="code">Additional code of item.</param>
        /// <param name="item">Item to add additional code.</param>
        /// <param name="measure">Measure of item.</param>
        /// <param name="multiplier">Ratio a additional measure to main measure.</param>
        private ItemCode(string code, Items.Item item, string measure, double multiplier)
        {
            this.Code = code;
            this.Item = item;
            this.Measure = measure;
            this.Multiplier = multiplier;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ItemCode"/> class.
        /// </summary>
        /// <param name="code">Additional code of item.</param>
        /// <param name="item">Item to add additional code.</param>
        /// <param name="measure">Measure of item.</param>
        /// <param name="multiplier">Ratio a additional measure to main measure.</param>
        /// <returns>Returns <see cref="ItemCode"/> class if parameters are correct.</returns>
        public static ItemCode Create(string code, Items.Item item, string measure, double multiplier)
        {
            // check rule

            return new ItemCode(code, item, measure, multiplier);
        }
    }
}
