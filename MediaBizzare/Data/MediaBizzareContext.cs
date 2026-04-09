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
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<ProductVariations> ProductVariations { get; set; }
        public DbSet<Employee_Contract> EmployeeContracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User <-> Employee
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.UserId)
                .IsUnique();

            // Department -> Employees
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department -> Categories
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Categories)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Department -> Manager(Employee)
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithOne(e => e.ManagedDepartment)
                .HasForeignKey<Department>(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Department>()
                .HasIndex(d => d.ManagerId)
                .IsUnique();

            // Category -> Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product -> ProductVariations
            modelBuilder.Entity<ProductVariations>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.Variations)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariations>()
                .HasIndex(pv => pv.SKU)
                .IsUnique();

            // Employee <-> Role many-to-many through EmployeeRole
            modelBuilder.Entity<EmployeeRole>()
                .HasKey(er => new { er.EmployeeId, er.RoleId });

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(r => r.EmployeeRoles)
                .HasForeignKey(er => er.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employee -> Contracts (one-to-many)
            modelBuilder.Entity<Employee_Contract>()
                .HasOne(ec => ec.Employee)
                .WithMany(e => e.Contracts)
                .HasForeignKey(ec => ec.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}