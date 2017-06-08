using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using YoungsCQRS.WriteModel.Commands;
using YoungsCQRS.WriteModel.Domain;
using YoungsCQRS.ReadModel.Infrastructure;
using YoungsCQRS.ReadModel.Dtos;

namespace YoungsCQRS.WriteModel.Handlers
{
    public class AccountCommandHandler :ICommandHandler<CreateCommand>, 
                                        ICommandHandler<DepositCommand>, 
                                        ICommandHandler<WithdrawCommand>
    {
        private readonly ISession _session;

        public AccountCommandHandler(ISession session)
        {
            _session = session;           
        }

        public async Task Handle(WithdrawCommand message)
        {
            var account = await _session.Get<AccountWM>(message.Id, message.ExpectedVersion);

            account.Withdraw(message.AccountNumber, message.Amount, message.Currency);

            await _session.Commit();
        }

        public async Task Handle(DepositCommand message)
        {
            var item = await _session.Get<AccountWM>(message.Id, message.ExpectedVersion);

            item.Deposit(message.AccountNumber, message.Amount, message.Currency);

            await _session.Commit();

        }

        public async Task Handle(CreateCommand message)
        {
            AccountWM account = new AccountWM(message.Id, message.AccountNumber, message.Amount, message.Currency);

            await _session.Add(account);

            await _session.Commit();
        }
        
    }
}
