using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CQRSlite.Events;
using YoungsCQRS.ReadModel.Events;
using YoungsCQRS.ReadModel.Infrastructure;
using YoungsCQRS.ReadModel.Dtos;

namespace YoungsCQRS.ReadModel.Handlers
{
    public class AccountEventHandler : IEventHandler<AccountCreateEvent>,
                                       IEventHandler<AccountDepositEvent>,
                                       IEventHandler<AccountWithdrawEvent>                                       
    {        

        private readonly IAccountRepository<Account> _accountRepo;

        public AccountEventHandler(IAccountRepository<Account> accountRepo)
        {  
            _accountRepo = accountRepo;
        }

        public Task Handle(AccountCreateEvent message)
        {            
            _accountRepo.AddAccount(message.Id, message.AccountNumber, message.Amount, message.Currency, message.Version);

            return Task.CompletedTask;
        }

        public Task Handle(AccountDepositEvent message)
        {
            _accountRepo.Deposit(message.AccountNumber, message.Amount, message.Currency, message.Version);

            return Task.CompletedTask;
        }

        public Task Handle(AccountWithdrawEvent message)
        {  
            _accountRepo.Withdraw(message.AccountNumber, message.Amount, message.Currency, message.Version);

            return Task.CompletedTask;
        }
    }
}
