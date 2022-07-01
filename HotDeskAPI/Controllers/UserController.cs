using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotDeskAPI.Models;
using HotDeskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HotDeskAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUsers _usersRepo;
    private readonly IConfiguration _config;
    
    public UserController(IUsers usersRepo, IConfiguration config)
    {
        _usersRepo = usersRepo;
        _config = config;
    }
    
    [AllowAnonymous]
    [HttpPost("/login")]
    public async Task<IActionResult> Login(UserLoginDto user)
    {
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            return BadRequest();

        var loginUser = await Task.FromResult(_usersRepo.GetUser(user));
        if (loginUser == null)
            return NotFound();

        var claims = new[]
        {
            new Claim("UserId", loginUser.UserId.ToString()),
            new Claim("UserName", loginUser.Username),
            new Claim(ClaimTypes.Role, loginUser.Role)
        };
        
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(tokenString);
    }
    
    [AllowAnonymous]
    [HttpPost("/register")]
    public async Task<IActionResult> Register(UserLoginDto user)
    {
        await Task.FromResult(_usersRepo.AddUser(user));
        return Ok();
    }
    
}