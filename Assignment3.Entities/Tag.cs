namespace Assignment3.Entities;

public class Tag
{
    public int Id{ get; set;}
    public string Name{ get; set; }
    public virtual List<Task> Tasks{ get; set; }

    public Tag()
    {
        Tasks = new List<Task>();
    }
}
