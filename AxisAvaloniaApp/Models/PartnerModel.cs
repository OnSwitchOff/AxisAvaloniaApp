﻿using ReactiveUI;

namespace AxisAvaloniaApp.Models
{
    /// <summary>
    /// Describes data of partner.
    /// </summary>
    public partial class PartnerModel : ReactiveObject
    {
        private int id;
        private string name;
        private string principal;
        private string city;
        private string address;
        private string phone;
        private string email;
        private string taxNumber;
        private string vATNumber;
        private string bankName;
        private string bankBIC;
        private string iBAN;
        private string discountCardNumber;
        private GroupModel group;
        private Microinvest.CommonLibrary.Enums.ENomenclatureStatuses status;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerModel"/> class.
        /// </summary>
        public PartnerModel()
        {
            this.id = 0;
            this.name = string.Empty;
            this.principal = string.Empty;
            this.city = string.Empty;
            this.address = string.Empty;
            this.phone = string.Empty;
            this.email = string.Empty;
            this.taxNumber = string.Empty;
            this.vATNumber = string.Empty;
            this.bankName = string.Empty;
            this.bankBIC = string.Empty;
            this.iBAN = string.Empty;
            this.discountCardNumber = string.Empty;
            this.group = new GroupModel();
            this.status = Microinvest.CommonLibrary.Enums.ENomenclatureStatuses.Active;
        }

        /// <summary>
        /// Gets or sets id of partner.
        /// </summary>
        /// <date>14.03.2022.</date>
        public int Id
        {
            get => this.id;
            set => this.RaiseAndSetIfChanged(ref this.id, value);
        }

        /// <summary>
        /// Gets or sets name of partner.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        /// <summary>
        /// Gets or sets partner's person in charge.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Principal
        {
            get => this.principal;
            set => this.RaiseAndSetIfChanged(ref this.principal, value);
        }

        /// <summary>
        /// Gets or sets city of the partner location or registration.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string City
        {
            get => this.city;
            set => this.RaiseAndSetIfChanged(ref this.city, value);
        }

        /// <summary>
        /// Gets or sets address of the partner location or registration.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Address
        {
            get => this.address;
            set => this.RaiseAndSetIfChanged(ref this.address, value);
        }

        /// <summary>
        /// Gets or sets phone of partner.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Phone
        {
            get => this.phone;
            set => this.RaiseAndSetIfChanged(ref this.phone, value);
        }

        /// <summary>
        /// Gets or sets e-mail of partner.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string Email
        {
            get => this.email;
            set => this.RaiseAndSetIfChanged(ref this.email, value);
        }

        /// <summary>
        /// Gets or sets partner's tax number.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string TaxNumber
        {
            get => this.taxNumber;
            set => this.RaiseAndSetIfChanged(ref this.taxNumber, value);
        }

        /// <summary>
        /// Gets or sets partner's VAT number.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string VATNumber
        {
            get => this.vATNumber;
            set => this.RaiseAndSetIfChanged(ref this.vATNumber, value);
        }

        /// <summary>
        /// Gets or sets name of a bank which serves of partner.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string BankName
        {
            get => this.bankName;
            set => this.RaiseAndSetIfChanged(ref this.bankName, value);
        }

        /// <summary>
        /// Gets or sets bIC of a bank which serves of partner.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string BankBIC
        {
            get => this.bankBIC;
            set => this.RaiseAndSetIfChanged(ref this.bankBIC, value);
        }

        /// <summary>
        /// Gets or sets iBAN of partner.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string IBAN
        {
            get => this.iBAN;
            set => this.RaiseAndSetIfChanged(ref this.iBAN, value);
        }

        /// <summary>
        /// Gets or sets number of partner's discount card.
        /// </summary>
        /// <date>14.03.2022.</date>
        public string DiscountCardNumber
        {
            get => this.discountCardNumber;
            set => this.RaiseAndSetIfChanged(ref this.discountCardNumber, value);
        }

