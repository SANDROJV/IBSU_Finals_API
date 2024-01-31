namespace Business.DTOs;

public class BookAuthorDTO
{

    public string Title { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public DateTime DateAdded { get; set; }
    public bool Available { get; set; }

    public int? AuthorId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    
    
}

