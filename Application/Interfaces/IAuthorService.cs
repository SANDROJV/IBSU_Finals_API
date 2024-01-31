using Business.DTOs;

namespace Application.Interfaces;

public interface IAuthorService
{
    Task<List<AuthorDTO>> GetAuthors();

    Task<AuthorDTO> GetAuthorById(int id);

    Task AddAuthor(AddAuthorDTO addAuthorDto);

    Task EditAuthor(EditAuthorDTO authorDto);

    Task DeleteAuthorById(int authorId);

}