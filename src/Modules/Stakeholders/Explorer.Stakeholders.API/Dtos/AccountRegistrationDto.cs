using Explorer.BuildingBlocks.Core.Domain.Enums;

namespace Explorer.Stakeholders.API.Dtos;

public class AccountRegistrationDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public UserRole UserRole { get; set; }
}