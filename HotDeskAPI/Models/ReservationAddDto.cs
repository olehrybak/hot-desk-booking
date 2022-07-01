namespace HotDeskAPI.Models;

public class ReservationAddDto
{
    public int? DeskId { get; set; }
    public string StartDay { get; set; } = string.Empty;
    public int DaysNum { get; set; }
}