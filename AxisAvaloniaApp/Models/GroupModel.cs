using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data of group.
    /// </summary>
    public class GroupModel : BaseModel
    {
        private int id;
        private string name;
        private string path;
        private double discount;
        private bool isExpanded;
        private GroupModel parentGroup;
        private ObservableCollection<GroupModel> subGroups;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupModel"/> class.
        /// </summary>
        public GroupModel()
        {
            this.id = 0;
            this.name = string.Empty;
            this.path = string.Empty;
            this.discount = 0;
            this.isExpanded = false;
            this.subGroups = new ObservableCollection<GroupModel>();
        }

        /// <summary>
        /// Gets or sets get or set Id of group.
        /// </summary>
        /// <date>14.03.2022.</date>
        public int Id
        {
            get => this.id;
            set => this.RaiseAndSetIfChanged(ref this.id, value);
        }

        /// <summary>
        /// Gets or sets get or set name of group.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        /// <summary>
        /// Gets or sets path of group.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Path
        {
            get => this.path;
            set => this.RaiseAndSetIfChanged(ref this.path, value);
        }

        /// <summary>
        /// Gets or sets discount of group.
        /// </summary>
        /// <date>14.03.2022.</date>
        public double Discount
        {
            get => this.discount;
            set => this.RaiseAndSetIfChanged(ref this.discount, value);
        }

        /// <summary>
        /// Gets or sets IsExpanded of group.
        /// </summary>
        /// <date>14.03.2022.</date>
        public bool IsExpanded
        {
            get => this.isExpanded;
            set => this.RaiseAndSetIfChanged(ref this.isExpanded, value);
        }

        /// <summary>
        /// Gets or sets parent of group.
        /// </summary>
        /// <date>14.03.2022.</date>
        public GroupModel ParentGroup
        {
            get => this.parentGroup;
            set => this.RaiseAndSetIfChanged(ref this.parentGroup, value);
        }

        /// <summary>
        /// Gets or sets list with subgroups.
        /// </summary>
        /// <date>14.03.2022.</date>
        public ObservableCollection<GroupModel> SubGroups
        {
            get => this.subGroups;
            set => this.RaiseAndSetIfChanged(ref this.subGroups, value);
        }

        /// <summary>
        /// Casts GroupModel object to DataBase.My100REnteties.PartnersGroups.PartnersGroup.
        /// </summary>
        /// <param name="group">Data of group.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator DataBase.Entities.PartnersGroups.PartnersGroup(GroupModel group)
        {
            DataBase.Entities.PartnersGroups.PartnersGroup partnersGroup = DataBase.Entities.PartnersGroups.PartnersGroup.Create(group.Path, group.Name, (int)group.Discount);
            partnersGroup.Id = group.Id;

            return partnersGroup;
        }

        /// <summary>
        /// Casts PartnersGroup object to GroupModel.
        /// </summary>
        /// <param name="partnersGroup">Data of group of partner from database.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator GroupModel?(DataBase.Entities.PartnersGroups.PartnersGroup partnersGroup)
        {
            if (partnersGroup == null)
            {
                return null;
            }

            return new GroupModel()
            {
                Id = partnersGroup.Id,
                Path = partnersGroup.Path,
                Name = partnersGroup.Name,
                Discount = partnersGroup.Discount,
            };
        }

        /// <summary>
        /// Casts GroupModel object to DataBase.My100REnteties.ItemsGroups.ItemsGroup.
        /// </summary>
        /// <param name="group">Data of group.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator DataBase.Entities.ItemsGroups.ItemsGroup?(GroupModel group)
        {
            if (group == null)
            {
                return null;
            }

            DataBase.Entities.ItemsGroups.ItemsGroup itemGroup = DataBase.Entities.ItemsGroups.ItemsGroup.Create(group.Path, group.Name, (int)group.Discount);
            itemGroup.Id = group.Id;

            return itemGroup;
        }

        /// <summary>
        /// Casts DataBase.My100REnteties.ItemsGroups.ItemsGroup object to GroupModel.
        /// </summary>
        /// <param name="itemGroup">Data of group of product from database.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator GroupModel(DataBase.Entities.ItemsGroups.ItemsGroup itemGroup)
        {
            if (itemGroup != null)
            {
                GroupModel group = new GroupModel()
                {
                    Id = itemGroup.Id,
                    Path = itemGroup.Path,
                    Name = itemGroup.Name,
                    Discount = itemGroup.Discount,
                };

                return group;
            }

            return null;
        }

        /// <summary>
        /// Sets path if it is absent.
        /// </summary>
        /// <date>07.07.2022.</date>
        public void SetPath()
        {
            if (string.IsNullOrEmpty(this.path))
            {
                string path = (this.ParentGroup.Path.Equals("-2") ? "" : this.ParentGroup.Path) + "AAA";

                StringBuilder nextPath = new StringBuilder(path);
                for (int i = nextPath.Length - 1; i >= 0; i--)
                {
                    while (nextPath[i] < 90)
                    {
                        if (this.ParentGroup.SubGroups.Where(g => g.Path.Equals(nextPath.ToString())).FirstOrDefault() == null)
                        {
                            this.Path = nextPath.ToString();
                            return;
                        }

                        nextPath[i]++;
                    }

                    nextPath[i] = 'A';
                }

                this.Path = nextPath.ToString();
            }
        }
    }
}
