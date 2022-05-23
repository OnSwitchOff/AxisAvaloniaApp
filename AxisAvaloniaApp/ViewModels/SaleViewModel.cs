using DataBase.Repositories.ApplicationLog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class SaleViewModel : ViewModelBase
    {
        public SaleViewModel()
        {
            IsCached = false;
        }

        private string activeSale;
        public string ActiveSale
        {
            get => activeSale;
            set => this.RaiseAndSetIfChanged(ref activeSale, value);
        }
    }
}
