using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Infrastructure.Database.Repositories
{
    public class TourSaleRepository : CrudDatabaseRepository<TourSale, PaymentContext>, ITourSaleRepository
    {
        private readonly PaymentContext _dbContext;

        public TourSaleRepository(PaymentContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public TourSale Create(TourSale entity)
        {
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public TourSale Update(long tourSaleId, TourSale entity)
        {
            TourSale tourSale = _dbContext.TourSales.Find(tourSaleId);
            if (tourSale == null) 
            {
                tourSale = entity;
            }
            _dbContext.SaveChanges();
            return tourSale;
        }

        public TourSale Delete(long tourSaleId)
        {
            TourSale tourSale = _dbContext.TourSales.Find(tourSaleId);

            if (tourSale == null)
            {
                throw new KeyNotFoundException($"TourSale with ID {tourSaleId} not found.");
            }
            _dbContext.TourSales.Remove(tourSale);
            _dbContext.SaveChanges();
            return tourSale;
        }

    }
}
