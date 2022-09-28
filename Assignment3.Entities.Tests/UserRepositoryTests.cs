namespace Assignment3.Entities.Tests;

public sealed class UserRepositoryTests : IDisposable
{

    private readonly KanbanContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var users = GetUsers();

        context.AddRange(users);
        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public void Create_User_Already_Exists()
    {
        var users = GetUsers();
        _context.Users.AddRange(users);
        //Arrange
        var user = new UserCreateDTO(
            "User1",
            "user1@example.com"
            );

        //Act
        var response = _repository.Create(user);

        //Assert
        Assert.Equal(Response.Conflict, response.Response);
    }

    [Fact]
    public void Create_User_Success()
    {
        _context.Users.AddRange(GetUsers());
        //Arrange
        var user = new UserCreateDTO(
            "NewUser",
            "brandNewUser@example.com");

        //Act
        var response = _repository.Create(user);

        //Assert
        Assert.Equal(Response.Created, response.Response);
    }

    //Users who are assigned to a task may only be deleted using the force

    [Fact]
    public void Delete_User_With_Tasks()
    {
        var users = GetUsers();
        _context.Users.AddRange(users);
        var userWithTasks = users[1];
        //Arrange
        var response = _repository.Delete(userWithTasks.UserId);
        //Act
        Assert.Equal(Response.Conflict, response);

    }

    [Fact]
    public void Delete_User_Without_Tasks()
    {
        var users = GetUsers();
        _context.Users.AddRange(users);
        var userWithoutTasks = users[0];
        //Arrange
        var response = _repository.Delete(userWithoutTasks.UserId);
        //Act
        Assert.Equal(Response.Deleted, response);
    }


    [Fact]
    public void Delete_User_With_Tasks_Force()
    {
        var users = GetUsers();
        _context.Users.AddRange(users);
        var userWithTasks = users[1];
        //Arrange
        var response = _repository.Delete(userWithTasks.UserId, true);
        //Act
        Assert.Equal(Response.Deleted, response);
    }

    [Fact]
    public void Delete_User_Doesnt_Exist()
    {
        var users = GetUsers();
        _context.Users.AddRange(users);
        //Arrange
        var response = _repository.Delete(20);
        //Act
        Assert.Equal(Response.NotFound, response);
    }

    [Fact]
    public void Find_User_Exists()
    {
        var users = GetUsers();
        _context.Users.AddRange(users);
        //Arrange
        var user = _repository.Find(users[0].UserId);
        var converted = new User();
        converted.UserId = user.Id;
        converted.Name = user.Name;
        converted.Email = user.Email;

        //Act
        Assert.Equal(users[0], converted);
    }

    [Fact]
    public void Find_User_Doesnt_Exist()
    {
        var users = GetUsers();
        _context.Users.AddRange(users);
        //Arrange
        var user = _repository.Find(20);
        //Act
        Assert.Null(user);
    }

    [Fact]
    public void Read_All_Users()
    {
        var users = GetUsers();
        _context.Users.AddRange(users);
        //Arrange
        var allUsers = _repository.Read();
        var converted = new List<User>();
        foreach (var user in allUsers)
        {
            var convertedUser = new User();
            convertedUser.UserId = user.Id;
            convertedUser.Name = user.Name;
            convertedUser.Email = user.Email;
            converted.Add(convertedUser);
        }

        //Act
        Assert.Equal(users, converted);
    }


    private List<Task> GetTasks()
    {
        var task1 = new Task();
        task1.TaskId = 0;
        task1.Title = "Task1";
        task1.Description = "Description1";
        task1.Created = DateTime.Now;
        task1.State = State.New;

        var task2 = new Task();
        task2.TaskId = 1;
        task2.Title = "Task2";
        task2.Description = "Description2";
        task2.Created = DateTime.Now;
        task2.State = State.Active;

        return new List<Task> { task1, task2 };
    }
    private List<User> GetUsers()
    {
        var tasks = GetTasks();

        var tag1 = new Tag();
        tag1.TagId = 0;
        tag1.Name = "Tag1";

        var tag2 = new Tag();
        tag2.TagId = 1;
        tag2.Name = "Tag2";

        var user1 = new User();
        user1.UserId = 0;
        user1.Name = "User1";
        user1.Email = "user1@example.com";

        var hasTasks = new User();
        hasTasks.UserId = 1;
        hasTasks.Name = "User2";
        hasTasks.Email = "user2@example.com";
        hasTasks.Tasks = new List<Task>() { tasks[0], tasks[1] };

        var user3 = new User();
        user3.UserId = 2;
        user3.Name = "user3";
        user3.Email = "user3@example.com";

        return new List<User>() { user1, hasTasks, user3 };
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
