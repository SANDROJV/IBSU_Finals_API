using System.IdentityModel.Tokens.Jwt;
using Business.DTOs;
using Database.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task<User> Register(UserDTO userDto);
    
    Task<string> Login(UserDTO userDto);
    
    
}