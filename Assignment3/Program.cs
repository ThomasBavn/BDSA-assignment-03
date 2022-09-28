// See https://aka.ms/new-console-template for more information
// var connectionString = "Server=localhost;Database=Kanban;User Id=sa;Password=Strong_Passw0rd;Trusted_Connection=False;Encrypt=False";
using Microsoft.Extensions.Configuration;
using Assignment3.Entities;
using Assignment3.Core;
using Task = Assignment3.Entities.Task;
using Microsoft.EntityFrameworkCore;
//init
namespace Assignment3;
public class Program
{

    public static TagRepository TagRepository { get; set; } = null!;
    public static TaskRepository TaskRepository { get; set; } = null!;
    public static UserRepository UserRepository { get; set; } = null!;
    public static KanbanContext Context { get; set; } = null!;

    static int userIndex = 0;
    static int taskIndex = 0;
    static int tagIndex = 0;

    private static void Main(string[] args)
    {

        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var factory = new KanbanContextFactory();
        var context = factory.CreateDbContext(args);

        //var e = from a in context.Tags where true select a;
        Context = context;
        //print all user ids
        
        context.Users.RemoveRange(context.Users);
        context.Tasks.RemoveRange(context.Tasks);
        context.Tags.RemoveRange(context.Tags);
        context.SaveChanges();
        context.Users.AddRange(GetUsers());
        context.Tasks.AddRange(GetTasks());
        context.Tags.AddRange(GetTags());

        context.SaveChanges();



        TagRepository = new TagRepository(context);
        TaskRepository = new TaskRepository(context);
        UserRepository = new UserRepository(context);


        /*userIndex = context.Users.Max(u => u.Id) + 1;
        taskIndex = context.Tasks.Max(t => t.Id) + 1;
        tagIndex = context.Tags.Max(t => t.Id) + 1;*/

        //user tests
        Console.WriteLine("User tests start");
        Create_User_Already_Exists();
        Create_Tag_Success();
        Delete_User_With_Tasks();
        Delete_User_Without_Tasks();
        Delete_User_With_Tasks_Force();
        Delete_User_Doesnt_Exist();
        Find_User_Exists();
        Find_User_Doesnt_Exist();
        Read_All_Users();
        Console.WriteLine("User tests done");
        //task tests
        Create_Task_Already_Exists();
        Create_Task_Success();
        Read_Check_If_Returns_Everything();
        Find_Task_By_ID();
        Find_Task_By_ID_Doesnt_Exist();
        Console.WriteLine("Task tests done");
        //Tag tests
        Create_Tag_Already_Exists();
        Create_Tag_Success();
        Read_Check_If_Returns_Everything_Tag();
        Delete_Tag_With_Tasks();
        Delete_Tag_Without_Tasks();
        Delete_Tag_With_Tasks_Force();
        Delete_Tag_Doesnt_Exist();
        Find_Tag_Exist();
        Find_Tag_Doesnt_Exist();
        Update_Tag_Exist();
        Update_Tag_Not_Exist();

        Console.WriteLine("Tag tests done");

        context.Dispose();
    }




    //Making some tests here after spending 2 full days trying to make sqlite work without success


    ///////// TEST FOR USERS /////////
    public static void Create_User_Already_Exists()
    {
        //var users = GetUsers();
        //users.ForEach(u => userRepository.Create(new UserCreateDTO(u.Name, u.Email)));
        //Arrange
        var user = new UserCreateDTO(
            "User1",
            "user1@example.com"
            );

        //Act
        var response = UserRepository.Create(user);

        //Assert
        Assert.Equal((Response.Conflict, 0), response);
    }

    public static void Create_User_Success()
    {
        //Arrange
        var user = new UserCreateDTO(
            "NewUser",
            "example@itu.dk");

        //Act
        var response = UserRepository.Create(user);
        userIndex++;
        //Assert
        Assert.Equal((Response.Created, Context.Users.Count()), response);
    }

    public static void Delete_User_With_Tasks()
    {
        var response = UserRepository.Delete(1);
        Assert.Equal(Response.Conflict, response);
    }
    public static void Delete_User_Without_Tasks()
    {
        //create user to delete
        var user = new UserCreateDTO(
            "NewUser",
            "verynew@mail.dk");
        UserRepository.Create(user);

        var response = UserRepository.Delete(userIndex);
        Assert.Equal(Response.Deleted, response);
    }
    public static void Delete_User_With_Tasks_Force()
    {
        //create user to delete
        var user = new UserCreateDTO(
            "NewUser",
            "verynew@mail.dk");
        UserRepository.Create(user);
        var response = UserRepository.Delete(userIndex, true);
        Assert.Equal(Response.Deleted, response);
    }

    public static void Delete_User_Doesnt_Exist()
    {
        var response = UserRepository.Delete(200);
        Assert.Equal(Response.NotFound, response);
    }

    public static void Find_User_Exists()
    {
        var response = UserRepository.Find(1);
        Assert.Equal(GetUsers()[1].Name, response.Name);
    }
    public static void Find_User_Doesnt_Exist()
    {
        var response = UserRepository.Find(200);
        Assert.Null(response);
    }

    public static void Read_All_Users()
    {
        var response = UserRepository.Read();
        Assert.Equal(Context.Users.Count(), response.Count);
    }

    ///////// TEST TIL TASK /////////

    public static void Create_Task_Already_Exists()
    {
        //Arrange
        var task = new TaskCreateDTO(
            "Task1",
            0,
            "Description1",
            new List<string> { "DISYS", "Assignment3" }
            );

        //Act
        var response = TaskRepository.Create(task);

        //Assert
        Assert.Equal((Response.Conflict, Context.Tasks.Count()), response);
    }

