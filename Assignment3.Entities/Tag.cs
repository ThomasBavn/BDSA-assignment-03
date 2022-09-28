namespace Assignment3.Entities;

using System.Collections.ObjectModel;
using Assignment3;
public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

}
