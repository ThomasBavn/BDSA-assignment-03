namespace Assignment3.Entities;
public class UserRepository : IUserRepository
{
    //private int index;
    readonly KanbanContext context;
    public UserRepository(KanbanContext context)
    {
        this.context = context;
        //index = context.Users.Max(u => u.UserId) + 1;
    }

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        if (context.Users.Where(u => u.Email.Equals(user.Email)).Any()) return (Response.Conflict, -1);
        context.Users.Count();
        User u = new User();
        //u.Id = index++;
        u.Name = user.Name;
        u.Email = user.Email;
        context.Users.Add(u);
        context.SaveChanges();
        return (Response.Created, u.Id);
    }


    public Response Delete(int userId, bool force = false)
    {
        IEnumerable<User> search = from u in context.Users where u.Id == userId select u;
        if (!search.Any()) return Response.NotFound;
        User user = search.First();
        if (!user.Tasks.Any() || force)
        {
            foreach (var t in user.Tasks)
            {
                t.AssignedTo = null;
            }
            context.Users.Remove(user);
            context.SaveChanges();
            return Response.Deleted;
        }
        return Response.Conflict;
    }

    public UserDTO Find(int userId)
    {
        var search = from u in context.Users where u.Id == userId select new UserDTO(u.Id, u.Name, u.Email);
        return search.Any() ? search.First() : null!;
    }

    public IReadOnlyCollection<UserDTO> Read()
    {
        var all = from u in context.Users select new UserDTO(u.Id, u.Name, u.Email);
        return all.ToList().AsReadOnly();
    }

    public Response Update(UserUpdateDTO user)
    {
        var search = from u in context.Users where u.Id == user.Id select u;
        if (!search.Any()) return Response.NotFound;
        var foundUser = search.First();
        if (context.Users.Find(user.Id) != null) return Response.Conflict;
        //foundUser.Id = user.Id;
        foundUser.Name = user.Name;
        foundUser.Email = user.Email;

        context.SaveChanges();
        return Response.Updated;
    }
}