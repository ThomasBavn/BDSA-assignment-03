namespace Assignment3.Entities;


public class Task
{
    public Task()
    {
        Tags = new List<Tag>();
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public User? AssignedTo { get; set; }
    public string? Description { get; set; }
    public Core.State State { get; set; }
    public virtual List<Tag> Tags {get; set;}


    /*
State : enum (New, Active, Resolved, Closed, Removed), required
    */
}
