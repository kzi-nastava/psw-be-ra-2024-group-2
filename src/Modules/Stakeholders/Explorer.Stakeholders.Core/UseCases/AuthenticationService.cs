﻿using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payment.API.Internal;
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
    private readonly IWalletService_Internal _walletService;

    public AuthenticationService(IUserRepository userRepository, ICrudRepository<Person> personRepository, IWalletService_Internal walletService, ITokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
        _personRepository = personRepository;
        _walletService = walletService;
    }

    public Result<AuthenticationTokensDto> Login(CredentialsDto credentials)
    {
        var user = _userRepository.GetActiveByName(credentials.Username);
        if (user == null || credentials.Password != user.Password) return Result.Fail(FailureCode.NotFound);
        if (user.IsBlocked) return Result.Fail(FailureCode.Forbidden).WithError("User has been blocked");

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

    public Result<AuthenticationTokensDto> RegisterUser(AccountRegistrationDto account)
    {
        if(_userRepository.Exists(account.Username)) return Result.Fail(FailureCode.NonUniqueUsername);

        try
        {
            var user = _userRepository.Create(new User(account.Username, account.Password, account.UserRole, true));
            var person = _personRepository.Create(new Person(user.Id, account.Name, account.Surname, account.Email));
            var wallet = _walletService.Create(user.Id);

            return _tokenGenerator.GenerateAccessToken(user, person.Id);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            // There is a subtle issue here. Can you find it?
        }
    }
}