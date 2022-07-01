using HotDeskAPI.Data;
using HotDeskAPI.Entities;
using HotDeskAPI.Models;

namespace HotDeskAPI.Services;

public class LocationService : ILocations
{
    private readonly DataContext _context;
    
    public LocationService(DataContext context)
    {
        _context = context;
    }
    
    public List<LocationDto> GetLocations()
    {
        var locations = from l in  _context.Locations.ToList() 
            select new LocationDto()
            {
                LocationId = l.LocationId,
                Name = l.Name,
            };
        return locations.ToList();
    }

    public LocationDto? DeleteLocation(int id)
    {
        Location? location = _context.Locations.FirstOrDefault(e => e.LocationId == id);

        if (location != null)
        {
            if(_context.Desks.Any(e => e.LocationId == location.LocationId))
                return null;
            _context.Locations.Remove(location);
            _context.SaveChanges();
            var locationDto = new LocationDto(){
                    LocationId = location.LocationId,
                    Name = location.Name,
                };
            return locationDto;
        }

        return null;
    }

    public LocationDto AddLocation(LocationAddDto locationDto)
    {
        Location location = new Location()
        {
            Name = locationDto.Name
        };
        var dbLocation = _context.Locations.Add(location);
        _context.SaveChanges();

        return new LocationDto()
        {
            LocationId = dbLocation.Entity.LocationId,
            Name = dbLocation.Entity.Name
        };
    }
}