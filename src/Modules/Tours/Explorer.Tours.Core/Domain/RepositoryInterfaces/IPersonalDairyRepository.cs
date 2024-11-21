using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface IPersonalDairyRepository
    {
        public PersonalDairy Get(int id);
        public PersonalDairy Create(PersonalDairy dairy);
        public PersonalDairy Update(PersonalDairy aggregateRoot);
        public PagedResult<PersonalDairy> GetPaged(int page, int pageSize);
        public PersonalDairy GetByUserId(int userId);
        IEnumerable<PersonalDairy> GetAllCompletedByUserId(long userId);
        PersonalDairy GetById(long personalDairyId);
    }
}
