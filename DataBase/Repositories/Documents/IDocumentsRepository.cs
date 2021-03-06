using DataBase.Entities.Documents;
using Microinvest.CommonLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Repositories.Documents
{
    public interface IDocumentsRepository
    {
        /// <summary>
        /// GetDocumentsByOperationHeader.
        /// </summary>
        /// <returns>Document</returns>
        /// <date>06.07.2022.</date>
        Task<Document?> GetDocumentsByOperationHeaderAsync(Entities.OperationHeader.OperationHeader OperationHeader, EDocumentTypes DocumentType);
        Task<int> AddDocumentAsync(Document document);
        Task<string> GetNextDocumentNumberAsync(EDocumentTypes documentType);
        Task<bool> IsExistDocumentNumberAsync(string documentNumber, EDocumentTypes documentType);

        /// <summary>
        /// GetDocumentsByDatesByDates.
        /// </summary>
        /// <returns>Documents</returns>
        /// <date>14.07.2022.</date>
        Task<List<Document>> GetDocumentsByDatesAsync(DateTime from, DateTime to, EDocumentTypes DocumentType);
    }
}
