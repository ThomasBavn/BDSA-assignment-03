namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }

    // [StringLength(50, MinimumLength = 5)]
    // [Required]
    //mangler at tjekke for at den er unique
    public string Email { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }


}
