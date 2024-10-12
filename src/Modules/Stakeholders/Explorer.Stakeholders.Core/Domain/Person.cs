using Explorer.BuildingBlocks.Core.Domain;
using System.Net.Mail;

namespace Explorer.Stakeholders.Core.Domain;

public class Person : Entity
{
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string? Biography { get; set; }
    public string? Moto { get; set; }
    public long? ImageId { get; set; }

    public User User { get; set; }
    public Image? Image { get; set; }

    public Person(long userId, string name, string surname, string email)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        Email = email;
        Validate();
    }

    public Person(long userId, string name, string surname, string email, string biography, string moto, long imageId)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        Email = email;
        Biography = biography;
        Moto = moto;
        ImageId = imageId;
        Validate();
    }

    private void Validate()
    {
        if (UserId == 0) throw new ArgumentException("Invalid UserId");
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name");
        if (string.IsNullOrWhiteSpace(Surname)) throw new ArgumentException("Invalid Surname");
        if (!MailAddress.TryCreate(Email, out _)) throw new ArgumentException("Invalid Email");
    }
}