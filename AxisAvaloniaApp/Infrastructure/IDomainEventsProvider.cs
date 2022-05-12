using DataBase.Events;
using System.Collections.Generic;

namespace AxisAvaloniaApp.Infrastructure
{
    public interface IDomainEventsProvider
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

        void ClearAllDomainEvents();
    }
}
