using HotDeskAPI.Models;
using HotDeskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LocationController : ControllerBase
{
    private readonly ILocations _locationsRepo;
    
    public LocationController(ILocations locationsRepo)
    {
        _locationsRepo = locationsRepo;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDto>>> Get()
    {
        var locations = await Task.FromResult(_locationsRepo.GetLocations());
        return Ok(locations);
    }
    
    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost]
    public async Task<ActionResult<LocationDto>> Post(LocationAddDto newLocation)
    {
        if (newLocation.Name.Length == 0)
            return BadRequest();
        
        var location = await Task.FromResult(_locationsRepo.AddLocation(newLocation));
        return Ok(location);
    }
    
    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<LocationDto>> Delete(int id)
    {
        var location = await Task.FromResult(_locationsRepo.DeleteLocation(id));
        if (location == null)
            return BadRequest();
        return Ok(location);
    }
    
}