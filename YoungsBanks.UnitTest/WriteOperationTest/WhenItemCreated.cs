using System;
using System.Collections.Generic;
using System.Text;
using CQRSlite.Events;
using CQRSlite.Tests.Extensions.TestHelpers;
using YoungsCQRS.WriteModel.Handlers;
using Xunit;
using YoungsCQRS.WriteModel.Domain;
using YoungsCQRS.WriteModel.Commands;
using YoungsCQRS.ReadModel.Events;
using System.Linq;

namespace YoungsBanks.UnitTest.WriteOperationTest
{
    public class WhenItemCreated : Specification<AccountWM, AccountCommandHandler, CreateCommand>
    {
        private Guid _id;
        protected override AccountCommandHandler BuildHandler()
        {
            return new AccountCommandHandler(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _id = Guid.NewGuid();
            return new List<IEvent>();
        }

        protected override CreateCommand When()
        {
            return new CreateCommand(_id,1,111,"yh100002");
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<AccountCreateEvent>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_name()
        {
            Assert.Equal("yh100002", ((AccountCreateEvent)PublishedEvents.First()).Currency);
        }
    }
}
