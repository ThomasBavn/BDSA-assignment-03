namespace Assignment3.Entities;


public class Task
{
    public int Id { get; set; }

    [StringLength(100)]
    [Required]
    public string Title { get; set; }
    
    //Optional - is nullable
    public User? AssignedTo { get; set; }

    [MaxLength(int.MaxValue)]
    public string? Description { get; set; }

    //[Required] - virker ikke, fordi det ikke er et property
    public enum State {
        New,
        Active,
        Resolved,
        Closed,
        Removed
    }

    public Tag Tags {get; set;}


    /*
State : enum (New, Active, Resolved, Closed, Removed), required
    */
}
