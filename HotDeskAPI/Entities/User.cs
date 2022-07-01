namespace HotDeskAPI.Entities;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string Role { get; set; } = string.Empty;
    
    public List<Reservation>? Reservation { get; set; }
}