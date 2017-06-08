using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using CQRSlite.Events;
using YoungsCQRS.ReadModel.Dtos;
using YoungsCQRS.ReadModel.Events;

namespace YoungsCQRS.ReadModel.Infrastructure
{
    public class EventRepository : IEventRepository
    {
        protected readonly AccountContext _context;
        public EventRepository(AccountContext context)
        {
            _context = context;
        }
        public void Add(Guid eventid, List<IEvent> values)
        {
            var @event = new BaseEvent() { Id = eventid, Version = 1, TimeStamp = DateTimeOffset.UtcNow };
            AccountContext.EventStorage.Add(new EventStore()
            {
                Eventid = @event.Id,
                TimeStamp = @event.TimeStamp,
                Version = @event.Version
            }
            );

            values.Add(@event);

            AccountContext.SaveChanges();
            
        }

        public bool TryGetValue(Guid eventid, out List<IEvent> values)
        {            
            var list = _context.EventStorage.Where(x => x.Eventid == eventid);

            values = new List<IEvent>();

            foreach (var i in list)
            {
                BaseEvent @event = new BaseEvent() { Id=i.Eventid, Version=i.Version, TimeStamp= i.TimeStamp };

                values.Add(@event);
            }

            if (list == null) return false;

            if (values.Count() == 0)
            {
                values = null;
                return false;
            }
            
            return true;
        }

        public AccountContext AccountContext
        {
            get { return _context as AccountContext; }
        }
    }
}
