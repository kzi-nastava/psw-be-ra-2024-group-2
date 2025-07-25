using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly ICrudRepository<Person> _personRepository;

    public AuthenticationService(IUserRepository userRepository, ICrudRepository<Person> personRepository, ITokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _personRepository = personRepository;
    }

    public Result<AuthenticationTokensDto> Login(CredentialsDto credentials)
    {
        Console.WriteLine($"Username: {credentials.Username}");


        var user = _userRepository.GetActiveByName(credentials.Username);
        
        Console.WriteLine($"Username: {user?.Username}");


        if (user == null || credentials.Password != user.Password) return Result.Fail(FailureCode.NotFound);

        long personId;
        try
        {
            personId = _userRepository.GetPersonId(user.Id);
        }
        catch (KeyNotFoundException)
        {
            personId = 0;
        }
        return _tokenGenerator.GenerateAccessToken(user, personId);
    }

    public Result<AuthenticationTokensDto> RegisterTourist(AccountRegistrationDto account)
    {
        if(_userRepository.Exists(account.Username)) return Result.Fail(FailureCode.NonUniqueUsername);

        try
        {
            Console.WriteLine("Username: " + account.Username);
            Console.WriteLine("Password: " + account.Password);

            var interestsList = string.IsNullOrEmpty(account.Interests)
                ? new List<TourCategory>()
                : account.Interests
                    .Split(",")
                    .Select(e => Enum.Parse<TourCategory>(e))
                    .ToList();
            var user = _userRepository.Create(new User(account.Username, account.Password, UserRole.Author, true, interestsList));
            var person = _personRepository.Create(new Person(user.Id, account.Name, account.Surname, account.Email));
            //_userRepository.CreateWallet(user.Id);

            return _tokenGenerator.GenerateAccessToken(user, person.Id);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            // There is a subtle issue here. Can you find it?
        }
    }

    public Result<List<string>> GetUsersWithMatchingInterests(string interestsInput)
    {
        Console.Write("Get User emails");
        var x = _userRepository.GetUsersWithMatchingInterests(interestsInput);
        return x;
    }


    public Result<WalletDto> GetUserWallet(long userId)
    {
        var wallet = _userRepository.GetWallet(userId);
        if (wallet == null) return Result.Fail(FailureCode.NotFound);

        WalletDto dto = new WalletDto(wallet.UserId, wallet.Balance, wallet.BonusPoints);

        return Result.Ok(dto);
    }

}