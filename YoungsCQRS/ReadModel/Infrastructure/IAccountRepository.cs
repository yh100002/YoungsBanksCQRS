using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using YoungsCQRS.ReadModel.Dtos;

namespace YoungsCQRS.ReadModel.Infrastructure
{
    public interface IAccountRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        Account GetAccount(int accountNumber);



        void AddAccount(Guid id, int accountNumber, double amount, string currency,int version);

        void Deposit(int accountNumber, double amount, string currency,int version);

        void Withdraw(int accountNumber, double amount, string currency,int version);

        bool Exists(int accountNumber);

        double GetBalance(int accountNumber);

        int GetVersion(Guid guid);

        int SaveAll();

        void DeleteAll();

    }
}
