using DataBase.Entities.Documents;
using Microinvest.CommonLibrary.Enums;
using System;
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

        /// <summary>
        /// Adds new document to table with documents.
        /// </summary>
        /// <param name="document">document to add to table with partners in the database.</param>
        /// <returns>Returns 0 if document wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>12.07.2022.</date>
        public async Task<int> AddDocumentAsync(Document document)
        {
            return await Task.Run<int>(() =>
            {
                document.OperationHeader = databaseContext.OperationHeaders.Where(oh => oh.Id == document.OperationHeader.Id).FirstOrDefault();
                databaseContext.Documents.Add(document);
                int rec = databaseContext.SaveChanges();

                return document.Id;
            });
        }

        /// <summary>
        /// Get DocumentNumber to the concrete document type.
        /// </summary>
        /// <param name="documentType">documentType for which is needed to find next  number.</param>
        /// <returns>Next DocumentNumber</returns>
        /// <date>12.07.2022.</date>
        public async Task<string> GetNextDocumentNumberAsync(EDocumentTypes documentType)
        {
            return await Task.Run(() =>
            {
                List<string> NumbersList = databaseContext.
                     Documents.
                     Where(d => d.DocumentType == documentType).
                     Select(d => d.DocumentNumber).
                     ToList();

                if (NumbersList != null && NumbersList.Count > 0)
                {
                    int max = 0;
                    foreach (string code in NumbersList)
                    {
                        if (string.IsNullOrEmpty(code))
                        {
                            continue;
                        }
                        string tmp = code.Trim();
                        if (string.IsNullOrEmpty(tmp))
                        {
                            continue;
                        }

                        if (tmp.ToCharArray().All(c => Char.IsDigit(c)))
                        {
                            int codeInt = int.Parse(tmp);
                            max = codeInt > max ? codeInt : max;
                        }
                    }
                    return (max + 1).ToString();
                }
                else
                {
                    return "1";
                }

            });
        }
    }
}
