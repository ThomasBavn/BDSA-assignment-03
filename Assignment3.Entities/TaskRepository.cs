namespace Assignment3.Entities;

using System.Collections.Generic;
using Assignment3;
public class TaskRepository : ITaskRepository
{
    readonly KanbanContext context;
    //private int index;
    public TaskRepository(KanbanContext context)
    {


        this.context = context;
        //index = context.Tasks.Max(t => t.Id) + 1;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        //var tasks = context.Tasks;
        //var check = from t in tasks where t.Name == task.Name select new { Id = t.Id };
        //if user doesn't exist return BadRequest
        if (context.Users.Find(task.AssignedToId) == null) return (Response.BadRequest, -1);
        //if (check.Count() > 0) return (Response.Conflict, -1); //return check first item id
        //var index = check.Count() + 1;

        var assignedTo = context.Users.Find(task.AssignedToId)!;

        var description = task.Description; //Description isn't used for anything
        var newTask = new Task();
        //newTask.Id = index++;
        newTask.Title = task.Title;
        newTask.Created = DateTime.UtcNow;
        newTask.AssignedTo = assignedTo;
        newTask.Description = task.Description;
        newTask.Tags = CollectTags(task.Tags);
        newTask.State = State.New;
        newTask.StateUpdated = DateTime.UtcNow;
        context.Tasks.Add(newTask);
        context.SaveChanges();
        return (Response.Created, newTask.Id);
    }

    public TaskDetailsDTO Find(int taskId)
    {
        var search = from t in context.Tasks
                     where t.Id == taskId
                     select t;
        if (!search.Any()) return null!;
        var task = search.First();

        return new TaskDetailsDTO(task.Id, task.Title, task.Description, task.Created, task.AssignedTo!.Name, ExtractTags(task), task.State, task.StateUpdated);
    }

    public IReadOnlyCollection<TaskDTO> Read()
    {
        var all = from t in context.Tasks
                  select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, ExtractTags(t), t.State);
        return all.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadRemoved()
    {
        var removed = from t in context.Tasks
                      where t.State == State.Removed
                      select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, ExtractTags(t), t.State);
        return removed.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadByTag(string tag)
    {
        var search = from t in context.Tasks
                     where t.Tags.Where(tag0 => tag0.Name == tag).Any()
                     select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, ExtractTags(t), t.State);
        return search.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadByUser(int userId)
    {
        var search = from t in context.Tasks
                     where (t.AssignedTo != null && t.AssignedTo.Id == userId)
                     select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, ExtractTags(t), t.State);
        return search.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadByState(State state)
    {
        var search = from t in context.Tasks
                     where t.State == state
                     select new TaskDTO(t.Id, t.Title, t.AssignedTo!.Name, ExtractTags(t), t.State);
        return search.ToList().AsReadOnly();
    }

    public Response Update(TaskUpdateDTO task)
    {
        var search = from t in context.Tasks where t.Id == task.Id select t;
        if (!search.Any()) return Response.NotFound;
        var foundTask = search.First();
        foundTask.Title = task.Title;
        foundTask.AssignedTo = (from u in context.Users where u.Id == task.AssignedToId select u).FirstOrDefault();
        foundTask.Description = task.Description;
        foundTask.Tags = CollectTags(task.Tags);
        foundTask.State = task.State;
        //foundTask.StateUpdated = DateTime.UtcNow;

        context.SaveChanges();
        return Response.Updated;
    }

    public Response Delete(int taskId)
    {
        var search = from t in context.Tasks where t.Id == taskId select t;
        if (!search.Any()) return Response.NotFound;
        var task = search.First();
        if (task.State == State.New)
        {
            context.Tasks.Remove(task);
            context.SaveChanges();
            return Response.Deleted;
        }
        if (task.State == State.Active)
        {
            task.State = State.Removed;
            context.SaveChanges();
            return Response.Deleted;
        }
        return Response.Conflict;
    }

    private IReadOnlyCollection<string> ExtractTags(Task t)
    {
        List<string> tags = new List<string>();
        foreach (var tag in t.Tags)
        {
            tags.Add(tag.Name);
        }
        return tags.ToList().AsReadOnly();
    }
    private List<Tag> CollectTags(IEnumerable<string> tags)
    {
        List<Tag> newTags = new List<Tag>();
        foreach (var tagname in tags) //run through all tagnames
        {
            var existingTag = (from t in context.Tags where t.Name.Equals(tagname) select t);
            if (existingTag.Any()) newTags.Add(existingTag.First()); //if tag with name exists add it
            else
            { //else make a new tag with that name
                //newTag.Id = index++;
                var newTag = new Tag();
                newTag.Name = tagname;
                newTags.Add(newTag);
            }
        }
        return newTags;
    }
}
