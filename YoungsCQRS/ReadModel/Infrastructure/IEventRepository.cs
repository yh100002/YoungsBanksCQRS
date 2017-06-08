using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using CQRSlite.Events;
using YoungsCQRS.ReadModel.Dtos;

namespace YoungsCQRS.ReadModel.Infrastructure
{
    public interface IEventRepository
    {
        bool TryGetValue(Guid eventid, out List<IEvent> values);

        void Add(Guid eventid, List<IEvent> values);
    }
}
