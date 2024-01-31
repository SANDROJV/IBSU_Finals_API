namespace Business.DTOs;

public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public byte[]? Image { get; set; }
    public double Rating { get; set; }
    public DateTime DateAdded { get; set; }
    public bool Available { get; set; }
    
    public ICollection<AuthorDTO> Authors { get; set; }

}