using System.Linq;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

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

    public List<string> GetUsersWithMatchingInterests(string interestsInput)
    {
        try
        {
            // Convert the input string to a list of TourCategory values
            var interestsList = interestsInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                              .Select(e => e.Trim()) // Trim to remove extra spaces
                                              .ToList();
            //return new List<string> { "sergej.vlaskalic@gmail.com", "sergej.vlaskalic@gmail.com" };
            // Step 1: Find the UserIDs where at least one interest matches
            Console.WriteLine("GetUserWithMAtchingInterests 1");
            Console.WriteLine("interestInput: " + interestsInput);
            Console.WriteLine("interestInput: " + interestsList.ToString());
            /*
            var matchingUserIds = _dbContext.Users
                                            .Where(u => u.Interests.Any(i => interestsList.Contains(i.ToString()))) // Make sure Interests is a collection of strings
                                            .Select(u => u.Id) // Assuming the User entity has an ID property
                                            .ToList();
            */
            List<long> matchingUserIds = new List<long>();
            var allUsers = _dbContext.Users.ToList();

            foreach (var user in allUsers)
            {
                Console.WriteLine($"User ID: {user.Id}");
                var userInterests = user.Interests.Select(i => i.ToString()).ToList(); // Convert interests to strings
                Console.WriteLine($"User Interests: {string.Join(", ", userInterests)}");
                foreach (var interest in interestsList)
                {
                    Console.WriteLine($" - Interest: {interest}");
                    if (userInterests.Contains(interest)) // Ensure correct comparison type
                    {
                        matchingUserIds.Add(user.Id);
                        break;
                    }
                }
            }
            Console.WriteLine("GetUserWithMAtchingInterests 2");
            if (!matchingUserIds.Any())
            {
                //throw new Exception("No users found with matching interests.");
                return null;
            }
            Console.WriteLine("GetUserWithMAtchingInterests 3");
            // Step 2: Find the emails from the Persons table based on the matching UserIDs
            var userEmails = _dbContext.People
                .Where(p => matchingUserIds.Contains(p.UserId)) // Filter by matching UserIds
                .Select(p => $"{p.Email}*{p.Name}*{p.Surname}") // Format as email*name*surname
                .ToList();

            Console.WriteLine("GetUserWithMAtchingInterests 4");
            return userEmails;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
            throw new Exception("Failed to fetch user emails.", ex);
        }
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