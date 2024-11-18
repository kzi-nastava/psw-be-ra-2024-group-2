using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain.RepositoryInterfaces;

public interface IShoppingCartRepository : ICrudRepository<ShoppingCart>
{
    ShoppingCart GetByUserId(long userId);
}

