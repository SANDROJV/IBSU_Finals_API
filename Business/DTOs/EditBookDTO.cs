using Microsoft.AspNetCore.Http;

namespace Business.DTOs;

public class EditBookDTO
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public IFormFile ImageFile { get; set; }
    public double Rating { get; set; }

    public int? AuthorId { get; set; } // Nullable, as it will be null when creating a new author
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}