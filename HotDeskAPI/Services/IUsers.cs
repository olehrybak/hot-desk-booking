using HotDeskAPI.Entities;
using HotDeskAPI.Models;

namespace HotDeskAPI.Services;

public interface IUsers
{
    public UserDto? GetUser(UserLoginDto user);
    public User AddUser(UserLoginDto user);
}