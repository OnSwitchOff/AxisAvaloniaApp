using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.Templates;
using System.Collections;
using System.Linq;

namespace AxisAvaloniaApp.UserControls.ComboBoxes
{
    public class ComboBoxWithTreeView : TemplatedControl
    {
        public static readonly StyledProperty<string?> PlaceholderTextProperty =
            AvaloniaProperty.Register<ComboBoxWithTreeView, string?>(nameof(PlaceholderText), string.Empty);

        /// <summary>
        /// Gets or sets placeholder text of the ComboBox.
        /// </summary>
        /// <date>02.06.2022.</date>
        private string? PlaceholderText
        {
            get => GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public static readonly StyledProperty<string> ExplanationKeyProperty =
            AvaloniaProperty.Register<ComboBoxWithTreeView, string>(nameof(ExplanationKey), string.Empty);

        /// <summary>
        /// Gets or sets key to search explanation for this ComboBox in the dictionary.
        /// </summary>
        /// <date>02.06.2022.</date>
        public string ExplanationKey
        {
            get => GetValue(ExplanationKeyProperty);
            set => SetValue(ExplanationKeyProperty, value);
        }

        public static readonly StyledProperty<PlacementMode> PlacementModeProperty =
            AvaloniaProperty.Register<ComboBoxWithTreeView, PlacementMode>(nameof(PlacementMode), PlacementMode.Bottom);

        /// <summary>
        /// Gets or sets the placement mode of the popup in relation to the ComboBox.
        /// </summary>
        /// <date>02.06.2022.</date>
        public PlacementMode PlacementMode
        {
            get => GetValue(PlacementModeProperty);
            set => SetValue(PlacementModeProperty, value);
        }

        public static readonly StyledProperty<IEnumerable> ItemsProperty =
            AvaloniaProperty.Register<ComboBoxWithTreeView, IEnumerable>(nameof(Items));

        /// <summary>
        /// Gets or sets the items to display.
        /// </summary>
        /// <date>02.06.2022.</date>
        public IEnumerable Items
        {
            get => GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public static readonly StyledProperty<int> ItemCountProperty =
            AvaloniaProperty.Register<ComboBoxWithTreeView, int>(nameof(ItemCount));

        /// <summary>
        /// Gets the number of items in <see cref="Items"/>.
        /// </summary>
        /// <date>02.06.2022.</date>
        public int ItemCount
        {
            get => (from object item in Items
                    select item).Count();
        }

        public static readonly StyledProperty<TreeDataTemplate> ItemTemplateProperty =
            AvaloniaProperty.Register<ComboBoxWithTreeView, TreeDataTemplate>(nameof(ItemTemplate));

        /// <summary>
        /// Gets or sets the data template used to display the items in the control.
        /// </summary>
        /// <date>02.06.2022.</date>
        public TreeDataTemplate ItemTemplate
        {
            get => GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly StyledProperty<object> SelectedItemProperty =
            AvaloniaProperty.Register<ComboBoxWithTreeView, object>(nameof(SelectedItem));

        /// <summary>
        ///  Gets or sets the selected item.
        /// </summary>
        /// <date>02.06.2022.</date>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly StyledProperty<string> KeyTextProperty =
            AvaloniaProperty.Register<ComboBoxWithTreeView, string>(nameof(KeyText), string.Empty);

        /// <summary>
        /// Gets or sets key to find property of selected item that should be shown into ComboBox.
        /// </summary>
        /// <date>02.06.2022.</date>
        public string KeyText
        {
            get => GetValue(KeyTextProperty);
            set => SetValue(KeyTextProperty, value);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            TreeView treeView = e.NameScope.Find<TreeView>("treeView");
            if (treeView != null)
            {
                if (treeView.SelectedItem != null)
                {
                    PlaceholderText = GetText();
                }

                treeView.DoubleTapped += (tv, e) =>
                {
                    if (SelectedItem != null)
                    {
                        PlaceholderText = GetText();
                    }

                    if (tv is TreeView tree && tree.Parent != null && tree.Parent is Popup popup)
                    {
                        popup.IsOpen = false;
                    }
                };

                treeView.PointerLeave += (tv, e) =>
                {
                    if (tv is TreeView tree && tree.Parent != null && tree.Parent is Popup popup)
                    {
                        popup.IsOpen = false;
                    }
                };
            }
        }

        /// <summary>
        /// Gets text of SelectedItem to show user.
        /// </summary>
        /// <returns>Returns text to show user</returns>
        /// <exception cref="System.Exception">Throws exception if key to get text is absent.</exception>
        /// <date>23.06.2022.</date>
        private string? GetText()
        {
            var property = SelectedItem.GetType().GetProperty(KeyText);
            if (property != null)
            {
                return property.GetValue(SelectedItem, null)?.ToString();
            }
            else
            {
                throw new System.Exception("\"KeyText\" is not initialized or has an invalid value!");
            }
        }
    }
}
