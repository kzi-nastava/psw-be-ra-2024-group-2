using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Payment.Core.Domain.RepositoryInterfaces;

public interface IOrderItemRepository : ICrudRepository<OrderItem>
{
    OrderItem GetByShoppingCartId(long shoppingCartId);
}
