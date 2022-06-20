using DataBase.Entities.Interfaces;
using System.Collections.Generic;

namespace DataBase.Entities.PartnersGroups
{
    public class PartnersGroup : Entity, INomenclaturesGroup
    {
        /// <summary>
        /// Gets or sers Id of group.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets path of group.
        /// </summary>
        public string Path { get; set; } = null!;

        /// <summary>
        /// Gets or sets name of group.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets discount of group.
        /// </summary>
        public int Discount { get; set; }

        public List<Partners.Partner> Partners { get; set; } = new List<Partners.Partner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnersGroup"/> class.
        /// </summary>
        internal PartnersGroup() : this("", "", 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnersGroup"/> class.
        /// </summary>
        /// <param name="path">Path of a group to identify main group and subgroups.</param>
        /// <param name="name">Name of group.</param>
        /// <param name="discount">Discount that is given to partners included to this group.</param>
        private PartnersGroup(string path, string name, int discount)
        {
            this.Path = path;
            this.Name = name;
            this.Discount = discount;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PartnersGroup"/> class.
        /// </summary>
        /// <param name="path">Path of a group to identify main group and subgroups.</param>
        /// <param name="name">Name of group.</param>
        /// <param name="discount">Discount that is given to partners included to this group.</param>
        /// <returns>Returns <see cref="PartnersGroup"/> class if parameters are correct.</returns>
        public static PartnersGroup Create(string path, string name, int discount)
        {
            // check rule

            return new PartnersGroup(path, name, discount);
        }
    }
}
