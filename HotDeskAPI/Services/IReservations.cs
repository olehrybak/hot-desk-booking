using HotDeskAPI.Entities;
using HotDeskAPI.Models;

namespace HotDeskAPI.Services;

public interface IReservations
{
    public List<ReservationDto> GetReservations();
    public ReservationDto? AddReservation(ReservationAddDto reservation, string accessToken);
    public ReservationDto? ModifyReservation(ReservationAddDto reservation, int id, string accessToken);
}