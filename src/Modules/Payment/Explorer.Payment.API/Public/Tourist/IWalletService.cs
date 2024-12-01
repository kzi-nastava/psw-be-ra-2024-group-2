using Explorer.Payment.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Public.Tourist;

public interface IWalletService
{
    Result<WalletDto> GetWallet(long userId);
    Result AddFunds(long userId, long amount);
}