    public static void Create_Task_Success()
    {
        //Arrange
        var task = new TaskCreateDTO(
            "NewTask",
            0,
            "Description1",
            new List<string> { }
            );

        //Act
        var response = TaskRepository.Create(task);
        taskIndex++;
        //Assert
        Assert.Equal((Response.Created, Context.Tasks.Count()), response);
    }

    public static void Read_Check_If_Returns_Everything()
    {
        //Arrange
        var tasks = Context.Tasks;

        //Act
        var response = TaskRepository.Read();

        //Assert
        Assert.Equal(tasks.Count(), response.Count);
    }

    public static void Find_Task_By_ID()
    {
        //Arrange
        var task = GetTasks()[1];

        //Act
        var response = TaskRepository.Find(task.Id);

        //Assert
        Assert.Equal(task.Title, response.Title);
    }

    public static void Find_Task_By_ID_Doesnt_Exist()
    {
        //Arrange
        var task = GetTasks()[1];

        //Act
        var response = TaskRepository.Find(20);

        //Assert
        Assert.Null(response);
    }



    ///////// TEST TIL TAG /////////

    public static void Create_Tag_Already_Exists()
    {
        //Arrange
        var tag = new TagCreateDTO(
            "Tag1"
            );

        //Act
        var response = TagRepository.Create(tag);

        //Assert
        Assert.Equal(Response.Conflict, response.Response);
    }

    public static void Create_Tag_Success()
    {
        //Arrange
        var tag = new TagCreateDTO(
            "NewTag"
            );

        //Act
        var response = TagRepository.Create(tag);
        tagIndex++;
        //Assert
        Assert.Equal(Response.Created, response.Response);
    }

    public static void Read_Check_If_Returns_Everything_Tag()
    {
        //Arrange
        var tags = GetTags();

        //Act
        var response = TagRepository.Read();

        //Assert
        Assert.Equal(Context.Tags.Count(), response.Count);
    }

    public static void Find_Tag_Exist()
    {
        var response = TagRepository.Find(1);
        Assert.Equal("Tag1", response.Name);
    }

    public static void Find_Tag_Doesnt_Exist()
    {
        var response = TagRepository.Find(420);
        Assert.Null(response);
    }

    public static void Delete_Tag_With_Tasks()
    {
        var response = TagRepository.Delete(1);
        Assert.Equal(Response.Conflict, response);
    }

    public static void Delete_Tag_Without_Tasks()
    {
        //create new tag to delete
        var tag = new TagCreateDTO(
            "NewTag"
            );
        TagRepository.Create(tag);

        var response = TagRepository.Delete(tagIndex);
        Assert.Equal(Response.Deleted, response);
    }

    public static void Delete_Tag_With_Tasks_Force()
    {
        var response = TagRepository.Delete(1, true);
        Assert.Equal(Response.Deleted, response);
    }

    public static void Delete_Tag_Doesnt_Exist()
    {
        var response = TagRepository.Delete(20);
        Assert.Equal(Response.NotFound, response);
    }

    public static void Update_Tag_Not_Exist()
    {
        var tag = new TagUpdateDTO(
            20,
            "New tag"
            );
        var response = TagRepository.Update(tag);
        Assert.Equal(Response.NotFound, response);
    }

    public static void Update_Tag_Exist()
    {
        var tag = new TagUpdateDTO(
            1,
            "New tag"
            );
        var response = TagRepository.Update(tag);
        Assert.Equal(Response.Updated, response);
    }







    ///////// SET UP /////////
    public static List<Task> GetTasks()
    {
        var task1 = new Task();
        //task1.TaskId = 0;
        task1.Title = "Task1";
        task1.Description = "Description1";
        task1.Created = DateTime.UtcNow;
        task1.State = State.New;

        var task2 = new Task();
        //task2.TaskId = 1;
        task2.Title = "Task2";
        task2.Description = "Description2";
        task2.Created = DateTime.UtcNow;
        task2.State = State.Active;

        var removedTask = new Task();
        //removedTask.TaskId = 2;
        removedTask.Title = "Task3";
        removedTask.Description = "Description3";
        removedTask.Created = DateTime.UtcNow;
        removedTask.State = State.Removed;

        var hasTags = new Task();
        //hasTags.TaskId = 3;
        hasTags.Title = "Task4";
        hasTags.Description = "Description4";
        hasTags.Created = DateTime.UtcNow;
        hasTags.State = State.New;
        hasTags.Tags = GetTags();


        return new List<Task> { task1, task2, removedTask };
    }

    public static List<Tag> GetTags()
    {
        var tag1 = new Tag();
        //tag1.TagId = 0;
        tag1.Name = "Tag1";

        var tag2 = new Tag();
        //tag2.TagId = 1;
        tag2.Name = "Tag2";

        var tag3 = new Tag();
        //tag3.TagId = 2;
        tag3.Name = "Tag3";


        return new List<Tag> { tag1, tag2 };
    }

    public static List<User> GetUsers()
    {
        var tasks = GetTasks();

        var user1 = new User();
        //user1.UserId = 0;
        user1.Name = "User1";
        user1.Email = "user1@example.com";

        var hasTasks = new User();
        //hasTasks.UserId = 1;
        hasTasks.Name = "User2";
        hasTasks.Email = "user2@example.com";
        hasTasks.Tasks = tasks;

        var user3 = new User();
        //user3.UserId = 2;
        user3.Name = "user3";
        user3.Email = "user3@example.com";

        return new List<User> { user1, hasTasks, user3 };

    }
}