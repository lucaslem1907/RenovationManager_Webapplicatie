using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DatabaseContext : DbContext
    {

        public DbSet<User> Users => Set<User>();
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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasMany(u => u.Projects)
                .WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(p => p.Budget)
                      .HasColumnType("decimal(18,2)"); 

                entity.HasMany(p => p.Rooms)
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            });
                

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);
                
                entity.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasMany(r => r.Tasks)
                .WithOne(t => t.Room)
                .HasForeignKey(t => t.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
            });
                

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.HasMany(t => t.Subtasks)
                .WithOne(s => s.TaskItem)
                .HasForeignKey(s => s.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            


            modelBuilder.Entity<Expense>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)");  // Use HasColumnType for PostgreSQL

                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Project)
                .WithMany(p => p.Expenses)
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Room)
                .WithMany(r => r.Expenses)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }

    }
}
