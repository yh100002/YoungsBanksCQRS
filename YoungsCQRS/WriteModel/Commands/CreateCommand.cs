using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using CQRSlite.Commands;

namespace YoungsCQRS.WriteModel.Commands
{
    public class CreateCommand : ICommand
    {
        public readonly int AccountNumber;

        public readonly double Amount;

        public readonly string Currency;

        public CreateCommand(Guid id, int accountNumber, double amount, string currency)
        {
            Id = id;

            AccountNumber = accountNumber;

            Amount = amount;

            Currency = currency;
        }

        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
        public int Version { get; set; }
    }
}
