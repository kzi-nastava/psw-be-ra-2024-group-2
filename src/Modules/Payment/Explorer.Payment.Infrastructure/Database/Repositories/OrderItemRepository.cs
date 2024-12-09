using Explorer.Payment.Core.Domain;
using Explorer.Payment.Core.Domain.RepositoryInterfaces;

namespace Explorer.Payment.Infrastructure.Database.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly PaymentContext paymentContext;

    public OrderItemRepository(PaymentContext paymentContext)
    {
        this.paymentContext = paymentContext;
    }

    // Retrieve all OrderItems
    public List<OrderItem> GetAll()
    {
        return paymentContext.OrderItems.ToList();
    }

    // Retrieve all BundleOrderItems
    public List<BundleOrderItem> GetAllBundles()
    {
        return paymentContext.OrderItems.OfType<BundleOrderItem>().ToList();
    }

    // Retrieve all TourOrderItems
    public List<TourOrderItem> GetAllTours()
    {
        return paymentContext.OrderItems.OfType<TourOrderItem>().ToList();
    }

    // Retrieve all souvenirs
    public List<SouvenirOrderItem> GetAllSouvenirs()
    {
        return paymentContext.OrderItems.OfType<SouvenirOrderItem>().ToList();
    }

    // Add a new OrderItem
    public void Add(OrderItem orderItem)
    {
        paymentContext.OrderItems.Add(orderItem);
        paymentContext.SaveChanges();
    }

    // Update an existing OrderItem
    public void Update(OrderItem orderItem)
    {
        var existingItem = paymentContext.OrderItems.Find(orderItem.Id);
        if (existingItem != null)
        {
            paymentContext.Entry(existingItem).CurrentValues.SetValues(orderItem);
            paymentContext.SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException("OrderItem not found.");
        }
    }

    // Delete an OrderItem by Id
    public void Delete(long id)
    {
        var orderItem = paymentContext.OrderItems.Find(id);
        if (orderItem != null)
        {
            paymentContext.OrderItems.Remove(orderItem);
            paymentContext.SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException("OrderItem not found.");
        }
    }

    public BundleOrderItem? GetBundle(long bundleId)
    {
        return paymentContext.OrderItems.OfType<BundleOrderItem>().FirstOrDefault(o => o.Id == bundleId);
    }

    public TourOrderItem? GetTour(long id)
    {
        return paymentContext.OrderItems.OfType<TourOrderItem>().FirstOrDefault(o => o.Id == id);
    }

    public SouvenirOrderItem? GetSouvenir(long id)
    {
        return paymentContext.OrderItems.OfType<SouvenirOrderItem>().FirstOrDefault(o => o.Id == id);
    }
}