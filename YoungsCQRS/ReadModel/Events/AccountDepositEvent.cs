using System;
using System.Collections.Generic;
using System.Text;
using CQRSlite.Events;

namespace YoungsCQRS.ReadModel.Events
{
    public class AccountDepositEvent : BaseEvent
    {
        public readonly int AccountNumber;

        public readonly double Amount;

        public readonly string Currency;

        public AccountDepositEvent(Guid id, int accountNumber, double amount, string currency)
        {
            Id = id;

            AccountNumber = accountNumber;

            Amount = amount;

            Currency = currency;
        }
    }
}
