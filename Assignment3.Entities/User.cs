namespace Assignment3.Entities;

public class User
{
    public User()
    {
        Tasks = new List<Task>();
    }
    public int Id{ get; set; }
    public string Name{ get; set; }

    [StringLength(100)]
    [Required]
    //mangler at tjekke for at den er unique
    public string Email{ get; set; }

    public virtual List<Task> Tasks{ get; set; }


}
