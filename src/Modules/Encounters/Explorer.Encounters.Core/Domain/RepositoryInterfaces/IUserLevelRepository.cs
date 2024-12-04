using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Core.Domain.RepositoryInterfaces
{
    public interface IUserLevelRepository
    {
        UserLevel GetById(long id);
        void AddUserLevel(UserLevel userLevel);
        void UpdateUserLevel(UserLevel userLevel);
        List<UserLevel> GetAllUserLevels();
    }
}
