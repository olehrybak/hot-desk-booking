using System.IdentityModel.Tokens.Jwt;
using HotDeskAPI.Models;
using HotDeskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace HotDeskAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservations _reservationService;

    public ReservationController(IReservations reservationService)
    {
        _reservationService = reservationService;
    }
    
    // GET: /api/reservation
    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> Get()
    {
        var reservations = await Task.FromResult(_reservationService.GetReservations());
        return Ok(reservations);
    }

    // POST: /api/reservation
    [HttpPost]
    public async Task<ActionResult<ReservationDto>> Post(ReservationAddDto reservation)
    {
        string accessToken = Request.Headers[HeaderNames.Authorization];

        var newReservation = await Task.FromResult(_reservationService.AddReservation(reservation, accessToken));
        if (newReservation == null)
            return BadRequest();

        return newReservation;
    }

    // PUT: /api/reservation/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ReservationDto>> Put(ReservationAddDto reservation, int id)
    {
        string accessToken = Request.Headers[HeaderNames.Authorization];
        var newReservation = await  Task.FromResult(_reservationService.ModifyReservation(reservation, id, accessToken));
        if (newReservation == null)
            return BadRequest();
        return newReservation;
    }
}