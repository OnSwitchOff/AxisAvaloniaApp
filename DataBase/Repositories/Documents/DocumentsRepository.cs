using DataBase.Entities.Documents;
using Microinvest.CommonLibrary.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Repositories.Documents
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">Object to get data from the database or set data to database.</param>
        public DocumentsRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        /// <summary>
        /// GetDocumentsByOperationHeader.
        /// </summary>
        /// <returns>Document</returns>
        /// <date>06.07.2022.</date>
        public async Task<Document?> GetDocumentsByOperationHeaderAsync(Entities.OperationHeader.OperationHeader OperationHeader, EDocumentTypes DocumentType)
        {
            return await Task.Run(async () =>
            {
                return databaseContext.
                     Documents.
                     Where(doc => doc.OperationHeader.Id == OperationHeader.Id && doc.DocumentType == DocumentType).FirstOrDefault();
            });
        }
    }
}
