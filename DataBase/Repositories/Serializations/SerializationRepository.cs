using System.Linq;

namespace DataBase.Repositories.Serializations
{
    public class SerializationRepository : ISerializationRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public SerializationRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// Gets value of the setting by the key and name of group.
        /// </summary>
        /// <param name="groupName">Name of settings group.</param>
        /// <param name="key">Key to search value of setting.</param>
        /// <param name="defaultValue">Default value of setting.</param>
        /// <returns>Returns value of the setting.</returns>
        /// <date>25.03.2022.</date>
        public string GetValue(string groupName, string key, string defaultValue)
        {
            Entities.Serializations.Serialization serialization = databaseContext.Serializations.FirstOrDefault(s => s.Group.Equals(groupName) && s.Key.Equals(key));

            if (serialization != null)
            {
                return serialization.Value;
            }
            else
            {
                this.SetValue(groupName, key, defaultValue);

                return defaultValue;
            }
        }

        /// <summary>
        /// Sets value of the setting by the key and name of group.
        /// </summary>
        /// <param name="groupName">Name of settings group.</param>
        /// <param name="key">Key to search value of setting.</param>
        /// <param name="value">New value of setting.</param>
        /// <date>25.03.2022.</date>
        public void SetValue(string groupName, string key, string value)
        {
            databaseContext.Add(Entities.Serializations.Serialization.Create(groupName, key, value));
            databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates value of the setting by the key and name of group.
        /// </summary>
        /// <param name="groupName">Name of settings group.</param>
        /// <param name="key">Key to search value of setting.</param>
        /// <param name="value">New value of setting.</param>
        /// <date>25.03.2022.</date>
        public void UpdateValue(string groupName, string key, string value)
        {
            Entities.Serializations.Serialization serialization = databaseContext.Serializations.FirstOrDefault(s => s.Group.Equals(groupName) && s.Key.Equals(key));

            if (serialization != null)
            {
                serialization.Value = value;
                databaseContext.Update(serialization);
                databaseContext.SaveChangesAsync();
            }
            else
            {
                SetValue(groupName, key, value);
            }
        }
    }
}
