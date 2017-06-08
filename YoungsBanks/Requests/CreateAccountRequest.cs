using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;
using YoungsCQRS.ReadModel.Dtos;
using YoungsCQRS.ReadModel.Infrastructure;

namespace YoungsBanks.Requests
{
    public class CreateAccountRequest
    {
        public int AccountNumber { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }      
    }

    public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator(IAccountRepository<Account> accountRepo)
        {

            RuleFor(x => x.AccountNumber).Must(x => !accountRepo.Exists(x)).WithMessage("An AccountNumber with this Number already exists.");

            RuleFor(x => x.AccountNumber).NotNull().NotEmpty().WithMessage("The AccountNumber cannot be blank.");

            RuleFor(x => x.Amount).NotNull().NotEmpty().WithMessage("The Amount cannot be blank.");

            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("The Amount cannot be less than 0.");

            RuleFor(x => x.Currency).NotNull().NotEmpty().WithMessage("The Currency cannot be blank.");

            RuleFor(x => x.Currency).Length(2).WithMessage("The Length of Currency must be 2.");

        }
    }


}
