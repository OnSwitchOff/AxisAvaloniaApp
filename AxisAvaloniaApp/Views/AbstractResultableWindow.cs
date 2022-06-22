using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Views
{
    public abstract class AbstractResultableWindow : Window, IResultable
    {
        public abstract bool? DialogResult { get; set; }
    }
}
