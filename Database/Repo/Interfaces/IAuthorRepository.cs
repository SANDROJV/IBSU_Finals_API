using Business.DTOs;
using Database.Entities;

namespace Database.Repo.Interfaces;

public interface IAuthorRepository
{
    Task<List<Book>> GetBooksForAuthors();
    Task<Author> GetAuthorById(int id);
    Task CheckAuthor(string normalizedFirstName , string normalizedLastName);
    Task<Author> CheckAuthorForUpdate(EditAuthorDTO editAuthorDTO);
    Task DeleteAuthorById(int id);
}