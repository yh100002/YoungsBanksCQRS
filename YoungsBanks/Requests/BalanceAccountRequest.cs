using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoungsCQRS.ReadModel.Dtos;
using YoungsCQRS.ReadModel.Infrastructure;

namespace YoungsBanks.Requests
{
    public class BalanceAccountRequest
    {
        public int AccountNumber { get; set; }
    }

    public class BalanceAccountRequestValidator : AbstractValidator<BalanceAccountRequest>
    {
        public BalanceAccountRequestValidator(IAccountRepository<Account> accountRepo)
        {
            RuleFor(x => x.AccountNumber).NotNull().NotEmpty().WithMessage("The AccountNumber cannot be blank.");          

        }
    }
}
