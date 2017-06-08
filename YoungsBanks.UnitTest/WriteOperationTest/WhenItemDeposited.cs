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
    public class WhenItemDeposited : Specification<AccountWM, AccountCommandHandler, DepositCommand>
    {
        private Guid _guid;
        protected override AccountCommandHandler BuildHandler()
        {
            return new AccountCommandHandler(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _guid = Guid.NewGuid();
            return new List<IEvent> { new AccountCreateEvent(_guid, 1, 10000, "yh100002") { Version =1 }
            , new AccountDepositEvent(_guid, 1, 20000, "yh100002") { Version = 2 } };
        }

        protected override DepositCommand When()
        {
            return new DepositCommand(_guid, 1, 20000, "yh100002",2);
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<AccountDepositEvent>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_amount()
        {
            Assert.Equal(20000, ((AccountDepositEvent)PublishedEvents.First()).Amount);
        }
    }
}
