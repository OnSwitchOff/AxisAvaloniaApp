using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entities.Interfaces
{
    public interface INomenclaturesGroup
    {
        /// <summary>
        /// Gets or sers Id of group.
        /// </summary>
        /// <date>17.06.2022.</date>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets path of group.
        /// </summary>
        /// <date>17.06.2022.</date>
        string Path { get; set; }

        /// <summary>
        /// Gets or sets name of group.
        /// </summary>
        /// <date>17.06.2022.</date>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets discount of group.
        /// </summary>
        /// <date>17.06.2022.</date>
        int Discount { get; set; }
    }
}
