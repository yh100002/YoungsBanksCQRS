using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace YoungsCQRS.ReadModel.Events
{
    public class BaseEvent : IEvent
    {
        public Guid Id { get; set; }

        /// <summary>
        /// The Version of the Aggregate which results from this event
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// The UTC time when this event occurred.
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }
    }
}
