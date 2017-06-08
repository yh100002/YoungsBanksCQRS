using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using YoungsCQRS.ReadModel.Infrastructure;
using YoungsCQRS.ReadModel.Dtos;

namespace YoungsCQRS.WriteModel
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;

        private readonly Dictionary<Guid, List<IEvent>> _inMemoryDb = new Dictionary<Guid, List<IEvent>>();

        private readonly IEventRepository _eventRepo;

        public InMemoryEventStore(IEventPublisher publisher, IEventRepository sql)
        {
            _publisher = publisher;

            _eventRepo = sql;          
            
        }

        public async Task Save(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _inMemoryDb.TryGetValue(@event.Id, out var list);

                //_eventRepo.TryGetValue(@event.Id, out var list2);

                
                if (list == null)
                {
                    list = new List<IEvent>();

                    _inMemoryDb.Add(@event.Id, list);
                }
                list.Add(@event);
                /*
                if (list2 == null)
                {
                    list2 = new List<IEvent>();
                    _eventRepo.Add(@event.Id, list2);
                }
                list2.Add(@event);
                */

                await _publisher.Publish(@event);
            }
        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion)
        {
            _inMemoryDb.TryGetValue(aggregateId, out var events);

            //_eventRepo.TryGetValue(aggregateId, out var events);

            return Task.FromResult(events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>());
        }
    }
}
