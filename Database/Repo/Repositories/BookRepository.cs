using AutoMapper;
using Business.DTOs;
using Database.Data;
using Database.Entities;
using Database.Repo.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Database.Repo.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;
    private readonly IMapper _mapper;

    public BookRepository(LibraryContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<List<BookDTO>> GetBooks()
    {
        var books = await _context.Books
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .ToListAsync();
        List<BookDTO> bookDtos = new();
        foreach (var item in books)
        {
            bookDtos.Add(_mapper.Map<BookDTO>(item));
        }
        
        
        
        return bookDtos;
    }

    public async Task<BookDTO> GetBookById(int id)
    {
        var book = await _context.Books
            .Where(b => b.Id == id)
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .SingleOrDefaultAsync();
        
        
        if (book == null)
        {
            throw new ArgumentNullException("No book with the given ID was found.");
        }
        
        return _mapper.Map<BookDTO>(book);
        
    }

    

    public async Task<Author> CheckBook(BookAuthorDTO bookAuthorDTO)
    {
        Author existingAuthor = await _context.Authors.FirstOrDefaultAsync(a =>
            a.FirstName.ToLower() == bookAuthorDTO.FirstName.ToLower() &&
            a.LastName.ToLower() == bookAuthorDTO.LastName.ToLower());

        bool titleExists = await _context.Books.AnyAsync(b =>
            b.Title.ToLower() == bookAuthorDTO.Title.ToLower());
        
        if (titleExists)
        {
            throw new ArgumentException("A book with the same title already exists.");
        }


        return existingAuthor;
    }



    public async Task EditBook(EditBookDTO editBookDTO)
    {
        var book = await _context.Books
            .Where(b => b.Id == editBookDTO.BookId)
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .SingleOrDefaultAsync();

        if (book == null)
        {
            throw new ArgumentException("No book with the given ID was found.");
        }
        
        
        _mapper.Map(editBookDTO, book);

        if (editBookDTO.AuthorId.HasValue)
        {
            var existingAuthor = await _context.Authors.FindAsync(editBookDTO.AuthorId.Value);

            if (existingAuthor == null)
            {
                throw new ArgumentException("The specified author does not exist.");
            }

            var existingBookAuthor = book.BookAuthors.FirstOrDefault();
            if (existingBookAuthor != null)
            {
                existingBookAuthor.Author = existingAuthor;
            }
            else
            {
                book.BookAuthors.Add(new BookAuthor { Author = existingAuthor });
            }
        }
        else
        {
            var newAuthor = new Author
            {
                FirstName = editBookDTO.FirstName,
                LastName = editBookDTO.LastName,
                BirthDate = editBookDTO.BirthDate
            };

            var existingBookAuthor = book.BookAuthors.FirstOrDefault();
            if (existingBookAuthor != null)
            {
                existingBookAuthor.Author = newAuthor;
            }
            else
            {
                book.BookAuthors.Add(new BookAuthor { Author = newAuthor });
            }
        }

        if (editBookDTO.ImageFile != null && editBookDTO.ImageFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await editBookDTO.ImageFile.CopyToAsync(memoryStream);
                book.Image = memoryStream.ToArray();
            }
        }

        await _context.SaveChangesAsync();

    }

    public async Task UpdateBookStatus(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);

        if (book == null)
        {
            throw new ArgumentException("No book with the given ID was found.");
        }

        book.available = !book.available;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookById(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);

        if (book == null)
        {
            throw new ArgumentException("No book with the given ID was found.");
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}