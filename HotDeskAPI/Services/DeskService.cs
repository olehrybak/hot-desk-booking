using HotDeskAPI.Data;
using HotDeskAPI.Entities;
using HotDeskAPI.Models;

namespace HotDeskAPI.Services;

public class DeskService : IDesks
{
    private readonly DataContext _context;
    
    public DeskService(DataContext context)
    {
        _context = context;
    }
    
    public List<DeskDto> GetDesks()
    {
        var desks = from d in  _context.Desks.ToList() 
            select new DeskDto()
            {
                DeskId = d.DeskId,
                LocationId = d.LocationId,
                IsAvailable = d.IsAvailable
            };
        return desks.ToList();
    }
    
    public DeskDto? DeleteDesks(int id)
    {
        Desk? desk = _context.Desks.Find(id);

        if (desk != null)
        {
            if(_context.Reservations.Any(e => e.DeskId == desk.DeskId))
                return null;
            _context.Desks.Remove(desk);
            _context.SaveChanges();
            var deskDto = new DeskDto(){
                LocationId = desk.LocationId,
                DeskId = desk.DeskId,
                IsAvailable = desk.IsAvailable
            };
            return deskDto;
        }
        return null;
    }
    
    public DeskDto? AddDesk(DeskAddDto deskDto)
    {
        if (!_context.Locations.Any(e => e.LocationId == deskDto.LocationId))
            return null;
        Desk desk = new Desk()
        {
            LocationId = deskDto.LocationId,
            IsAvailable = true
        };
        var dbDesk = _context.Desks.Add(desk);
        _context.SaveChanges();

        return new DeskDto()
        {
            LocationId = dbDesk.Entity.LocationId,
            DeskId = dbDesk.Entity.DeskId,
            IsAvailable = dbDesk.Entity.IsAvailable
        };
    }

    public DeskDto? ChangeAvailability(int id)
    {
        Desk? desk = _context.Desks.Find(id);
        
        if (desk != null)
        {
            desk.IsAvailable = !desk.IsAvailable;
            _context.Desks.Update(desk);
            _context.SaveChanges();
            var deskDto = new DeskDto(){
                LocationId = desk.LocationId,
                DeskId = desk.DeskId,
                IsAvailable = desk.IsAvailable
            };
            return deskDto;
        }
        return null;
    }

    public List<ReservationUserDto> GetDeskReservations(int id)
    {
        List<Reservation> reservations = _context.Reservations.Where(e => e.DeskId == id).ToList();

        var reservationDtos = from r in reservations
            select new ReservationUserDto()
            {
                ReservationId = r.ReservationId,
                StartDate = r.StartDate,
                EndDate = r.EndDate
            };

        return reservationDtos.ToList();
    }

    public List<DeskDto> GetDesksById(int id)
    {
        List<Desk> desks = _context.Desks.Where(e => e.LocationId == id).ToList();

        var deskDtos = from d in desks
            select new DeskDto()
            {
                DeskId = d.DeskId,
                IsAvailable = d.IsAvailable,
                LocationId = d.LocationId
            };
        return deskDtos.ToList();
    }
}