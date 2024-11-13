using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.Domain.RepositoryInterfaces
{
    public interface ITourExecutionRepository
    {
        public TourExecution Get(int id);
        public TourExecution Create(TourExecution execution);
        public TourExecution Update(TourExecution aggregateRoot);
        public PagedResult<TourExecution> GetPaged(int page, int pageSize);
        public TourExecution GetByUserId(int userId);
    }
}
