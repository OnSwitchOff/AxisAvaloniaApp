using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Models
{
    public class TreeViewModel
    {
        public string Name { get; set; }

        public List<TreeViewModel> Nodes { get; set; } = new List<TreeViewModel>();
    }
}
