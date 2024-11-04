using Explorer.BuildingBlocks.Core.UseCases;

public interface IShoppingCartRepository : ICrudRepository<ShoppingCart>
{
    ShoppingCart GetByUserId(long userId);
}
