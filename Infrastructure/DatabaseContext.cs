using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DatabaseContext : DbContext
    {


        public DbSet<Project> RenovationProjects => Set<Project>();
        public DbSet<Room> Rooms => Set<Room>();

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        public DbSet<Subtask> Subtasks => Set<Subtask>();

        public DbSet<Expense> Expenses => Set<Expense>();

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Rooms)
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Tasks)
                .WithOne(t => t.Room)
                .HasForeignKey(t => t.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasMany(t => t.Subtasks)
                .WithOne(s => s.TaskItem)
                .HasForeignKey(s => s.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subtask>()
                .HasOne(s => s.TaskItem)
                .WithMany(t => t.Subtasks)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.Amount)
                .HasPrecision(18, 2);

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Project)
                .WithMany(p => p.Expenses)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);


            }

            );



        }

    }
}
