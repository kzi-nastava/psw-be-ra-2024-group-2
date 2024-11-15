using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.Core.Domain;

public interface IShoppingCartRepository : ICrudRepository<ShoppingCart>
{
    ShoppingCart GetByUserId(long userId);
}
