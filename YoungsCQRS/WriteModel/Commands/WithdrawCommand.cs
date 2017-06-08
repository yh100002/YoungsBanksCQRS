using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CQRSlite.Commands;

namespace YoungsCQRS.WriteModel.Commands
{
    public class WithdrawCommand : ICommand
    {
        public readonly int AccountNumber;

        public readonly double Amount;

        public readonly string Currency;        

        public WithdrawCommand(Guid id, int accountNumber, double amount, string currency, int version)
        {
            Id = id;

            AccountNumber = accountNumber;

            Amount = amount;

            Currency = currency;

            ExpectedVersion = version;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
    }
}
