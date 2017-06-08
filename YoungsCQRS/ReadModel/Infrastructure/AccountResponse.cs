using System;
using System.Collections.Generic;
using System.Text;

namespace YoungsCQRS.ReadModel.Infrastructure
{
    public class AccountResponse
    {
        public int AccountNumber { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

    }
}
