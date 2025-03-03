using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Wallet : Entity
    {
        // Properties
        public long UserId { get; private set; } // User associated with the wallet
        public decimal Balance { get; private set; } // Balance in the wallet
        public decimal BonusPoints { get; private set; } // Bonus Points in the wallet

        // Constructor to initialize a wallet for a user
        public Wallet(long userId, decimal balance = 0, decimal bonusPoints = 0)
        {
            UserId = userId;
            Balance = balance;
            BonusPoints = bonusPoints;
        }

        // Method to add funds to the wallet
        public void AddFunds(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to add must be greater than zero.");
            }

            Balance += amount;
        }

        // Method to subtract funds from the wallet
        public void DeductFunds(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to deduct must be greater than zero.");
            }

            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }

            Balance -= amount;
        }

        // Method to add bonus points to the wallet
        public void AddBonusPoints(decimal points)
        {
            if (points <= 0)
            {
                throw new ArgumentException("Points to add must be greater than zero.");
            }

            BonusPoints += points;
        }

        // Method to deduct bonus points from the wallet
        public void DeductBonusPoints(decimal points)
        {
            if (points <= 0)
            {
                throw new ArgumentException("Points to deduct must be greater than zero.");
            }

            if (points > BonusPoints)
            {
                throw new InvalidOperationException("Insufficient bonus points.");
            }

            BonusPoints -= points;
        }
    }
}
