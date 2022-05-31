using Microinvest.DeviceService.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data of operation.
    /// </summary>
    public class OperationItemModel : ReactiveObject
    {
        private ItemModel item;
        private string code;
        private string name;
        private string barcode;
        private double qty;
        private ObservableCollection<ItemCodeModel> measures;
        private ItemCodeModel selectedMeasure;
        private double partnerDiscount;
        private double itemDiscount;
        private double discount;
        private double price;
        private double amount;
        private string note;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationItemModel"/> class.
        /// </summary>
        public OperationItemModel()
        {
            this.item = new ItemModel();
            this.code = string.Empty;
            this.name = string.Empty;
            this.barcode = string.Empty;
            this.qty = 0;
            this.selectedMeasure = new ItemCodeModel();
            this.measures = new ObservableCollection<ItemCodeModel>();
            this.measures.Add(this.selectedMeasure);
            this.partnerDiscount = 0;
            this.itemDiscount = 0;
            this.discount = 0;
            this.price = 0;
            this.amount = 0;
            this.note = string.Empty;

            this.PropertyChanged += OperationItemModel_PropertyChanged;
        }

        /// <summary>
        /// Gets or sets id of additional code of item.
        /// </summary>
        /// <date>14.03.2022.</date>
        public ItemModel Item
        {
            get => item;
            set => this.RaiseAndSetIfChanged(ref item, value);
        }

        /// <summary>
        /// Gets or sets code of item.
        /// </summary>
        /// <date>15.03.2022.</date>
        public string Code
        {
            get => code;
            set => this.RaiseAndSetIfChanged(ref code, value);
        }

        /// <summary>
        /// Gets or sets name of item.
        /// </summary>
        /// <date>15.03.2022.</date>
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        /// <summary>
        /// Gets or sets barcode of item.
        /// </summary>
        /// <date>15.03.2022.</date>
        public string Barcode
        {
            get => barcode;
            set => this.RaiseAndSetIfChanged(ref barcode, value);
        }

        /// <summary>
        /// Gets or sets quantity of item.
        /// </summary>
        /// <date>15.03.2022.</date>
        public double Qty
        {
            get => qty;
            set => this.RaiseAndSetIfChanged(ref qty, value);
        }

        /// <summary>
        /// Gets or sets supported measures list.
        /// </summary>
        /// <date>15.03.2022.</date>
        public ObservableCollection<ItemCodeModel> Measures
        {
            get => measures;
            set => this.RaiseAndSetIfChanged(ref measures, value);
        }

        /// <summary>
        /// Gets or sets selected measure.
        /// </summary>
        /// <date>15.03.2022.</date>
        public ItemCodeModel SelectedMeasure
        {
            get => selectedMeasure;
            set => this.RaiseAndSetIfChanged(ref selectedMeasure, value);
        }

        /// <summary>
        /// Gets or sets discount of group of partners.
        /// </summary>
        /// <date>15.03.2022.</date>
        public double PartnerDiscount
        {
            get => partnerDiscount;
            set => this.RaiseAndSetIfChanged(ref partnerDiscount, value);
        }

        /// <summary>
        /// Gets or sets discount of group of items.
        /// </summary>
        /// <date>15.03.2022.</date>
        public double ItemDiscount
        {
            get => itemDiscount;
            set => this.RaiseAndSetIfChanged(ref itemDiscount, value);
        }

        /// <summary>
        /// Gets or sets discount.
        /// </summary>
        /// <date>15.03.2022.</date>
        public double Discount
        {
            get => discount;
            set => this.RaiseAndSetIfChanged(ref discount, value);
        }

        /// <summary>
        /// Gets or sets price of item.
        /// </summary>
        /// <date>15.03.2022.</date>
        public double Price
        {
            get => price;
            set => this.RaiseAndSetIfChanged(ref price, value);
        }

        /// <summary>
        /// Gets amount to pay.
        /// </summary>
        /// <date>15.03.2022.</date>
        public double Amount
        {
            get => amount;
            private set => this.RaiseAndSetIfChanged(ref amount, value);
        }

        /// <summary>
        /// Gets or sets notey.
        /// </summary>
        /// <date>15.03.2022.</date>
        public string Note
        {
            get => note;
            set => this.RaiseAndSetIfChanged(ref note, value);
        }

        /// <summary>
        /// Casts OperationItemModel to SaleProductModel.
        /// </summary>
        /// <param name="operationItem">Operation item data.</param>
        /// <date>17.03.2022.</date>
        public static implicit operator SaleProductModel(OperationItemModel operationItem)
        {
            SaleProductModel productModel = new SaleProductModel();
            productModel.Name = operationItem.Item.Name;
            productModel.Price = (decimal)operationItem.Price;
            productModel.Quantity = (decimal)operationItem.Qty;
            productModel.Discount = Math.Round((decimal)operationItem.Discount / 100, 2);
            productModel.VAT = new PrinterService.Models.VATModel(
                operationItem.Item.VATGroup.Id.ToString(),
                operationItem.Item.VATGroup.Name,
                Math.Round((decimal)operationItem.Item.VATGroup.Value / 100, 2));

            return productModel;
        }

        /// <summary>
        /// Updates dependent property when main property was changed.
        /// </summary>
        /// <param name="e">Event args.</param>
        /// <date>29.03.2022.</date>
        private void OperationItemModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(this.Item):
                    this.Code = this.Item.Code;
                    this.Barcode = this.Item.Barcode;
                    this.Name = this.Item.Name;
                    this.Measures.Clear();
                    this.Measures.Add(new ItemCodeModel()
                    {
                        Code = this.Code,
                        Measure = this.Item.Measure,
                    });
                    foreach (ItemCodeModel itemCode in this.Item.Codes)
                    {
                        this.Measures.Add(itemCode);
                    }

                    this.SelectedMeasure = this.Measures[0];
                    this.Qty = 1;
                    this.Price = (double)this.Item.Price;
                    this.ItemDiscount = this.Item.Group.Discount;

                    break;
                case nameof(this.Qty):
                    break;
                case nameof(this.Price):
                    break;
                case nameof(this.Discount):
                    break;
                case nameof(this.ItemDiscount):
                case nameof(this.PartnerDiscount):
                    break;
            }
        }
    }
}
