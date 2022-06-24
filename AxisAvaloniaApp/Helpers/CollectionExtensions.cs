using AxisAvaloniaApp.Models;
using Microinvest.PDFCreator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Helpers
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Finds and returns an OperationItemModel by ItemMode.
        /// </summary>
        /// <param name="collection">Collection to search.</param>
        /// <param name="item">Item to search it into collection.</param>
        /// <returns>Returns OperationItemModel if collection contains item; otherwise returns null.</returns>
        /// <date>21.06.2022.</date>
        public static OperationItemModel? Contains(this ObservableCollection<OperationItemModel> collection, ItemModel item)
        {
            return collection.Where(c => c.Item.Id == item.Id).FirstOrDefault();
        }

        /// <summary>
        /// Finds and returns an VATModel by rate of VAT.
        /// </summary>
        /// <param name="vATs">Collection to search.</param>
        /// <param name="vATRate">Rate of VAT to search into collection.</param>
        /// <returns>Returns VATModel if collection contains item; otherwise returns null.</returns>
        /// <date>24.06.2022.</date>
        public static VATModel? Contains(this List<VATModel> vATs, float vATRate)
        {
            return vATs.Where(v => v.VATRate == vATRate).FirstOrDefault();
        }

        /// <summary>
        /// Copies images from List to ObservableCollection.
        /// </summary>
        /// <param name="images">Collection to copy.</param>
        /// <returns>Returns ObservableCollection of images.</returns>
        /// <date>24.06.2022.</date>
        public static ObservableCollection<System.Drawing.Image> Clone(this List<System.Drawing.Image> images)
        {
            ObservableCollection<System.Drawing.Image> imagesList = new ObservableCollection<System.Drawing.Image>();
            foreach (System.Drawing.Image image in images)
            {
                imagesList.Add(image);
            }

            return imagesList;
        }
    }
}
