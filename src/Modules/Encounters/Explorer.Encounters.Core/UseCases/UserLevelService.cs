using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Encounters.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.UseCases
{
    public class UserLevelService : IUserLevelService
    {
        private readonly IUserLevelRepository _userLevelRepository;

        public UserLevelService(IUserLevelRepository userLevelRepository)
        {
            _userLevelRepository = userLevelRepository;
        }

        // Create a new UserLevel
        public Result<UserLevelDto> CreateUserLevel(UserLevelDto userLevelDto)
        {
            try
            {
                var userLevel = new UserLevel
                {
                    UserId = userLevelDto.UserId,
                    Xp = userLevelDto.Xp,
                    Level = userLevelDto.Level,
                };

                _userLevelRepository.AddUserLevel(userLevel);

                return Result.Ok(new UserLevelDto
                {
                    Id = userLevel.Id,
                    UserId = userLevel.UserId,
                    Xp = userLevel.Xp,
                    Level = userLevel.Level,
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to create user level: {ex.Message}");
            }
        }

        // Update an existing UserLevel
        public Result<UserLevelDto> UpdateUserLevel(UserLevelDto userLevelDto)
        {
            var existingUserLevel = _userLevelRepository.GetById(userLevelDto.Id);
            if (existingUserLevel == null)
            {
                return CreateUserLevel(userLevelDto);
            }
            else
            {

                existingUserLevel.Xp = userLevelDto.Xp;
                existingUserLevel.Level = userLevelDto.Level;

                _userLevelRepository.UpdateUserLevel(existingUserLevel);

                return Result.Ok(new UserLevelDto
                {
                    Id = existingUserLevel.Id,
                    UserId = existingUserLevel.UserId,
                    Xp = existingUserLevel.Xp,
                    Level = existingUserLevel.Level,
                });
            }
        }

        // Retrieve UserLevel by Id
        public Result<UserLevelDto> GetUserLevelById(long id)
        {
            var userLevel = _userLevelRepository.GetById(id);
            if (userLevel == null)
            {
                return Result.Fail("User level not found.");
            }

            return Result.Ok(new UserLevelDto
            {
                Id = userLevel.Id,
                UserId = userLevel.UserId,
                Xp = userLevel.Xp,
                Level = userLevel.Level,
            });
        }

        // Get all UserLevels
        public Result<List<UserLevelDto>> GetAllUserLevels()
        {
            var userLevels = _userLevelRepository.GetAllUserLevels();
            var userLevelDtos = new List<UserLevelDto>();

            foreach (var userLevel in userLevels)
            {
                userLevelDtos.Add(new UserLevelDto
                {
                    Id = userLevel.Id,
                    UserId = userLevel.UserId,
                    Xp = userLevel.Xp,
                    Level = userLevel.Level,
                });
            }

            return Result.Ok(userLevelDtos);
        }
    }
}
