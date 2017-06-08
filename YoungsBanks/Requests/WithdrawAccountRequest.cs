using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoungsCQRS.ReadModel.Dtos;
using YoungsCQRS.ReadModel.Infrastructure;

namespace YoungsBanks.Requests
{
    public class WithdrawAccountRequest
    {
        public int AccountNumber { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public Guid AggregateID { get; set; }

        public int Version { get; set; }
    }

    public class WithdrawAccountRequestValidator : AbstractValidator<WithdrawAccountRequest>
    {
        public WithdrawAccountRequestValidator(IAccountRepository<Account> accountRepo)
        {
            //RuleFor(x => x.AccountNumber).Must(x => accountRepo.Exists(x)).WithMessage("An AccountNumber with this Number must exists.");

            RuleFor(x => x.AccountNumber).NotNull().NotEmpty().WithMessage("The AccountNumber cannot be blank.");

            RuleFor(x => x.AccountNumber).Must(x => accountRepo.GetBalance(x) > 0).WithMessage("The Balance cannot be less than 0.");            

            RuleFor(x => x).Must(x => Math.Abs(accountRepo.GetBalance(x.AccountNumber)) >= Math.Abs(x.Amount)).WithMessage("The Amount cannot be less than the existing balance.");

            RuleFor(x => x.Amount).NotNull().NotEmpty().WithMessage("The Amount cannot be blank.");

            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("The Amount cannot be less than 0.");

            RuleFor(x => x.Currency).NotNull().NotEmpty().WithMessage("The Currency cannot be blank.");

            RuleFor(x => x.Currency).Length(2).WithMessage("The Length of Currency must be 2.");

            RuleFor(x => x.Version).NotNull().NotEmpty().WithMessage("The Version field cannot be blank!");

            RuleFor(x => x.AggregateID).NotNull().NotEmpty().WithMessage("The AggregateID field cannot be blank!");

        }
    }
}
