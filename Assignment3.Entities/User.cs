namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // [StringLength(50, MinimumLength = 5)]
    // [Required]
    //mangler at tjekke for at den er unique
    public string Email { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();


}
