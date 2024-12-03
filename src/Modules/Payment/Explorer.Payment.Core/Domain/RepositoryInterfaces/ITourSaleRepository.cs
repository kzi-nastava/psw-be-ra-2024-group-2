using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain.RepositoryInterfaces
{
    public interface ITourSaleRepository
    {
        public TourSale Create(TourSale entity);
        public TourSale Update(long tourSaleId, TourSale entity);
        public TourSale Delete(long tourSaleId);
    }
}
