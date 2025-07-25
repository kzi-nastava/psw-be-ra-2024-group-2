using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class User : Entity
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; set; }
    public decimal BonusPoints { get; private set; }
    public List<TourCategory> Interests { get; private set; }

    public User(string username, string password, UserRole role, bool isActive)
    {
        Username = username;
        Password = password;
        Role = role;
        IsActive = isActive;
        BonusPoints = 0;
        Validate();
    }

    public User(string username, string password, UserRole role, bool isActive, List<TourCategory> interests)
    {
        Username = username;
        Password = password;
        Role = role;
        IsActive = isActive;
        BonusPoints = 0;
        Interests = interests;
        Validate();
    }

    public User(string username, string password, UserRole role, bool isActive, decimal bonusPoints)
    {
        Username = username;
        Password = password;
        Role = role;
        IsActive = isActive;
        BonusPoints = bonusPoints;
        Validate();
    }

    public static List<TourCategory> ConvertStringToTourCategories(string interestsString)
    {
        return interestsString
            .Split(',', StringSplitOptions.RemoveEmptyEntries) // Split by comma
            .Select(s => Enum.TryParse<TourCategory>(s, out var category) ? category : (TourCategory?)null) // Convert to Enum
            .Where(c => c.HasValue) // Filter valid values
            .Select(c => c.Value) // Convert Nullable<TourCategory> to TourCategory
            .ToList();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username)) throw new ArgumentException("Invalid Name");
        if (string.IsNullOrWhiteSpace(Password)) throw new ArgumentException("Invalid Surname");
    }

    public string GetPrimaryRoleName()
    {
        return Role.ToString().ToLower();
    }
}

public enum UserRole
{
    Administrator,
    Author,
    Tourist
}

public enum TourCategory
{
    Nature,      // Priroda
    Art,         // Umetnost
    Sports,      // Sport
    Shopping,    // Šoping
    Food         // Hrana
}
