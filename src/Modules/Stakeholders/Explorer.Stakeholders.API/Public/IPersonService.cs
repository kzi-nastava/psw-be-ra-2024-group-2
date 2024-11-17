using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public;

public interface IPersonService
{
    public Result<PersonDto> UpdateTouristPosition(int userId, double latitude, double longitude);
    public Result<PersonDto> GetPositionByUserId(int userId);
    public Result<PersonDto> GetPerson(long userId);
    public Result<AccountDto> GetAccount(long userId);
}
