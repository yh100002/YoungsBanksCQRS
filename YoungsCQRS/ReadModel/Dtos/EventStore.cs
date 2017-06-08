using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace YoungsCQRS.ReadModel.Dtos
{
    public class EventStore
    {
        public Int64 Id { get; set; }
        public Guid Eventid { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }

    public class EventStoreMap
    {
        public EventStoreMap(EntityTypeBuilder<EventStore> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Eventid).IsRequired();           
            entityBuilder.Property(t => t.Version).IsRequired();
            entityBuilder.Property(t => t.TimeStamp).IsRequired();
        }
    }
}
