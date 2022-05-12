using Microinvest.CommonLibrary.Enums;
using Microinvest.PDFCreator.Models;
using System.Collections.Generic;
using AxisAvaloniaApp.Enums;
using AxisAvaloniaApp.Models;

namespace AxisAvaloniaApp.Services.Document
{
    /// <summary>
    /// Describes service to generate pdf document.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Gets or sets data to generate document.
        /// </summary>
        /// <date>18.03.2022.</date>
        System.Data.DataTable ItemsData { get; set; }

        /// <summary>
        /// Gets or sets width of the columns of the utem data table.
        /// </summary>
        /// <date>18.03.2022.</date>
        double[] ItemsTableColumnsWidth { get; set; }

        /// <summary>
        /// Gets or sets path with logo of company.
        /// </summary>
        /// <date>18.03.2022.</date>
        string CompanyLogoPath { get; set; }

        /// <summary>
        /// Gets or sets path with signature of company.
        /// </summary>
        /// <date>18.03.2022.</date>
        string CompanySignaturePath { get; set; }

        /// <summary>
        /// Sets data of customer.
        /// </summary>
        /// <date>18.03.2022.</date>
        PartnerModel CustomerData { set; }

        /// <summary>
        /// Gets or sets data of a document fields.
        /// </summary>
        /// <date>18.03.2022.</date>
        DocumentModel DocumentDescription { get; set; }

        /// <summary>
        /// Gets or sets parameters of a document page.
        /// </summary>
        /// <date>18.03.2022.</date>
        DocumentPageModel PageParameters { get; set; }

        /// <summary>
        /// Generates report.
        /// </summary>
        /// <returns>Returns true if a report was generated successfully; otherwise returns false.</returns>
        /// <date>18.03.2022.</date>
        bool GenerateReport();

        /// <summary>
        /// Generates invoice.
        /// </summary>
        /// <param name="versionPrinting">Versions of document to print.</param>
        /// <param name="paymentType">Order payment type.</param>
        /// <returns>Returns true if an invoice was generated successfully; otherwise returns false.</returns>
        /// <date>18.03.2022.</date>
        bool GenerateInvoice(EDocumentVersionsPrinting versionPrinting = EDocumentVersionsPrinting.Original, EPaymentTypes paymentType = EPaymentTypes.Cash);

        /// <summary>
        /// Generates debit note.
        /// </summary>
        /// <param name="versionPrinting">Versions of document to print.</param>
        /// <param name="paymentType">Order payment type.</param>
        /// <returns>Returns true if a debit note was generated successfully; otherwise returns false.</returns>
        /// <date>18.03.2022.</date>
        bool GenerateDebitNote(EDocumentVersionsPrinting versionPrinting = EDocumentVersionsPrinting.Original, EPaymentTypes paymentType = EPaymentTypes.Cash);

        /// <summary>
        /// Generates credit note.
        /// </summary>
        /// <param name="versionPrinting">Versions of document to print.</param>
        /// <param name="paymentType">Order payment type.</param>
        /// <returns>Returns true if a credit note was generated successfully; otherwise returns false.</returns>
        /// <date>18.03.2022.</date>
        bool GenerateCreditNote(EDocumentVersionsPrinting versionPrinting = EDocumentVersionsPrinting.Original, EPaymentTypes paymentType = EPaymentTypes.Cash);

        /// <summary>
        /// Generates proform invoice.
        /// </summary>
        /// <param name="versionPrinting">Versions of document to print.</param>
        /// <param name="paymentType">Order payment type.</param>
        /// <returns>Returns true if a proform invoice was generated successfully; otherwise returns false.</returns>
        /// <date>18.03.2022.</date>
        bool GenerateProformInvoice(EDocumentVersionsPrinting versionPrinting = EDocumentVersionsPrinting.Original, EPaymentTypes paymentType = EPaymentTypes.Cash);

        /// <summary>
        /// Generates receipt.
        /// </summary>
        /// <param name="versionPrinting">Versions of document to print.</param>
        /// <param name="paymentType">Order payment type.</param>
        /// <returns>Returns true if a receipt was generated successfully; otherwise returns false.</returns>
        /// <date>18.03.2022.</date>
        bool GenerateReceipt(EDocumentVersionsPrinting versionPrinting = EDocumentVersionsPrinting.Original, EPaymentTypes paymentType = EPaymentTypes.Cash);

        /// <summary>
        /// Saves document.
        /// </summary>
        /// <param name="path">Path to save document.</param>
        /// <returns>Returns true if a document was saved successfully; otherwise returns false.</returns>
        /// <date>18.03.2022.</date>
        bool SaveDocument(string path);

        /// <summary>
        /// Converts document to list with images.
        /// </summary>
        /// <returns>Returns true if a document was converted to image list successfully; otherwise returns false.</returns>
        /// <date>18.03.2022.</date>
        List<System.Drawing.Image> ConvertDocumentToImageList();
    }
}
