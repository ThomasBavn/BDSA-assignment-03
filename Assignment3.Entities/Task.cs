namespace Assignment3.Entities;


public class Task
{
    /*public int Id { get; set; }

    [StringLength(100)]
    [Required]
    public string Title { get; set; }
    
    public virtual User? AssignedTo { get; set; }

    [MaxLength(int.MaxValue)]
    public string? Description { get; set; }

    [Required]
    [EnumDataType(typeof(State))]
    public string State{ get; set; }

    //Many-to-many reference
    //public Tag Tags {get; set;}
    public virtual ICollection<Tag> Tags { get; set; }*/

}

    public enum State {
        New,
        Active,
        Resolved,
        Closed,
        Removed
    }
    
