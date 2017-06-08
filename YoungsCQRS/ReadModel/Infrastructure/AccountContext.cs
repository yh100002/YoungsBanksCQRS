
using Microsoft.EntityFrameworkCore;

using YoungsCQRS.ReadModel.Dtos;

namespace YoungsCQRS.ReadModel.Infrastructure
{
    public class AccountContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<EventStore> EventStorage { get; set; }

        public AccountContext(DbContextOptions<AccountContext> options) :base(options)
        {
        }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new AccountMap(modelBuilder.Entity<Account>());
            new EventStoreMap(modelBuilder.Entity<EventStore>());

        }

    }
}
