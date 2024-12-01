using Explorer.Encounters.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public
{
    public interface IUserLevelService
    {
        Result<UserLevelDto> CreateUserLevel(UserLevelDto userLevelDto);
        Result<UserLevelDto> UpdateUserLevel(UserLevelDto userLevelDto);
        Result<UserLevelDto> GetUserLevelById(long id);
        Result<List<UserLevelDto>> GetAllUserLevels();
    }
}
