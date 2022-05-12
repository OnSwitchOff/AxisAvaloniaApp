namespace DataBase.Repositories.Serializations
{
    public interface ISerializationRepository
    {
        /// <summary>
        /// Gets value of the serialization data by the key and name of group.
        /// </summary>
        /// <param name="groupName">Name of serialization data group.</param>
        /// <param name="key">Key to search value of serialization data.</param>
        /// <param name="defaultValue">Default value of serialization data.</param>
        /// <returns>Returns value of the serialization data.</returns>
        /// <date>28.03.2022.</date>
        string GetValue(string groupName, string key, string defaultValue);

        /// <summary>
        /// Sets value of the serialization data by the key and name of group.
        /// </summary>
        /// <param name="groupName">Name of serialization data group.</param>
        /// <param name="key">Key to search value of serialization data.</param>
        /// <param name="value">New value of serialization data.</param>
        /// <date>28.03.2022.</date>
        void SetValue(string groupName, string key, string value);

        /// <summary>
        /// Updates value of the serialization data by the key and name of group.
        /// </summary>
        /// <param name="groupName">Name of serialization data group.</param>
        /// <param name="key">Key to search value of serialization data.</param>
        /// <param name="value">New value of serialization data.</param>
        /// <date>28.03.2022.</date>
        void UpdateValue(string groupName, string key, string value);
    }
}
