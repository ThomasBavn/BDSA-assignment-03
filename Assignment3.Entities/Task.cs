using System.Collections.ObjectModel;

namespace Assignment3.Entities;


public class Task
{

    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime Created { get; set; }
    public User? AssignedTo { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }
    public State State { get; set; }
    public DateTime StateUpdated { get; set; }

}
