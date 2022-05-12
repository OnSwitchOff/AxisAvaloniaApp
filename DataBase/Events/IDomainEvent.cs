using MediatR;
using System;

namespace DataBase.Events
{
    public interface IDomainEvent : INotification
    {
        /// <summary>
        /// Date and time, when the event was raised.
        /// </summary>
        DateTime OccurredOn { get; }
    }
}
