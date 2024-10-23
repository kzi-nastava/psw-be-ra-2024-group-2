using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.UseCases;

public class AccountService : CrudService<AccountDto, Person>, IAccountService
{
    private readonly ICrudRepository<Person> _personRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService(ICrudRepository<Person> personRepository,
        IUserRepository userRepository,
        ITransactionRepository transactionRepository,
        IMapper mapper) 
        : base(personRepository, mapper)
    {
        _personRepository = personRepository;
        _userRepository = userRepository;
        _transactionRepository = transactionRepository;
    }
    public Result<PagedResult<AccountDto>> GetPagedTourists(int page, int pageSize)
    {
        var pagedResult = _personRepository.GetPaged(page, pageSize);
        var filteredResults = pagedResult.Results
            .Where(t => t.User.Role == UserRole.Tourist)
            .ToList();
        var mappedResults = filteredResults.Select(person => new AccountDto
        {
            UserId = person.UserId,
            Username = person.User.Username,
            Email = person.Email,
            Role = (int)person.User.Role,
            IsBlocked = person.User.IsBlocked
        }).ToList();
        var filteredPagedResult = new PagedResult<AccountDto>(mappedResults, pagedResult.TotalCount);

        return Result.Ok(filteredPagedResult);
    }

    public Result<PagedResult<AccountDto>> GetPaged(int page, int pageSize)
    {
        var accounts = _personRepository.GetPaged(page, pageSize);
        return MapToDto(accounts);
    }

    public Result<AccountDto> Get(long personId)
    {
        try
        {
            var person = _personRepository.Get(personId);
            return MapToDto(person);
        }
        catch(KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<AccountDto> Block(long personId)
    {
        try
        {
            var person = _personRepository.Get(personId);
            if (person.User == null)
            {
                throw new ArgumentException("User associated with person not found");
            }
            var user = person.User;

            _transactionRepository.BeginTransaction();

            person.User.IsBlocked = true;
            user.IsBlocked = true;

            var newPerson = _personRepository.Update(person);
            _userRepository.Update(user);

            if (newPerson.Id != personId)
            {
                throw new ArgumentException("PersonId does not match the updated person");
            }

            _transactionRepository.CommitTransaction();
            return MapToDto(newPerson);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
        catch (ArgumentException e)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
        }
    }

}
