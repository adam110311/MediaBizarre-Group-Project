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
        public DbSet<CategoryProduct> categoryProducts { get; set; }
        public DbSet<EmployeeRole> employeeRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProductVariations> ProductVariations { get; set; }
        public DbSet<Employee_Contract> EmployeeContracts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User <-> Employee (optional on User side, required on Employee side)
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
                .OnDelete(DeleteBehavior.SetNull);

            // Department -> Manager(Employee)
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithOne(e => e.ManagedDepartment)
                .HasForeignKey<Department>(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Department>()
                .HasIndex(d => d.ManagerId)
                .IsUnique();

            // CategoryProduct join table: Category <-> Product
            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasKey("CategoryId", "ProductId");

                entity.HasOne(cp => cp.Category)
                    .WithMany()
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cp => cp.Product)
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // EmployeeRole join table: Employee <-> Role
            modelBuilder.Entity<EmployeeRole>(entity =>
            {
                entity.HasKey("EmployeeId", "RoleId");

                entity.HasOne(er => er.Employee)
                    .WithMany()
                    .HasForeignKey("EmployeeId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(er => er.Role)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Product -> ProductVariations (one-to-many)
            modelBuilder.Entity<ProductVariations>(entity =>
            {
                entity.HasOne(pv => pv.Product)
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(pv => pv.SKU)
                    .IsRequired();

                entity.HasIndex(pv => pv.SKU)
                    .IsUnique();
            });

            // Employee -> Employee_Contract
            modelBuilder.Entity<Employee_Contract>(entity =>
            {
                entity.HasOne(ec => ec.Employee)
                    .WithOne()
                    .HasForeignKey<Employee_Contract>("EmployeeId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex("EmployeeId")
                    .IsUnique();
            });
        }
    }
}