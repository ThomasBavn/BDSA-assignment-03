namespace Assignment3.Entities;

public class Tag
{
    public int Id{ get; set;}

    [StringLength(50)]
    [Required]
    //////////Skal tjekke for unique
    public string Name{ get; set; }

    
    //Many-to-many reference
    //public Task Tasks{ get; set; }
    public virtual ICollection<Task> Tasks { get; set; }
}
