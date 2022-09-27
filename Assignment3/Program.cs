// See https://aka.ms/new-console-template for more information
// var connectionString = "Server=localhost;Database=Kanban;User Id=sa;Password=Strong_Passw0rd;Trusted_Connection=False;Encrypt=False";
using Microsoft.Extensions.Configuration;
//init
var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var connectionString = configuration.GetConnectionString("ConnectionString");

var factory = new KanbanContextFactory();
var context = factory.CreateDbContext(args);

//var e = from a in context.Tags where true select a;
var tagRepository = new TagRepository(context);
var taskRepository = new TaskRepository(context);
var userRepository = new UserRepository(context);

