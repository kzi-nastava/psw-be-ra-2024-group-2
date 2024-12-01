using Explorer.Payment.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.API.Internal
{
    public interface IWalletService_Internal
    {
        public Result<WalletDto> GetWallet(long userId);
        public Result AddFunds(long userId, long amount);
        public Result<WalletDto> Create(long userId);
    }
}
