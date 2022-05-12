using DataBase.Enums;
using System;

namespace DataBase.Entities.ApplicationLog
{
    public class ApplicationLog : Entity
    {
        public int Id { get; set; }
        public EApplicationLogEvents Event { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime RealTime { get; set; }
        public OperationHeader.OperationHeader OperationHeader { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLog"/> class.
        /// </summary>
        internal ApplicationLog() : this(EApplicationLogEvents.ApplicationStart, "", null)
        {
            this.RealTime = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLog"/> class.
        /// </summary>
        /// <param name="_event">Type of event.</param>
        /// <param name="description">Description of the error.</param>
        /// <param name="operationHeader">The operation that was invoked the error during invocation.</param>
        private ApplicationLog(EApplicationLogEvents _event, string description, OperationHeader.OperationHeader operationHeader)
        {
            this.Event = _event;
            this.Description = description;
            this.RealTime = DateTime.Now;
            this.OperationHeader = operationHeader;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApplicationLog"/> class.
        /// </summary>
        /// <param name="_event">Type of event.</param>
        /// <param name="description">Description of the error.</param>
        /// <param name="operationHeader">The operation that was invoked the error during invocation.</param>
        /// <returns>Returns <see cref="ApplicationLog"/> class if parameters are correct.</returns>
        public static ApplicationLog Create(EApplicationLogEvents _event, string description, OperationHeader.OperationHeader operationHeader = null)
        {
            // check rule

            return new ApplicationLog(_event, description, operationHeader);
        }
    }
}
