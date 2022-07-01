using HotDeskAPI.Models;
using HotDeskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DeskController : ControllerBase
{
    private readonly IDesks _desksRepo;
    
    public DeskController(IDesks desksRepo)
    {
        _desksRepo = desksRepo;
    }
    
    // GET: /api/desk
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeskDto>>> Get()
    {
        var desks = await Task.FromResult(_desksRepo.GetDesks());
        return Ok(desks);
    }
    
    // POST: /api/desk
    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost]
    public async Task<IActionResult> Post(DeskAddDto newDesk)
    {
        var desk = await Task.FromResult(_desksRepo.AddDesk(newDesk));
        if (desk == null)
            return NotFound();
        return Ok(desk);
    }
    
    // DELETE: /api/desk
    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var desk = await Task.FromResult(_desksRepo.DeleteDesks(id));
        if (desk == null)
            return BadRequest();
        return Ok(desk);
    }
    
    // PUT: /api/desk/id
    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id)
    {
        var desk = await Task.FromResult(_desksRepo.ChangeAvailability(id));
        if (desk == null)
            return NotFound();
        return Ok(desk);
    }
    
    // GET: /api/desk/id
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<ReservationUserDto>>> GetReservations(int id)
    {
        var reservations = await Task.FromResult(_desksRepo.GetDeskReservations(id));
        if (reservations.Count == 0)
            return NotFound();
        return Ok(reservations);
    }
    
    // GET: /api/desk/filter?locationId={id}
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<DeskDto>>> GetById([FromQuery] int locationId)
    {
        var desks = await Task.FromResult(_desksRepo.GetDesksById(locationId));
        if (desks.Count == 0)
            return NotFound();
        return Ok(desks);
    }
}