namespace Assignment3.Entities;

public class User
{
    public int Id{ get; set; }

    [StringLength(100)]
    [Required]
    public string Name{ get; set; }

    [StringLength(100)]
    [Required]
    ////////////Skal tjekke for unique
    public string Email{ get; set; }
    //[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property, AllowMultiple=false)]
    //public sealed class EmailAddressAttribute : System.ComponentModel.DataAnnotations.DataTypeAttribute {};
    

    public List<Task> Tasks{ get; set; }
    
}
