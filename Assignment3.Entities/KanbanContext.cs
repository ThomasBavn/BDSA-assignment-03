using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities
{
    public partial class KanbanContext : DbContext
    {

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options)
        { }

        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();

            modelBuilder.Entity<Tag>(entity =>
            {
                //Name : string(50), required, unique
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();

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
                //Name : string(100), required
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

                //Email : string(100), required, unique
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();

                //WorkItems : list of WorkItem entities belonging to User
                entity.HasMany(e => e.Tasks);
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
