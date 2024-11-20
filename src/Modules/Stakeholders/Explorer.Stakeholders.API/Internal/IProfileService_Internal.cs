using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Internal;

public interface IProfileService_Internal
{
    public Result<PersonDto> UpdateTouristPosition(int userId, double latitude, double longitude);
    public Result<PersonDto> GetPositionByUserId(int userId);
    public Result<PersonDto> GetPerson(long userId);
    public Result<AccountDto> GetAccount(long userId);
    public Result<AccountImageDto> GetAccountImage(long userId);
    public Result<List<AccountImageDto>> GetManyAccountImage(List<long> userIds);
}
