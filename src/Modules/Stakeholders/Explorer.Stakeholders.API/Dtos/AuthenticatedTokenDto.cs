namespace Explorer.Stakeholders.API.Dtos;

public class AuthenticatedTokenDto
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public long PersonId { get; set; }
    public string Role { get; set; }
}