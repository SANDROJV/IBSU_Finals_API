using Application.Interfaces;
using Application.Services;
using Database.Data;
using Business.DTOs;
using Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lemon_Library.Controllers;

public class BooksController : BaseApiController
{
    private readonly LibraryContext _context;
    private readonly IBooksService _booksService;

    public BooksController(LibraryContext context, IBooksService booksService)
    {
        _context = context;
        _booksService = booksService;
    }
    
    // წიგნების სრული სიის წამოღება
    
    
    // GET: api/books
    [HttpGet()]
    public async Task<IActionResult> GetBooks()
    {
        try
        {
            var bookDTOs = await _booksService.GetBooks();

            return Ok(bookDTOs);
        }
        catch (InvalidOperationException e)
        {
            return NotFound($"An error occurred: {e.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
        
    }
    
    
    // დეტალების წამოღება
    
    // GET: api/books/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        try
        {
            var BookDTO= await _booksService.GetBookById(id);
            return Ok(BookDTO);
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

    
    
    
    // წიგნის დამატება
    
    // POST: api/books
        [HttpPost("add")]
        public async Task<IActionResult> AddBookWithAuthor([FromForm] BookAuthorDTO bookAuthorDTO, IFormFile imageFile)
        {
            try
            {
                await _booksService.AddBookWithAuthor(bookAuthorDTO, imageFile);

                return Ok("Book added successfully");
            }
            catch (ArgumentException e)
            {
                return BadRequest($"An error occurred: {e.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
            return BadRequest("Invalid book data.");
        }
        
        
    // წიგნის რედაქტირება
    
    [HttpPut("edit")]
    public async Task<IActionResult> UpdateBook([FromForm] EditBookDTO updatedBook)
    {
        try
        {
            await _booksService.EditBook(updatedBook);

            return Ok("Book edited successfully");
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
    
    // გატანა დაბრუნება
    [HttpPut("{bookId}/toggle-status")]
    public async Task<IActionResult> ToggleBookStatus(int bookId)
    {
        try
        {
            await _booksService.ToggleBookStatus(bookId);
            return Ok("Book availability status toggled successfully.");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    
    //წაშლა
    [HttpDelete("{bookId}")]
    public async Task<IActionResult> DeleteBookById(int bookId)
    {
        try
        {
            await _booksService.DeleteBookById(bookId);
            return Ok("Book was deleted successfully");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    
}