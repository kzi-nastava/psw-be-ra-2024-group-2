using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Payment.Core.Domain;

public sealed class TourBundle : Entity
{
    public long AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public BundleStatus Status { get; set; }
    public List<int> Tours { get; set; } = new();

    public TourBundle() { }
    public TourBundle(long authorId, string name, double price, BundleStatus status)
    {
        AuthorId = authorId;
        Name = name;
        Price = price;
        Status = status;
    }

    public void ArchiveBundle()
    {
        Status = BundleStatus.Archived;
    }

    public void PublishBundle()
    {
        Status = BundleStatus.Published;
    }
}
