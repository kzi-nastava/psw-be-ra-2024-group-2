using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Explorer.Payment.Core.Domain;

public sealed class TourSouvenir : Entity
{
    public string Name { get; private set; } = string.Empty;
    public double Price { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int Count { get; private set; }
    public SouvenirStatus SouvenirStatus { get; private set; }

    public long ImageId { get; private set; }
    public long TourId { get; private set; }
    public long AuthorId { get; private set; }
    public Image? Image { get; set; }

    public TourSouvenir()
    {
        
    }
    public TourSouvenir(string name, double price, string description, int count, SouvenirStatus souvenirStatus, long imageId, long tourId, long authorId)
    {
        Name = name;
        Price = price;
        Description = description;
        ImageId = imageId;
        TourId = tourId;
        AuthorId = authorId;
        Count = count;
        SouvenirStatus = souvenirStatus;
    }

    public TourSouvenir(string name, double price, string description, int count, SouvenirStatus souvenirStatus, Image image, long tourId, long authorId)
    {
        Name = name;
        Price = price;
        Description = description;
        Image = image;
        TourId = tourId;
        AuthorId = authorId;
        Count = count;
        SouvenirStatus = souvenirStatus;
    }

    public void Archive()
    {
        SouvenirStatus = SouvenirStatus.Archived;
    }

    public void Publish()
    {
        SouvenirStatus = SouvenirStatus.Published;
    }

    public void UpdateCount(int count)
    {
        Count = count;
    }

    public void Update(string name, double price, string description, int count, SouvenirStatus souvenirStatus, long id)
    {
        Name = name;
        Price = price;
        Description = description;
        Count = count;
        SouvenirStatus = souvenirStatus;
        ImageId = id;
    }

    public void DecreaseCount(int count)
    {
        Count -= count;
    }
}
