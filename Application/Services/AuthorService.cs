using Application.Interfaces;
using AutoMapper;
using Business.DTOs;
using Database.Data;
using Database.Entities;
using Database.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AuthorService : IAuthorService
{
    private readonly LibraryContext _context;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public AuthorService(LibraryContext context, IAuthorRepository authorRepository, IMapper mapper)
    {
        _context = context;
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<List<AuthorDTO>> GetAuthors()
    {
        var books = await _authorRepository.GetBooksForAuthors();

        var authors = books
            .SelectMany(b => b.BookAuthors.Select(ba => ba.Author))
            .Distinct()
            .ToList();
        
        var authorDTOs = _mapper.Map<List<AuthorDTO>>(authors);
        
        foreach (var authorDTO in authorDTOs)
        {
            authorDTO.Books = books
                .Where(b => b.BookAuthors.Any(ba => ba.Author.Id == authorDTO.Id))
                .Select(b => _mapper.Map<BookDTO>(b))
                .ToList();
        }
        
        if (authorDTOs.Count == 0)
        {
            throw new InvalidOperationException("No authors found.");
        }
        
        return authorDTOs;
    }


    public async Task<AuthorDTO> GetAuthorById(int id)
    {
        var author = await _authorRepository.GetAuthorById(id);

        if (author == null)
        {
            throw new ArgumentException("No author with given ID was found");
        }

        var booksDTO = author.BookAuthors.Select(ba => new BookDTO
        {
            Id = ba.Book.Id,
            Title = ba.Book.Title,
            Description = ba.Book.Description,
            Rating = ba.Book.Rating,
            DateAdded = ba.Book.DateAdded,
            Available = ba.Book.available,
        }).ToList();
        
        var authorDTO = new AuthorDTO
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            BirthDate = author.BirthDate,
        
            Books = booksDTO
        };

        return authorDTO;
    }

    public async Task AddAuthor(AddAuthorDTO addAuthorDto)
    {
        string normalizedFirstName = addAuthorDto.FirstName.ToLower();
        string normalizedLastName = addAuthorDto.LastName.ToLower();
        
        await _authorRepository.CheckAuthor(normalizedFirstName, normalizedLastName);

        var newAuthor = new Author
        {
            FirstName = addAuthorDto.FirstName,
            LastName = addAuthorDto.LastName,
            BirthDate = addAuthorDto.BirthDate
        };
        
        _context.Authors.Add(newAuthor);
        await _context.SaveChangesAsync();

    }

    public async Task EditAuthor(EditAuthorDTO editAuthorDTO)
    {
        var author = await _authorRepository.CheckAuthorForUpdate(editAuthorDTO);

        author.FirstName = editAuthorDTO.FirstName;
        author.LastName = editAuthorDTO.LastName;
        author.BirthDate = editAuthorDTO.BirthDate;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAuthorById(int authorId)
    {
        await _authorRepository.DeleteAuthorById(authorId);
    }
}