namespace Assignment3.Entities.Tests;

public sealed class TagRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        _context = new KanbanContext(builder.Options);

        //_context.Database.Migrate();
        _context.Database.EnsureCreated();

        _context.SaveChanges();
        _repository = new TagRepository(_context);
    }

    [Fact]
    void Create_User_Already_Exists()
    {
        // Arrange
        //var sut = new TagCreateDTO("Send invoices");

        // Act
        //var result = X.Create(sut);

        // Assert

    }

    [Fact]
    void Read_Check_If_Returns_Everything()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    void Find_Tag_By_ID_2()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    void Update_Check_If_Updated_Correctly()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    void Delete_With_Force()
    {
        // Arrange
        // Act
        // Assert
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}
