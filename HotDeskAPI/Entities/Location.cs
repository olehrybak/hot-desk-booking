namespace HotDeskAPI.Entities;

public class Location
{
    public int LocationId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Desk>? Desk { get; set; }
}