using Microinvest.CommonLibrary.Enums;
using System;

namespace DataBase.Entities.Documents
{
    public class Document : Entity
    {
        public int Id { get; set; }
        public OperationHeader.OperationHeader OperationHeader { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public EDocumentTypes DocumentType { get; set; }
        public DateTime TaxDate { get; set; }
        public string SourceDocumentNumber { get; set; }
        public DateTime SourceDocumentDate { get; set; }
        public string RecipientName { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public string DealDescription { get; set; } = null!;
        public string DealLocation { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        internal Document() : this(null, "", DateTime.Now, EDocumentTypes.Unknown, DateTime.Now, "", DateTime.Now, "", "", "", "")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="operHeader">The operation that is the base for the current document.</param>
        /// <param name="docNumber">Number of the document.</param>
        /// <param name="docDate">Date of the document.</param>
        /// <param name="docType">Type of the document.</param>
        /// <param name="taxDate">Date of payment.</param>
        /// <param name="srcDocNumber">The document that is the base for the current document.</param>
        /// <param name="srcDocDate">The date of document that is the base for the current document.</param>
        /// <param name="recipientName">Name of the document recipient.</param>
        /// <param name="creatorName">Name of the document creator.</param>
        /// <param name="dealDescr">Description of the deal.</param>
        /// <param name="dealLocation">Location of the deal.</param>
        private Document(OperationHeader.OperationHeader operHeader, string docNumber, DateTime docDate, EDocumentTypes docType, DateTime taxDate, string srcDocNumber, DateTime srcDocDate, string recipientName, string creatorName, string dealDescr, string dealLocation)
        {
            this.OperationHeader = operHeader;
            this.DocumentNumber = docNumber;
            this.DocumentDate = docDate;
            this.DocumentType = docType;
            this.TaxDate = taxDate;
            this.SourceDocumentNumber = srcDocNumber;
            this.SourceDocumentDate = srcDocDate;
            this.RecipientName = recipientName;
            this.CreatorName = creatorName;
            this.DealDescription = dealDescr;
            this.DealLocation = dealLocation;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="operHeader">The operation that is the base for the current document.</param>
        /// <param name="docNumber">Number of the document.</param>
        /// <param name="docDate">Date of the document.</param>
        /// <param name="docType">Type of the document.</param>
        /// <param name="taxDate">Date of payment.</param>
        /// <param name="srcDocNumber">The document that is the base for the current document.</param>
        /// <param name="srcDocDate">The date of document that is the base for the current document.</param>
        /// <param name="recipientName">Name of the document recipient.</param>
        /// <param name="creatorName">Name of the document creator.</param>
        /// <param name="dealDescr">Description of the deal.</param>
        /// <param name="dealLocation">Location of the deal.</param>
        /// <returns>Returns <see cref="Document"/> class if parameters are correct.</returns>
        public static Document Create(OperationHeader.OperationHeader operHeader, string docNumber, DateTime docDate, EDocumentTypes docType, DateTime taxDate, string srcDocNumber, DateTime srcDocDate, string recipientName, string creatorName, string dealDescr, string dealLocation)
        {
            // check rule
            //CheckRule();

            return new Document(operHeader, docNumber, docDate, docType, taxDate, srcDocNumber, srcDocDate, recipientName, creatorName, dealDescr, dealLocation);
        }
    }
}
