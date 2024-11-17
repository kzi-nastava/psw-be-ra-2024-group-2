using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using FluentResults;
using AutoMapper;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.API.Internal;

namespace Explorer.Stakeholders.Core.UseCases;

public class PersonService : CrudService<PersonDto, Person>, IPersonService, IProfileService_Internal
{
    private readonly ICrudRepository<Person> _personRepository;

    public PersonService(ICrudRepository<Person> personRepository, IMapper mapper) : base(personRepository, mapper)
    {
        _personRepository = personRepository;
    }

    public Result<AccountDto> GetAccount(long userId)
    {
        try
        {
            var person = _personRepository.Get(userId);

            AccountDto account = new AccountDto();

            account.UserId = person.UserId;
            account.Email = person.Email;
            account.Role = person.User.Role;
            account.IsBlocked = person.User.IsBlocked;
            account.Username = person.User.Username;

            return Result.Ok(account);

        }
        catch (KeyNotFoundException ex)
        {
            return Result.Fail(FailureCode.NotFound).WithError(ex.Message);
        }
    }

    public Result<PersonDto> GetPerson(long userId)
    {
        try
        {
            var person = _personRepository.Get(userId);
            return MapToDto(person);
        }
        catch (KeyNotFoundException ex)
        {
            return Result.Fail(FailureCode.NotFound).WithError(ex.Message);
        }
    }

    public Result<PersonDto> GetPositionByUserId(int userId)
    {
        try
        {
            var person = _personRepository.Get(userId);
            return MapToDto(person);
        }
        catch (KeyNotFoundException ex)
        {
            return Result.Fail(FailureCode.NotFound).WithError(ex.Message);
        }

    }
    public Result<PersonDto> UpdateTouristPosition(int userId, double latitude, double longitude)
    {
        try
        {
            var person = _personRepository.Get(userId);

            if (person == null)
                return Result.Fail(FailureCode.NotFound).WithError("User not found");

            TouristPosition tp = new TouristPosition(latitude, longitude);

            person.UpdateTouristPosition(tp);

            _personRepository.Update(person);

            return MapToDto(person);
        }
        catch (Exception e)
        {
            return Result.Fail(FailureCode.Conflict).WithError(e.Message); //Conflict
        }
    }
}
