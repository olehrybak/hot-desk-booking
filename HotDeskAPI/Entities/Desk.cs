namespace HotDeskAPI.Entities;

public class Desk
{
    public int DeskId { get; set; }
    public int LocationId { get; set; }
    public bool IsAvailable { get; set; }
    
    public Location? Location { get; set; }
    public ICollection<Reservation>? Reservation { get; set; }
}