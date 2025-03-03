using System;

namespace Explorer.Stakeholders.API.Dtos
{
    public class WalletDto
    {
        public long UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal BonusPoints { get; set; }

        public WalletDto(long userId, decimal balance, decimal bonusPoints)
        {
            UserId = userId;
            Balance = balance;
            BonusPoints = bonusPoints;
        }
    }
}
