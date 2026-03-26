
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

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department -> Categories (one-to-many, optional on Category side)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Categories)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Employee -> Department (many employees work in one department)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department -> Manager (one department has one manager)
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithOne(e => e.ManagedDepartment)
                .HasForeignKey<Department>(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            // enforce one manager per department and one managed department per employee
            modelBuilder.Entity<Department>()
                .HasIndex(d => d.ManagerId)
                .IsUnique();
        }
    }
}