using System.IdentityModel.Tokens.Jwt;
using HotDeskAPI.Data;
using HotDeskAPI.Entities;
using HotDeskAPI.Models;

namespace HotDeskAPI.Services;

public class ReservationService : IReservations
{
    private readonly DataContext _context;
    
    public ReservationService(DataContext context)
    {
        _context = context;
    }
    
    public List<ReservationDto> GetReservations()
    {
        var reservations = from r in  _context.Reservations.ToList() 
            select new ReservationDto() 
            { 
                ReservationId = r.ReservationId,
                DeskId = r.DeskId, 
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                UserId = r.UserId
            };
        return reservations.ToList();
    }

    public ReservationDto? AddReservation(ReservationAddDto reservation, string accessToken)
    {
        Reservation? newReservation = ConstructReservation(reservation, accessToken, true);
        if (newReservation == null)
            return null;
        
        var dbReservation = _context.Reservations.Add(newReservation);
        _context.SaveChanges();

        return new ReservationDto()
        {
            StartDate = newReservation.StartDate,
            EndDate = newReservation.EndDate,
            ReservationId = dbReservation.Entity.ReservationId,
            DeskId = newReservation.DeskId,
            UserId = newReservation.UserId
        };
    }

    public ReservationDto? ModifyReservation(ReservationAddDto reservationDto, int id, string accessToken)
    {
        Reservation? reservation = _context.Reservations.Find(id);
        int userId = GetIdFromToken(accessToken);

        if (reservation == null)
            return null;
        
        if (reservation.UserId != userId)
            return null;
        
        var currentStartDate = DateTime.Parse(reservation.StartDate);
        if (DateTime.Now.CompareTo(currentStartDate.Subtract(TimeSpan.FromHours(24))) > 0)
            return null;
        
        Reservation? newReservation = ConstructReservation(reservationDto, accessToken, false);
        if (newReservation == null)
            return null;
        
        reservation.StartDate = newReservation.StartDate;
        reservation.EndDate = newReservation.EndDate;
        reservation.DeskId = newReservation.DeskId;

        reservation.ReservationId = id;
        var dbReservation = _context.Reservations.Update(reservation);
        _context.SaveChanges();

        return new ReservationDto()
        {
            StartDate = newReservation.StartDate,
            EndDate = newReservation.EndDate,
            ReservationId = dbReservation.Entity.ReservationId,
            DeskId = newReservation.DeskId,
            UserId = newReservation.UserId
        };
    }

    private int GetIdFromToken(string accessToken)
    {
        accessToken = accessToken.Substring(7);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(accessToken);
        int userId = int.Parse(token.Claims.First(claim => claim.Type == "UserId").Value);
        return userId;
    }

    private Reservation? ConstructReservation(ReservationAddDto reservation, string accessToken, bool createNew)
    {
        if (_context.Desks.Find(reservation.DeskId) == null)
            return null;
        
        if (reservation.DaysNum <= 0 || 
            reservation.DaysNum > 7 || 
            !_context.Desks.Find(reservation.DeskId)!.IsAvailable)
            return null;

        int userId = GetIdFromToken(accessToken);
        
        var day = DateOnly.Parse(reservation.StartDay);
        // Work day in the office starts at 08:00
        DateTime start = new DateTime(
            day.Year, 
            day.Month, 
            day.Day,
            8, 
            0, 
            0
        );
        DateTime end = start.AddDays(reservation.DaysNum).Subtract(TimeSpan.FromSeconds(1));

        if (start.CompareTo(DateTime.Now) < 0)
            return null;
        
        // Checking if the new date range overlaps with any of the old ones
        List<Reservation> overlapCheckList;
        if (createNew)
            overlapCheckList = _context.Reservations.Where(e => e.DeskId == reservation.DeskId).ToList();
        else
            overlapCheckList = _context.Reservations.Where(e => e.DeskId == reservation.DeskId && 
                                                            e.UserId != userId).ToList();
        
        foreach (var r in overlapCheckList)
        {
            var dStart = DateTime.Parse(r.StartDate);
            var dEnd = DateTime.Parse(r.EndDate);
            Console.WriteLine(dStart.CompareTo(end));
            Console.WriteLine(start.CompareTo(dEnd));
            if (dStart.CompareTo(end) <= 0 && start.CompareTo(dEnd) <= 0)
                return null;
        }

        Reservation newReservation = new Reservation()
        {
            DeskId = reservation.DeskId,
            UserId = userId,
            StartDate = start.ToString("MM-dd-yyyy hh:mm:ss"),
            EndDate = end.ToString("MM-dd-yyyy hh:mm:ss")
        };

        return newReservation;
    }
}