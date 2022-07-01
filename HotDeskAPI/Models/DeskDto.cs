namespace HotDeskAPI.Models;

public class DeskDto
{
    public int DeskId { get; set; }
    public int LocationId { get; set; }
    public bool? IsAvailable { get; set; }
}