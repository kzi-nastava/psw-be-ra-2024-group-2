using Explorer.BuildingBlocks.Core.Domain;
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
    public class TourExecutionRepository<Context> : ITourExecutionRepository where Context : DbContext
    {
        private readonly ToursContext _dbContext;
        private readonly DbSet<TourExecution> _dbSet;

        public TourExecutionRepository(ToursContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TourExecution>();
        }

        public TourExecution Get(int id)
        {
            return _dbContext.TourExecutions.Where(t => t.Id == id)
                .FirstOrDefault();
        }
        public TourExecution Create(TourExecution execution)
        {
            _dbContext.TourExecutions.Add(execution);
            _dbContext.SaveChanges();
            return execution;
        }

        public TourExecution Update(TourExecution aggregateRoot)
        {
            _dbContext.Entry(aggregateRoot).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return aggregateRoot;
        }
        public PagedResult<TourExecution> GetPaged(int page, int pageSize)
        {
            var task = _dbSet.IncludeRelatedEntities().GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }
    }
}
