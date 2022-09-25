// See https://aka.ms/new-console-template for more information
// var connectionString = "Server=localhost;Database=Kanban;User Id=sa;Password=Strong_Passw0rd;Trusted_Connection=False;Encrypt=False";
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var connectionString = configuration.GetConnectionString("ConnectionString");
