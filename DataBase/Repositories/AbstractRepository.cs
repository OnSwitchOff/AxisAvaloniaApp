using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Repositories
{
    internal class AbstractRepository
    {
        public bool CanExecute { get; set; }

        public AbstractRepository()
        {
            CanExecute = true;
        }
    }
}
