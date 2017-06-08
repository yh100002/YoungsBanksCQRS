
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;

using YoungsCQRS.ReadModel.Dtos;

namespace YoungsCQRS.ReadModel.Infrastructure
{
    public class AccountRepository<TEntity> : IAccountRepository<TEntity> where TEntity : class
    {
        protected readonly AccountContext Context;
        public AccountRepository(AccountContext context)
        {
            Context = context;
        }

        public AccountRepository()
        { }

        public virtual TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public Account GetAccount(int accountNumber)
        {
            return AccountContext.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();
        }

        public void AddAccount(Guid id, int accountNumber, double amount, string currency, int version)
        {
            AccountContext.Accounts.Add(new Account()
            {
                AggregateID = id,
                AccountNumber = accountNumber,
                Amount = amount,
                Currency = currency,
                Version = version
            }
            );

            AccountContext.SaveChanges();
            //SaveAll();
        }

        public bool Exists(int accountNumber)
        {
            int cnt = AccountContext.Accounts.Where(x => x.AccountNumber == accountNumber).Count();

            if (cnt > 0) return true;

            return false;
        }

        public void Deposit(int accountNumber, double amount, string currency, int version)
        {
            var item = AccountContext.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();

            item.Amount += amount;

            item.Currency = currency;

            item.Version = version;

            AccountContext.SaveChanges();
            //SaveAll();
        }

        public void Withdraw(int accountNumber, double amount, string currency, int version)
        {
            var item = AccountContext.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault();

            item.Amount -= amount;

            item.Currency = currency;

            item.Version = version;

            AccountContext.SaveChanges();            
            //SaveAll();
        }

        public int GetVersion(Guid guid)
        {
            var item = AccountContext.Accounts.Where(x => x.AggregateID == guid).FirstOrDefault();

            return item.Version;
        }

        public double GetBalance(int accountNumber)
        {
            var balance = AccountContext.Accounts.Where(x => x.AccountNumber == accountNumber).FirstOrDefault().Amount;

            return balance;
        }

        public int SaveAll()
        {
            int re = 0;

            bool saveFailed;

            do
            {
                saveFailed = false;

                try
                {
                    re = AccountContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    // Update the values of the entity that failed to save from the store 
                    ex.Entries.Single().Reload();
                }
            } while (saveFailed);

            return re;
        }

        public void DeleteAll()
        {
            AccountContext.Accounts.RemoveRange(AccountContext.Accounts);

            SaveAll();
        }        

        public AccountContext AccountContext
        {
            get { return Context as AccountContext; }
        }
    }
}
