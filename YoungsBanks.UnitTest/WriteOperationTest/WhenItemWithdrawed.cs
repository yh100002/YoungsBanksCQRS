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
    public class WhenItemWithdrawed : Specification<AccountWM, AccountCommandHandler, WithdrawCommand>
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
            , new AccountWithdrawEvent(_guid, 1, 12, "yh100002") { Version = 2 } };
        }

        protected override WithdrawCommand When()
        {
            return new WithdrawCommand(_guid, 1, 12, "yh100002", 2);
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<AccountWithdrawEvent>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_amount()
        {
            Assert.Equal(12, ((AccountWithdrawEvent)PublishedEvents.First()).Amount);
        }
    }
}
