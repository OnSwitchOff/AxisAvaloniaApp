using System.Linq;

namespace DataBase.Repositories.Settings
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public SettingsRepository(DatabaseContext databaseContext)
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
            Entities.Settings.Setting setting = databaseContext.Settings.SingleOrDefault(s => s.Group.Equals(groupName) && s.Key.Equals(key));

            if (setting != null)
            {
                return setting.Value;
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
            databaseContext.Add(Entities.Settings.Setting.Create(groupName, key, value));
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
            Entities.Settings.Setting setting = databaseContext.Settings.SingleOrDefault(s => s.Group.Equals(groupName) && s.Key.Equals(key));

            if (setting != null)
            {
                setting.Value = value;
                databaseContext.Update(setting);
                databaseContext.SaveChangesAsync();
            }
            else
            {
                SetValue(groupName, key, value);
            }
        }
    }
}