        /// <summary>
        /// Gets or sets group of partner.
        /// </summary>
        /// <date>14.03.2022.</date>
        public GroupModel Group
        {
            get => this.group;
            set => this.RaiseAndSetIfChanged(ref this.group, value);
        }

        /// <summary>
        /// Gets or sets partner's status.
        /// </summary>
        /// <date>14.03.2022.</date>
        public Microinvest.CommonLibrary.Enums.ENomenclatureStatuses Status
        {
            get => this.status;
            set => this.RaiseAndSetIfChanged(ref this.status, value);
        }

        /// <summary>
        /// Casts SearchService.Models.CompanyModel object to PartnerModel.
        /// </summary>
        /// <param name="company">Partner data.</param>
        /// <date>17.03.2022.</date>
        public static implicit operator PartnerModel(Microinvest.SearchService.Models.CompanyModel company)
        {
            PartnerModel partner = new PartnerModel()
            {
                Name = company.Name,
                TaxNumber = company.TaxNumber,
                VATNumber = company.VatNumber,
                City = company.City,
                Address = company.Address,
                Principal = company.Principal,
            };

            return partner;
        }

        /// <summary>
        /// Casts PartnerModel object to Microinvest.PDFCreator.Models.CompanyModel.
        /// </summary>
        /// <param name="partner">Data of partner.</param>
        /// <date>17.03.2022.</date>
        public static explicit operator Microinvest.PDFCreator.Models.CompanyModel(PartnerModel partner)
        {
            Microinvest.PDFCreator.Models.CompanyModel company = new Microinvest.PDFCreator.Models.CompanyModel();
            company.Name = partner.Name;
            company.Address = string.Format("{0}, {1}", partner.City, partner.Address);
            company.Principal = partner.Principal;
            company.TaxNumber = partner.TaxNumber;
            company.Phone = partner.Phone;
            company.VATNumber = partner.VATNumber;

            return company;
        }

        /// <summary>
        /// Casts PartnerModel object to DataBase.My100REnteties.Partners.Partner.
        /// </summary>
        /// <param name="partner">Data of partner.</param>
        /// <date>25.03.2022.</date>
        public static explicit operator DataBase.Entities.Partners.Partner(PartnerModel partner)
        {
            DataBase.Entities.Partners.Partner entityPartner = DataBase.Entities.Partners.Partner.Create(
                partner.Name,
                partner.Principal,
                partner.City,
                partner.Address,
                partner.Phone,
                partner.TaxNumber,
                partner.VATNumber,
                partner.BankName,
                partner.BankBIC,
                partner.IBAN,
                partner.DiscountCardNumber,
                partner.Email,
                (DataBase.Entities.PartnersGroups.PartnersGroup)partner.Group);

            return entityPartner;
        }

        /// <summary>
        /// Casts DataBase.My100REnteties.Partners.Partner object to PartnerModel.
        /// </summary>
        /// <param name="entityPartner">Data of partner from database.</param>
        /// <date>25.03.2022.</date>
        public static implicit operator PartnerModel(DataBase.Entities.Partners.Partner entityPartner)
        {
            PartnerModel partner = new PartnerModel();

            if (entityPartner != null)
            {
                partner.Id = entityPartner.Id;
                partner.Name = entityPartner.Company;
                partner.Principal = entityPartner.Principal;
                partner.City = entityPartner.City;
                partner.Address = entityPartner.Address;
                partner.Phone = entityPartner.Phone;
                partner.Email = entityPartner.Email;
                partner.TaxNumber = entityPartner.TaxNumber;
                partner.VATNumber = entityPartner.VATNumber;
                partner.BankName = entityPartner.BankName;
                partner.BankBIC = entityPartner.BankBic;
                partner.IBAN = entityPartner.IBAN;
                partner.DiscountCardNumber = entityPartner.DiscountCard;
                partner.Group = (GroupModel)entityPartner.Group;
                partner.Status = entityPartner.Status;
            }

            return partner;
        }
    }
}
