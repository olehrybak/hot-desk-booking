namespace HotDeskAPI.Entities;

public class Reservation
{
    public int ReservationId { get; set; }
    public int? DeskId { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public int UserId { get; set; }
    
    public Desk? Desk { get; set; }
    public User? User { get; set; }
}