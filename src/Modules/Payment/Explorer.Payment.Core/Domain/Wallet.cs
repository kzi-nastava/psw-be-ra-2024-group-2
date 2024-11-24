using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain;

public class Wallet : Entity
{
    public long UserId { get; set; }
    public double AdventureCoinsBalance { get; set; }

    public Wallet(long userId)
    {
        UserId = userId;
        AdventureCoinsBalance = 0;
    }

    public void AddFunds(double amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive");
        AdventureCoinsBalance += amount;
    }

    public void SubtractFunds(double amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be positive");
        if (AdventureCoinsBalance < amount)
        {
            throw new InvalidOperationException("Insufficient Adventure Coins balance");
        }
        AdventureCoinsBalance -= amount;
    }

}
