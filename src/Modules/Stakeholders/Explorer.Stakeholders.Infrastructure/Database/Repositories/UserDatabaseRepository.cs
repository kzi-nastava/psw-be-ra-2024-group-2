﻿using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class UserDatabaseRepository : IUserRepository
{
    private readonly StakeholdersContext _dbContext;

    public UserDatabaseRepository(StakeholdersContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(string username)
    {
        return _dbContext.Users.Any(user => user.Username == username);
    }

    public Wallet CreateWallet(long userId)
    {
        Wallet newWallet = new Wallet(userId);
        _dbContext.Wallets.Add(newWallet);
        _dbContext.SaveChanges();
        return newWallet;
    }

    public Wallet? GetWallet(long userId)
    {
        return _dbContext.Wallets.FirstOrDefault(w => w.UserId == userId);
    }


    public User? GetActiveByName(string username)
    {
        //return _dbContext.Users.FirstOrDefault(user => user.Username == username && user.IsActive);
        var y = Exists(username);

        Console.WriteLine($"Exist: {y}");

        var x = _dbContext.Users.FirstOrDefault(user => user.Username == username);
        if (x != null)
        {
            Console.WriteLine($"Username: {x.Username}, Role: {x.Role}, IsActive: {x.IsActive}");
        }
        else
        {
            Console.WriteLine("User not found.");
        }
        return _dbContext.Users.FirstOrDefault(user => user.Username == username);
    }

    public User Create(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return user;
    }

    public long GetPersonId(long userId)
    {
        var person = _dbContext.People.FirstOrDefault(i => i.UserId == userId);
        if (person == null) throw new KeyNotFoundException("Not found.");
        return person.Id;
    }
}