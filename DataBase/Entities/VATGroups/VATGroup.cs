using System.Collections.Generic;

namespace DataBase.Entities.VATGroups
{
    public class VATGroup : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal VATValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VATGroup"/> class.
        /// </summary>
        internal VATGroup() : this("", 0)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VATGroup"/> class.
        /// </summary>
        /// <param name="name">Name of VAT group.</param>
        /// <param name="vATValue">Value of VAT.</param>
        private VATGroup(string name, decimal vATValue)
        {
            this.Name = name;
            this.VATValue = vATValue;
        }

        /// <summary>
        /// Creates a new instance of the VATGroup class.
        /// </summary>
        /// <param name="name">Name of VAT group.</param>
        /// <param name="vATValue">Value of VAT.</param>
        /// <returns>Returns VATGroup class if parameters are correct.</returns>
        public static VATGroup Create(string name, decimal vATValue)
        {
            // check rule

            return new VATGroup(name, vATValue);
        }
    }
}
