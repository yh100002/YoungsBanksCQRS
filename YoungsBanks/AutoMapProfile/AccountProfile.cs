using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using CQRSlite.Commands;
using YoungsBanks.Requests;
using YoungsCQRS.WriteModel.Commands;
using YoungsCQRS.ReadModel.Events;
using YoungsCQRS.ReadModel.Dtos;

namespace YoungsBanks.AutoMapProfile
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            /*
            CreateMap<CreateAccountRequest, CreateCommand>()
                .ConstructUsing(x => new CreateCommand(Guid.NewGuid(), x.AccountNumber, x.Amount, x.Currency));
                */
            /*
            CreateMap<DepositAccountRequest, DepositCommand>()
               .ConstructUsing(x => new DepositCommand(Guid.NewGuid(), x.AccountNumber, x.Amount, x.Currency));

            CreateMap<WithdrawAccountRequest, WithdrawCommand>()
               .ConstructUsing(x => new WithdrawCommand(Guid.NewGuid(), x.AccountNumber, x.Amount, x.Currency));

            CreateMap<BalanceAccountRequest, BalanceCommand>()
               .ConstructUsing(x => new BalanceCommand(Guid.NewGuid(), x.AccountNumber));
               */

            //CreateMap<AccountCreateEvent, Account>()
            //   .ForMember(dest => dest.AggregateID, opt => opt.MapFrom(src => src.Id));

            /*
            CreateMap<AccountDepositEvent, AccountRM>()
               .ForMember(dest => dest.AggregateID, opt => opt.MapFrom(src => src.Id));

            CreateMap<AccountWithdrawEvent, AccountRM>()
               .ForMember(dest => dest.AggregateID, opt => opt.MapFrom(src => src.Id));

            CreateMap<AccountBalanceEvent, AccountRM>()
               .ForMember(dest => dest.AggregateID, opt => opt.MapFrom(src => src.Id));
               */
        }
    }
}
