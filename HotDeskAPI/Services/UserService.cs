using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using HotDeskAPI.Data;
using HotDeskAPI.Entities;
using HotDeskAPI.Models;

namespace HotDeskAPI.Services;

public class UserService : IUsers
{
    private readonly DataContext _context;
    
    public UserService(DataContext context)
    {
        _context = context;
    }
    
    public UserDto? GetUser(UserLoginDto user)
    {
        //Looking if a user with this login exists
        var userInfo = _context.Users.FirstOrDefault(o =>
            o.Username.Equals(user.Username));
        if (userInfo == null)
            return null;
        
        //Verifying password
        var hmac = new HMACSHA512(userInfo.PasswordSalt);
        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
        var passwordString1 = BitConverter.ToString(passwordHash).Replace("-", string.Empty).ToLower();
        var passwordString2 = BitConverter.ToString(userInfo.PasswordHash).Replace("-", string.Empty).ToLower();
        if (!passwordString1.Equals(passwordString2))
            return null;
        
        UserDto userDto = new UserDto()
        {
            Username = user.Username,
            UserId = userInfo.UserId,
            Role = userInfo.Role
        };

        return userDto;
    }

    public User AddUser(UserLoginDto user)
    {
        using (var hmac = new HMACSHA512())
        {
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            User newUser = new User()
            {
                Username = user.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };
            
            _context.Add(newUser);
            _context.SaveChanges();

            return newUser;
        }
    }
}