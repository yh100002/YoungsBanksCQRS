using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YoungsCQRS.ReadModel.Dtos
{
    public class Account
    {
        public Guid AggregateID { get; set; }
        public int AccountNumber { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }        

        public int Version { get; set; }
    }

    public class AccountMap
    {
        public AccountMap(EntityTypeBuilder<Account> entityBuilder)
        {
            entityBuilder.HasKey(t => t.AggregateID);
            entityBuilder.Property(t => t.AccountNumber).IsRequired();
            entityBuilder.Property(t => t.Amount).IsRequired();
            entityBuilder.Property(t => t.Currency).IsRequired();            
            entityBuilder.Property(t => t.Version).IsRequired();
        }
    }
}
