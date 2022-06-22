using AxisAvaloniaApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Helpers
{
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Finds and returns an OperationItemModel by ItemMode.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        /// <returns>Returns OperationItemModel if collection contains item; otherwise returns null.</returns>
        /// <date>21.06.2022.</date>
        public static OperationItemModel? Contains(this ObservableCollection<OperationItemModel> collection, ItemModel item)
        {
            return collection.Where(c => c.Item.Id == item.Id).FirstOrDefault();
        }
    }
}
