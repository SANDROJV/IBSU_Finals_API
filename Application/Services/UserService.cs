using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.Interfaces;
using Business.DTOs;
using Database.Data;
using Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly LibraryContext _context;
    private readonly IConfiguration _configuration;

    public UserService(LibraryContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<User> Register(UserDTO userDto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);

        if (existingUser != null)
        {
            throw new ArgumentException("Username already exists.");
        }

        CreatePasswordHash(userDto.Password, out byte[] passwordHash,out byte[] passwordSalt );

        var result = new User
        {
            Username = userDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        _context.Users.Add(result);
        await _context.SaveChangesAsync();

        return result;
    }

    public async Task<string> Login(UserDTO userDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == userDto.Username);

        if (user == null)
        {
            throw new ArgumentNullException("User not found");
        }

        if (!VerifyPasswordHash(userDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new ArgumentException("Wrong Password.");
        }

        var token = CreateToken(user);

        return token;
    }
    
    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}