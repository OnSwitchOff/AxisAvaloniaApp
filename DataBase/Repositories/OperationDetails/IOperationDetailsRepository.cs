namespace DataBase.Repositories.OperationDetails
{
    public interface IOperationDetailsRepository
    {
        /// <summary>
        /// Adds new record to OperationDetail table.
        /// </summary>
        /// <param name="record">Data to add.</param>
        /// <returns>Returns 0 if record wasn't added to database; otherwise returns real id of new record.</returns>
        /// <date>23.06.2022.</date>
        System.Threading.Tasks.Task<int> AddNewRecord(Entities.OperationDetails.OperationDetail record);
    }
}
