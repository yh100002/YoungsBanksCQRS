using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using YoungsCQRS.ReadModel;
using YoungsCQRS.WriteModel.Commands;
using CQRSlite.Commands;
using YoungsCQRS.ReadModel.Infrastructure;
using YoungsCQRS.ReadModel.Dtos;
using YoungsBanks.Requests;
using YoungsBanks.Filters;
using CQRSlite.Domain;

namespace YoungsBanks.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ICommandSender _commandSender;
                
        private readonly IAccountRepository<Account> _repository;        

        public AccountController(ICommandSender sender, IAccountRepository<Account> repository)
        {
            _commandSender = sender;

            _repository = repository;           
        }
        

        [ServiceFilter(typeof(BadRequestActionFilter))]
        [HttpPost]        
        public async Task<Account> Create([FromBody]CreateAccountRequest request)
        {
            await _commandSender.Send(new CreateCommand(Guid.NewGuid(), request.AccountNumber, request.Amount, request.Currency));

            var account = _repository.GetAccount(request.AccountNumber);

            return account;
        }

        [ServiceFilter(typeof(BadRequestActionFilter))]
        [HttpPut]
        public async Task<Account> Withdraw([FromBody]WithdrawAccountRequest request)
        {
            await _commandSender.Send(new WithdrawCommand(request.AggregateID, request.AccountNumber, request.Amount, request.Currency, request.Version));

            var account = _repository.GetAccount(request.AccountNumber);

            account.Version = _repository.GetVersion(account.AggregateID);

            return account;
        }

        [ServiceFilter(typeof(BadRequestActionFilter))]
        [HttpPut]
        public async Task<Account> Deposit([FromBody]DepositAccountRequest request)
        {
            await _commandSender.Send(new DepositCommand(request.AggregateID, request.AccountNumber, request.Amount, request.Currency, request.Version));

            var account = _repository.GetAccount(request.AccountNumber);

            return account;
        }

        
        [HttpPost]
        public Account Balance([FromBody]int accountNumber)
        {
            var account = _repository.GetAccount(accountNumber);

            return account;
        }

        [HttpGet]        
        public IEnumerable<Account> GetAll()
        {            
            return _repository.GetAll();
        }

        [HttpDelete]        
        public ActionResult DeleteAll()
        {
            _repository.DeleteAll();

            return Ok();
        }

    }
}
