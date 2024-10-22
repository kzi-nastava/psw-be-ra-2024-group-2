using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubRepository
    {
        Club Get(int id);
        Club? GetByName(string name);
        Club Create(Club club);
        Club Update(Club club);
        void Delete(int id);
    }
}
