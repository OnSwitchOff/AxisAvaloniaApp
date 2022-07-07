using Microinvest.CommonLibrary.Enums;
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
        Task<Entities.Documents.Document?> GetDocumentsByOperationHeaderAsync(Entities.OperationHeader.OperationHeader OperationHeader, EDocumentTypes DocumentType);
    }
}
