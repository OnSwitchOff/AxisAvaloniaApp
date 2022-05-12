namespace AxisAvaloniaApp.Infrastructure
{
    public partial class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly MediatR.IMediator _mediator;
        private readonly IDomainEventsProvider _domainEventsProvider;

        public async System.Threading.Tasks.Task DispatchEventsAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            var domainEvents = _domainEventsProvider.GetAllDomainEvents();
            _domainEventsProvider.ClearAllDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
