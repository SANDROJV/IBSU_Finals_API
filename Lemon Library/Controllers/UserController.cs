using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Business.DTOs;
using Database.Data;
using Database.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Lemon_Library.Controllers;

public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserDTO userDto)
    {

        try
        {
            var result = await _userService.Register(userDto);
            return Ok(result);
        }
        catch (ArgumentNullException e)
        {
            return NotFound($"An error occurred: {e.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }

         
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDTO userDto)
    {
        try
        {
            var token = await _userService.Login(userDto);
            return Ok(token);
        }
        catch (ArgumentNullException e)
        {
            return NotFound($"An error occurred: {e.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
        
        
    }
    
    
}