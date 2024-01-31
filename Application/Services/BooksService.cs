using System.Drawing.Imaging;
using Application.Interfaces;
using AutoMapper;
using Database.Repo.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Application.Services;
using Database.Data;
using Business.DTOs;
using Database.Entities;

public class BooksService : IBooksService
{
    private readonly LibraryContext _context;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BooksService(LibraryContext context, IBookRepository bookRepository, IMapper mapper)
    {
        _context = context;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<List<BookDTO>> GetBooks()
    {
        var books = await _bookRepository.GetBooks();
        
        var bookDTOs = _mapper.Map<List<BookDTO>>(books);

        return bookDTOs;
    }

    public async Task<BookDTO> GetBookById(int id)
    {
        var book = await _bookRepository.GetBookById(id);

        var bookDTO = _mapper.Map<BookDTO>(book);

        return bookDTO; 
    }

    public async Task AddBookWithAuthor(BookAuthorDTO bookAuthorDTO, IFormFile imageFile)
    {
        Author existingAuthor = await _bookRepository.CheckBook(bookAuthorDTO);
        

        if (existingAuthor == null)
        {
            existingAuthor = new Author
            {
                FirstName = bookAuthorDTO.FirstName,
                LastName = bookAuthorDTO.LastName,
                BirthDate = bookAuthorDTO.BirthDate
            };
        }
        
        byte[] imageByteArray = null;
        if (imageFile != null && imageFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);

                using (var image = Image.Load(memoryStream.ToArray()))
                {
                    var encoder = new JpegEncoder
                    {
                        Quality = 80 
                    };

                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(800, 600)
                    }));

                    image.SaveAsJpeg(memoryStream, encoder);
                }

                imageByteArray = memoryStream.ToArray();
            }
        }


        var book = new Book
        {
            Title = bookAuthorDTO.Title,
            Description = bookAuthorDTO.Description,
            Rating = bookAuthorDTO.Rating,
            DateAdded = bookAuthorDTO.DateAdded,
            available = bookAuthorDTO.Available,
            Image = imageByteArray
        };

        book.BookAuthors = new[]
        {
            new BookAuthor { Author = existingAuthor }
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }



    public async Task EditBook(EditBookDTO editBookDTO)
    {
        await _bookRepository.EditBook(editBookDTO);

    }

    public async Task ToggleBookStatus(int bookId)
    {
        await _bookRepository.UpdateBookStatus(bookId);
    }

    public async Task DeleteBookById(int bookId)
    {
        await _bookRepository.DeleteBookById(bookId);
    }
}