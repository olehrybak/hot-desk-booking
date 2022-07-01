using HotDeskAPI.Models;

namespace HotDeskAPI.Services;

public interface IDesks
{
    public List<DeskDto> GetDesks();
    public DeskDto? DeleteDesks(int id);
    public DeskDto? AddDesk(DeskAddDto desk);
    public DeskDto? ChangeAvailability(int id);
    public List<ReservationUserDto> GetDeskReservations(int id);
    public List<DeskDto> GetDesksById(int id);
}