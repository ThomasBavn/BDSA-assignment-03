using Assignment3.Core;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities;

public class KanbanContext : DbContext
{

    public KanbanContext(DbContextOptions<KanbanContext> options): base(options)
    { }

    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Task> Tasks { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIdentityColumns();

        modelBuilder.Entity<Tag>(entity =>
        {
            //Name : string(50), required, unique
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();

            //Tasks : many-to-many reference to Task entity
            entity.HasMany(e => e.Tasks)
                .WithMany(e => e.Tags)
                .UsingEntity(e => e.ToTable("TaskTags"));

        });

        modelBuilder.Entity<Task>(entity =>
        {
            //Title : string(100), required
            entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
            //Description : string(max), optional
            entity.Property(e => e.Description).HasMaxLength(Int32.MaxValue);

            //State : enum (New, Active, Resolved, Closed, Removed), required
            entity.Property(e => e.State)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (State)Enum.Parse(typeof(State), v));
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
        });
    }

}
