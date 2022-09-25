namespace Assignment3.Entities;

public class User
{
    public int Id{ get; set; }

    [StringLength(100)]
    [Required]
    public string Name{ get; set; }

    [StringLength(100)]
    [Required]
    //mangler at tjekke for at den er unique
    public string Email{ get; set; }

    public List<Task> Tasks{ get; set; }
    
    
    /*
Email : string(100), required, unique
    */
}
