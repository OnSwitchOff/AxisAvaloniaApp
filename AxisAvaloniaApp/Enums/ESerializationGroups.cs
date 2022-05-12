namespace AxisAvaloniaApp.Enums
{
    /// <summary>
    /// Key to save or update data in the Serialization table of the database by groups.
    /// </summary>
    public enum ESerializationGroups
    {
        /// <summary>
        /// Operation of sale.
        /// </summary>
        Sale,

        /// <summary>
        /// Invoice document.
        /// </summary>
        Invoice,

        /// <summary>
        /// proform-invoice document.
        /// </summary>
        Proform,

        /// <summary>
        /// Debit note.
        /// </summary>
        DebitNote,

        /// <summary>
        /// Credit note.
        /// </summary>
        CreditNote,

        /// <summary>
        /// Reports.
        /// </summary>
        Report,

        /// <summary>
        /// Nomenclature of items.
        /// </summary>
        ItemsNomenclature,

        /// <summary>
        /// Nomenclature of partners.
        /// </summary>
        PartnersNomenclature,
    }
}
