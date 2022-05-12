namespace AxisAvaloniaApp.Infrastructure
{
    public interface IDomainEventsDispatcher
    {
        /// <summary>
        /// Forwards all the <see cref="BuildingBlocks.Domain.Entities.Entity.DomainEvents"/> to the application layer through the SQRS mechanism.
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous dispatching operation.</returns>
        System.Threading.Tasks.Task DispatchEventsAsync(System.Threading.CancellationToken cancellationToken = default);
    }
}
