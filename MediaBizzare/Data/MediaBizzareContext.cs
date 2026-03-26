using MediaBizzare.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaBizzare.Data
{
    public class MediaBizzareContext : DbContext
    {
        public MediaBizzareContext(DbContextOptions<MediaBizzareContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.UserId)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Categories)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithOne(e => e.ManagedDepartment)
                .HasForeignKey<Department>(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Department>()
                .HasIndex(d => d.ManagerId)
                .IsUnique();
        }
    }
}