namespace DataBase.Entities.Settings
{
    public class Setting : Entity
    {
        public int Id { get; set; }
        public string Group { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        internal Setting() : this("", "", "")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        /// <param name="group">Name of the settings group.</param>
        /// <param name="key">Name of the settings key.</param>
        /// <param name="value">Value of the settings data.</param>
        private Setting(string group, string key, string value)
        {
            this.Group = group;
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Setting"/> class.
        /// </summary>
        /// <param name="group">Name of the settings group.</param>
        /// <param name="key">Name of the settings key.</param>
        /// <param name="value">Value of the settings data.</param>
        /// <returns>Returns <see cref="Setting"/> class if parameters are correct.</returns>
        public static Setting Create(string group, string key, string value)
        {
            // check rule

            return new Setting(group, key, value);
        }
    }
}
