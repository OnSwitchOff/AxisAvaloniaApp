namespace DataBase.Entities.Serializations
{
    public class Serialization : Entity
    {
        public int Id { get; set; }
        public string Group { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Serialization"/> class.
        /// </summary>
        internal Serialization() : this("", "", "")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Serialization"/> class.
        /// </summary>
        /// <param name="group">Name of the serialization group.</param>
        /// <param name="key">Name of the serialization key.</param>
        /// <param name="value">Value of the serialization data.</param>
        private Serialization(string group, string key, string value)
        {
            this.Group = group;
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Serialization"/> class.
        /// </summary>
        /// <param name="group">Name of the serialization group.</param>
        /// <param name="key">Name of the serialization key.</param>
        /// <param name="value">Value of the serialization data.</param>
        /// <returns>Returns <see cref="Serialization"/> class if parameters are correct.</returns>
        public static Serialization Create(string group, string key, string value)
        {
            // check rule

            return new Serialization(group, key, value);
        }
    }
}
