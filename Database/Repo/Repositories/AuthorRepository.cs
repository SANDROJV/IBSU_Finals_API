using Business.DTOs;
using Database.Data;
using Database.Entities;
using Database.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.Repo.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryContext _context;

    public AuthorRepository(LibraryContext context)
    {
        _context = context;
    }
    public async Task<List<Book>> GetBooksForAuthors()
    {
        var books = await _context.Books
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .ToListAsync();

        return books;
    }

    public async Task<Author> GetAuthorById(int id)
    {
        var author = await _context.Authors
            .Where(a => a.Id == id)
            .Include(a => a.BookAuthors)
            .ThenInclude(ba => ba.Book)
            .SingleOrDefaultAsync();

        return author;
    }

    public async Task CheckAuthor( string normalizedFirstName , string normalizedLastName)
    {
        var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a =>
            a.FirstName.ToLower() == normalizedFirstName &&
            a.LastName.ToLower() == normalizedLastName);

        if (existingAuthor != null)
        {
            throw new ArgumentException("This author is already added");
        }
    }

    public async Task<Author> CheckAuthorForUpdate(EditAuthorDTO editAuthorDTO)
    {
        var author = await _context.Authors
            .Where(b => b.Id == editAuthorDTO.AuthorId)
            .SingleOrDefaultAsync();

        if (author == null)
        {
            throw new ArgumentException("No author with the given ID was found.");
        }

        return author;
    }

    public async Task DeleteAuthorById(int authorId)
    {
        var author = await _context.Authors.FindAsync(authorId);

        if (author == null)
        {
            throw new ArgumentException("No author with the given ID was found.");
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
}