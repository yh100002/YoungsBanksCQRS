using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CQRSlite.Domain;
using YoungsCQRS.ReadModel.Events;

namespace YoungsCQRS.WriteModel.Domain
{
    public class AccountWM : AggregateRoot
    {
        private int _accountNumber;

        private double _amount;

        private string _currency;

        public AccountWM() { }

        public AccountWM(Guid id, int accountNumber, double amount, string currency)
        {
            this.Id = id;

            _accountNumber = accountNumber;

            _amount = amount;

            _currency = currency;

            ApplyChange(new AccountCreateEvent(id, accountNumber, amount, currency));
        }

        public void Deposit(int accountNumber, double amount, string currency)
        {

            _accountNumber = accountNumber;

            _amount = amount;

            _currency = currency;

            ApplyChange(new AccountDepositEvent(Id, accountNumber, amount, currency));
        }

        public void Withdraw(int accountNumber, double amount, string currency)
        {
            _accountNumber = accountNumber;

            _amount = amount;

            _currency = currency;

            ApplyChange(new AccountWithdrawEvent(Id, accountNumber, amount, currency));
        }

    }
}