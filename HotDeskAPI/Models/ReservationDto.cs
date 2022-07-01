namespace HotDeskAPI.Models;

public class ReservationDto
{
    public int ReservationId { get; set; }
    public int? DeskId { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public int UserId { get; set; }
}