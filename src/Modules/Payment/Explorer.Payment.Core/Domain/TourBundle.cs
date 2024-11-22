using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Payment.Core.Domain;

public sealed class TourBundle : Entity
{
    public long AuthorId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public BundleStatus Status { get; set; }
    public List<TourWithPrice> Tours { get; set; } = new();

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
        if(Tours.Count(Tours => Tours.TourStatus == TourStatus.Published) >= 2)
            Status = BundleStatus.Published;
        else throw new InvalidOperationException("A bundle must contain at least 2 published tours to be published.");
    }
}
