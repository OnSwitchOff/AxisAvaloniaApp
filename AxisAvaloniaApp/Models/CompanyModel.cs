using AxisAvaloniaApp.UserControls.Models;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data of settings.
    /// </summary>
    public class CompanyModel : PartnerModel
    {
        private string onlineShopNumber;
        private ComboBoxItemModel shopType;
        private string onlineShopDomainName;

        /// <summary>
        /// Gets or sets number of an online-shop.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string OnlineShopNumber
        {
            get => this.onlineShopNumber;
            set => this.SetProperty(ref this.onlineShopNumber, value);
        }

        /// <summary>
        /// Gets or sets type of an online-shop.
        /// </summary>
        /// <date>22.03.2022.</date>
        public ComboBoxItemModel ShopType
        {
            get => this.shopType;
            set => this.SetProperty(ref this.shopType, value);
        }

        /// <summary>
        /// Gets or sets domain name of an online-shop.
        /// </summary>
        /// <date>22.03.2022.</date>
        public string OnlineShopDomainName
        {
            get => this.onlineShopDomainName;
            set => this.SetProperty(ref this.onlineShopDomainName, value);
        }
    }
}
