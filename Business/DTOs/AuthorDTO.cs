namespace Business.DTOs;

public class AuthorDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }

    
    public ICollection<BookDTO> Books { get; set; }
}