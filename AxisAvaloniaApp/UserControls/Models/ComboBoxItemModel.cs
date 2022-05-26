using System;

namespace AxisAvaloniaApp.UserControls.Models
{
    /// <summary>
    /// Describes data of item of ComboBox.
    /// </summary>
    public class ComboBoxItemModel
    {
        /// <summary>
        /// Gets or sets text that will be shown by user.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string? Key { get; set; }

        /// <summary>
        /// Gets or sets value of item of ComboBox.
        /// </summary>
        /// <date>22.03.2022.</date>
        public object? Value { get; set; }


        public ComboBoxItemModel()
        {

        }

        public ComboBoxItemModel(Enum v)
        {
            Value = v;
            Key = "str" + v.ToString();
        }
    }
}
