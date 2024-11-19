using Explorer.BuildingBlocks.Core.Domain.Enums;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class PersonalDairyRepository<Context> : IPersonalDairyRepository where Context : DbContext
    {

        private readonly ToursContext _dbContext;
        private readonly DbSet<PersonalDairy> _dbSet;


        public PersonalDairyRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<PersonalDairy>();
        }

        public PersonalDairy Get(int id)
        {
            return _dbContext.PersonalDairys.Where(d => d.Id == id)
                .FirstOrDefault();
        }
        public PersonalDairy Create(PersonalDairy execution)
        {
            _dbContext.PersonalDairys.Add(execution);
            _dbContext.SaveChanges();
            return execution;
        }
        public PersonalDairy Update(PersonalDairy aggregateRoot)
        {
            _dbContext.Entry(aggregateRoot).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return aggregateRoot;
        }

        public PagedResult<PersonalDairy> GetPaged(int page, int pageSize)
        {
            var task = _dbSet.IncludeRelatedEntities().GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public PersonalDairy GetByUserId(int userId)
        {
            return _dbContext.PersonalDairys.Where(t => t.UserId == userId ).FirstOrDefault();
        }
    }
}
