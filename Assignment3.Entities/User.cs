using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Assignment3.Entities;

public class User
{
    public User()
    {
        Tasks = new List<Task>();
    }
    public int Id{ get; set; }
    public string Name{ get; set; }

    public string Email{ get; set; }

    public virtual List<Task> Tasks{ get; set; }


}
