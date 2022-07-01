using HotDeskAPI.Models;

namespace HotDeskAPI.Services;

public interface ILocations
{
    public List<LocationDto> GetLocations();
    public LocationDto? DeleteLocation(int id);
    public LocationDto AddLocation(LocationAddDto location);
}