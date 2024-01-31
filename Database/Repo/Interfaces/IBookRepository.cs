using Business.DTOs;
using Database.Entities;
using Microsoft.AspNetCore.Http;

namespace Database.Repo.Interfaces;

public interface IBookRepository
{
    Task<List<BookDTO>> GetBooks();
    Task<BookDTO> GetBookById(int id);
    Task<Author> CheckBook(BookAuthorDTO bookAuthorDTO);
    Task EditBook(EditBookDTO editBookDTO);
    Task UpdateBookStatus(int bookId);
    Task DeleteBookById(int bookId);
}