using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly ToursContext _dbContext;

        public TourRepository(ToursContext dbContext) 
        {
            _dbContext = dbContext;
        }

    }
}
