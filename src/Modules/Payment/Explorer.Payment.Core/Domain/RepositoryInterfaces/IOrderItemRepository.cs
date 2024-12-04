namespace Explorer.Payment.Core.Domain.RepositoryInterfaces;

public interface IOrderItemRepository
{
    BundleOrderItem? GetBundle(long bundleId);
    List<BundleOrderItem> GetAllBundles();

    TourOrderItem? GetTour(long id);
    List<TourOrderItem> GetAllTours();

    List<OrderItem> GetAll();
    
    void Add(OrderItem orderItem);
    void Update(OrderItem orderItem);
    void Delete(long id);
}
