using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;

namespace DataBase.Entities.OperationHeader
{
    public class OperationHeader : Entity
    {
        public int Id { get; set; }
        public EOperTypes OperType { get; set; }
        public long Acct { get; set; }
        public DateTime Date { get; set; }
        public string USN { get; set; } = null!;
        public Partners.Partner Partner { get; set; }
        public PaymentTypes.PaymentType Payment { get; set; }
        public string Note { get; set; } = null!;
        public OperationHeader SrcDoc { get; set; }
        public EECCheckTypes EcrreceiptType { get; set; }
        public long EcrreceiptNumber { get; set; }
        public DateTime UserRealTime { get; set; }
        public EOperationModes Status { get; set; }

        public List<OperationDetails.OperationDetail> OperationDetails { get; set; } = new List<OperationDetails.OperationDetail>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationHeader"/> class.
        /// </summary>
        internal OperationHeader() : this(EOperTypes.Sale, 0, DateTime.Now, "", null, null, "", null, EECCheckTypes.Unknown, 0, DateTime.Now)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationHeader"/> class.
        /// </summary>
        /// <param name="operType">Type of the operation.</param>
        /// <param name="acct">Number of the operation.</param>
        /// <param name="date">Date of the operation.</param>
        /// <param name="uSN">Unique sale number.</param>
        /// <param name="partner">The partner who takes part in the operation.</param>
        /// <param name="payment">Type of payment by the operation.</param>
        /// <param name="note">Note of the operation.</param>
        /// <param name="srcDoc">The operation that is the base for the current operation.</param>
        /// <param name="ecrreceiptType">Type of the cash register receipt.</param>
        /// <param name="ecrreceiptNumber">Number of the cash register receipt.</param>
        /// <param name="userRealTime">The date and time the operation was completed.</param>
        private OperationHeader(EOperTypes operType, long acct, DateTime date, string uSN, Partners.Partner partner, PaymentTypes.PaymentType payment, string note, OperationHeader srcDoc, EECCheckTypes ecrreceiptType, long ecrreceiptNumber, DateTime userRealTime)
        {
            this.OperType = operType;
            this.Acct = acct;
            this.Date = date;
            this.USN = uSN;
            this.Partner = partner;
            this.Payment = payment;
            this.Note = note;
            this.SrcDoc = srcDoc;
            this.EcrreceiptType = ecrreceiptType;
            this.EcrreceiptNumber = ecrreceiptNumber;
            this.UserRealTime = userRealTime;
            this.Status = EOperationModes.Posted;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OperationHeader"/> class.
        /// </summary>
        /// <param name="operType">Type of the operation.</param>
        /// <param name="acct">Number of the operation.</param>
        /// <param name="date">Date of the operation.</param>
        /// <param name="uSN">Unique sale number.</param>
        /// <param name="partner">The partner who takes part in the operation.</param>
        /// <param name="payment">Type of payment by the operation.</param>
        /// <param name="note">Note of the operation.</param>
        /// <param name="srcDoc">The operation that is the base for the current operation.</param>
        /// <param name="ecrreceiptType">Type of the cash register receipt.</param>
        /// <param name="ecrreceiptNumber">Number of the cash register receipt.</param>
        /// <param name="userRealTime">The date and time the operation was completed.</param>
        /// <returns>Returns <see cref="OperationHeader"/> class if parameters are correct.</returns>
        public static OperationHeader Create(EOperTypes operType, long acct, DateTime date, string uSN, Partners.Partner partner, PaymentTypes.PaymentType payment, string note, OperationHeader srcDoc, EECCheckTypes ecrreceiptType, long ecrreceiptNumber, DateTime userRealTime)
        {
            // check rule

            return new OperationHeader(operType, acct, date, uSN, partner, payment, note, srcDoc, ecrreceiptType, ecrreceiptNumber, userRealTime);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="OperationHeader"/> class.
        /// </summary>
        /// <param name="operType">Type of the operation.</param>
        /// <param name="acct">Number of the operation.</param>
        /// <param name="date">Date of the operation.</param>
        /// <param name="uSN">Unique sale number.</param>
        /// <param name="partner">The partner who takes part in the operation.</param>
        /// <param name="payment">Type of payment by the operation.</param>
        /// <param name="note">Note of the operation.</param>
        /// <param name="ecrreceiptType">Type of the cash register receipt.</param>
        /// <param name="ecrreceiptNumber">Number of the cash register receipt.</param>
        /// <returns>Returns <see cref="OperationHeader"/> class if parameters are correct.</returns>
        public static OperationHeader Create(EOperTypes operType, long acct, DateTime date, string uSN, Partners.Partner partner, PaymentTypes.PaymentType payment, string note, EECCheckTypes ecrreceiptType, long ecrreceiptNumber)
        {
            // check rule

            return new OperationHeader(operType, acct, date, uSN, partner, payment, note, null, ecrreceiptType, ecrreceiptNumber, DateTime.Now);
        }
    }
}
