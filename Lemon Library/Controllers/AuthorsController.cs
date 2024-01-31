using Application.Interfaces;
using Database.Data;
using Business.DTOs;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lemon_Library.Controllers;

public class AuthorsController : BaseApiController
{
    private readonly LibraryContext _context;
    private readonly IAuthorService _authorService;

    public AuthorsController(LibraryContext context, IAuthorService authorService)
    {
        _context = context;
        _authorService = authorService;
    }
    //ავტორების სრული სიის წამოღება
    
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        try
        {
            var authorDtos = await _authorService.GetAuthors();

            return Ok(authorDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    
    // დეტალების წამოღება
    
    // GET: api/authors/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor(int id)
    {
        try
        {
            var authorDtos = await _authorService.GetAuthorById(id);

            return Ok(authorDtos);
        }
        catch (ArgumentException e)
        {
            return NotFound($"An error occurred: {e.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    
    // ავტორის დამატება
    [HttpPost("add")]
    public async Task<IActionResult> AddAuthor([FromBody] AddAuthorDTO addAuthorDto)
    {
        try
        {
            await _authorService.AddAuthor(addAuthorDto);

            return Ok("Author was added successfully");
        }
        catch (ArgumentException e)
        {
            return NotFound($"An error occurred: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    //ავტორის რედაქტირება
    [HttpPut("edit")]
    public async Task<IActionResult> UpdateAuthor([FromBody] EditAuthorDTO updatedAuthor)
    {
        try
        {
            await _authorService.EditAuthor(updatedAuthor);
            return Ok("Author was updated successfully");
        }
        catch (ArgumentException e)
        {
            return NotFound($"An error occurred: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
    
    // ავტორის წაშლა
    [HttpDelete("{authorId}")]
    public async Task<IActionResult> DeleteBookById(int authorId)
    {
        try
        {
            await _authorService.DeleteAuthorById(authorId);
            return Ok("Author was deleted successfully");
        }
        catch (ArgumentException e)
        {
            return NotFound($"An error occurred: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}