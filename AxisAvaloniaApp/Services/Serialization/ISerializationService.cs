using DataBase.Repositories.Serializations;
using AxisAvaloniaApp.Enums;

namespace AxisAvaloniaApp.Services.Serialization
{
    /// <summary>
    /// Describes serialization service.
    /// </summary>
    public interface ISerializationService
    {
        /// <summary>
        /// Gets or sets serialization value by key.
        /// </summary>
        /// <param name="key">Key to search serialization value.</param>
        /// <returns>SerializationItemModel.</returns>
        /// <date>28.03.2022.</date>
        SerializationItemModel this[ESerializationKeys key] { get; set; }

        /// <summary>
        /// Set serialization data property.
        /// </summary>
        /// <param name="group">Key to search serialization value.</param>
        /// <date>28.03.2022.</date>
        void InitSerializationData(ESerializationGroups group);

        /// <summary>
        /// Determines whether the serialization data contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the serialization data.</param>
        /// <returns>Returns true if the serialization data contains an element with the specified key; otherwise returns false.</returns>
        /// <date>28.03.2022.</date>
        bool ContainsKey(ESerializationKeys key);

        /// <summary>
        /// Update serialization values in the database.
        /// </summary>
        /// <date>28.03.2022.</date>
        void Update();
    }
}
