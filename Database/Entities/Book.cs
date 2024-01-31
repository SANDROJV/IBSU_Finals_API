namespace Database.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public byte[]? Image { get; set; }
    public double Rating { get; set; }
    public DateTime DateAdded { get; set; }
    public bool available { get; set; }
    
    public ICollection<BookAuthor> BookAuthors { get; set; }
}