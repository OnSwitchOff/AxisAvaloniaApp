using Microinvest.CommonLibrary.Enums;

namespace DataBase.Entities.Partners
{
    public class Partner : Entity
    {
        public int Id { get; set; }
        public string Company { get; set; } = null!;
        public string Principal { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string TaxNumber { get; set; } = null!;
        public string VATNumber { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public string BankBic { get; set; } = null!;
        public string IBAN { get; set; } = null!;
        public string DiscountCard { get; set; } = null!;
        public string Email { get; set; } = null!;
        public PartnersGroups.PartnersGroup Group { get; set; }
        public ENomenclatureStatuses Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Partner"/> class.
        /// </summary>
        internal Partner() : this("", "", "", "", "", "", "", "", "", "", "", "", null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Partner"/> class.
        /// </summary>
        /// <param name="company">Name of the company.</param>
        /// <param name="principal">Partner's person in charge.</param>
        /// <param name="city">City of the partner location or registration.</param>
        /// <param name="address">Address of the partner location or registration.</param>
        /// <param name="phone">Phone of the company.</param>
        /// <param name="taxNumber">Tax number of the company.</param>
        /// <param name="vATNumber">VAT number of the company.</param>
        /// <param name="bankName">Name of a bank which serves of the company.</param>
        /// <param name="bankBIC">BIC of a bank which serves of the company.</param>
        /// <param name="iBAN">IBAN of the company.</param>
        /// <param name="discountCard">Number of partner's discount card.</param>
        /// <param name="e_mail">E-mail of the company.</param>
        /// <param name="group">Group that contains of this partner.</param>
        private Partner(string company, string principal, string city, string address, string phone, string taxNumber, string vATNumber, string bankName, string bankBIC, string iBAN, string discountCard, string e_mail, PartnersGroups.PartnersGroup group)
        {
            this.Company = company;
            this.Principal = principal;
            this.City = city;
            this.Address = address;
            this.Phone = phone;
            this.TaxNumber = taxNumber;
            this.VATNumber = vATNumber;
            this.BankName = bankName;
            this.BankBic = bankBIC;
            this.IBAN = iBAN;
            this.DiscountCard = discountCard;
            this.Email = e_mail;
            this.Group = group;
            this.Status = ENomenclatureStatuses.Active;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Partner"/> class.
        /// </summary>
        /// <param name="company">Name of the company.</param>
        /// <param name="principal">Partner's person in charge.</param>
        /// <param name="city">City of the partner location or registration.</param>
        /// <param name="address">Address of the partner location or registration.</param>
        /// <param name="phone">Phone of the company.</param>
        /// <param name="taxNumber">Tax number of the company.</param>
        /// <param name="vATNumber">VAT number of the company.</param>
        /// <param name="bankName">Name of a bank which serves of the company.</param>
        /// <param name="bankBIC">BIC of a bank which serves of the company.</param>
        /// <param name="iBAN">IBAN of the company.</param>
        /// <param name="discountCard">Number of partner's discount card.</param>
        /// <param name="e_mail">E-mail of the company.</param>
        /// <param name="group">Group that contains of this partner.</param>
        /// <returns>Returns <see cref="Partner"/> class if parameters are correct.</returns>
        public static Partner Create(string company, string principal, string city, string address, string phone, string taxNumber, string vATNumber, string bankName, string bankBIC, string iBAN, string discountCard, string e_mail, PartnersGroups.PartnersGroup group)
        {
            // check rule 

            return new Partner(company, principal, city, address, phone, taxNumber, vATNumber, bankName, bankBIC, iBAN, discountCard, e_mail, group);
        }
    }
}
