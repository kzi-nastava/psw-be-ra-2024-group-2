using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Dtos;
using Explorer.Payment.API.Internal;
using Explorer.Payment.API.Public.Tourist;
using Explorer.Payment.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.UseCases.Tourist;

public class WalletService : CrudService<WalletDto, Wallet>, IWalletService_Internal
{
    private readonly ICrudRepository<Wallet> _walletRepository;

    public WalletService(ICrudRepository<Wallet> walletRepository, IMapper mapper) : base(walletRepository, mapper)
    {
        _walletRepository = walletRepository;
    }

    public Result<WalletDto>Create(long userId)
    {
        var wallet = _walletRepository.Create(new Wallet(userId));
        var walletDto = new WalletDto
        {
            UserId = wallet.UserId,
            AdventureCoinsBalance = wallet.AdventureCoinsBalance
        };

        return Result.Ok(walletDto);

    }
    public Result<WalletDto> GetWallet(long userId)
    {
        var wallet = _walletRepository.GetPaged(1, int.MaxValue).Results
                      .FirstOrDefault(w => w.UserId == userId);

        if (wallet == null)
        {
            return Result.Fail("Wallet not found for the given user ID.");
        }

        var walletDto = new WalletDto
        {
            UserId = wallet.UserId,
            AdventureCoinsBalance = wallet.AdventureCoinsBalance
        };

        return Result.Ok(walletDto);
    }

    public Result AddFunds(long userId, long amount)
    {
        if (amount <= 0)
        {
            return Result.Fail("Amount must be greater than zero.");
        }

        var wallet = _walletRepository.GetPaged(1, int.MaxValue).Results
                      .FirstOrDefault(w => w.UserId == userId);

        if (wallet == null)
        {
            return Result.Fail("Wallet not found for the given user ID.");
        }

        wallet.AddFunds(amount);
        _walletRepository.Update(wallet);

        return Result.Ok();
    }
}
