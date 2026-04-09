using MediaBizzare.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MediaBizzare.Data
{
    public static class DbSeeder
    {
        public static void Seed(MediaBizzareContext context)
        {
            context.Database.Migrate();

            SeedUsers(context);
            SeedDepartments(context);
            SeedCategories(context);
            SeedProducts(context);
            SeedProductVariations(context);
            SeedEmployees(context);
            SeedRoles(context);
            SeedEmployeeRoles(context);
            SeedEmployeeContracts(context);
            AssignDepartmentManagers(context);
        }

        private static void SeedUsers(MediaBizzareContext context)
        {
            if (context.User.Any())
                return;

            context.User.AddRange(
                new User
                {
                    username = "jdoe",
                    name = "John",
                    surname = "Doe",
                    phone = "0611111111",
                    email = "john@example.com",
                    bank_account = "NL00BANK0123456789",
                    password_hash = "hash1",
                    street = "Main Street",
                    street_number = "1",
                    postal_code = "1234AB",
                    city = "Oosterhout",
                    country = "Netherlands"
                },
                new User
                {
                    username = "janed",
                    name = "Jane",
                    surname = "Doe",
                    phone = "0622222222",
                    email = "jane@example.com",
                    bank_account = "NL00BANK9876543210",
                    password_hash = "hash2",
                    street = "Second Street",
                    street_number = "2",
                    postal_code = "5678CD",
                    city = "Breda",
                    country = "Netherlands"
                },
                new User
                {
                    username = "mdoe",
                    name = "Mark",
                    surname = "Doe",
                    phone = "0633333333",
                    email = "mark@example.com",
                    bank_account = "NL00BANK1234567890",
                    password_hash = "hash3",
                    street = "Third Street",
                    street_number = "3",
                    postal_code = "9012EF",
                    city = "Tilburg",
                    country = "Netherlands"
                }
            );

            context.SaveChanges();
        }

        private static void SeedDepartments(MediaBizzareContext context)
        {
            if (context.Departments.Any())
                return;

            context.Departments.AddRange(
                new Department
                {
                    Name = "Electronics",
                    Slug = "electronics",
                    Description = "Electronics department"
                },
                new Department
                {
                    Name = "Computers",
                    Slug = "computers",
                    Description = "Computers department"
                }
            );

            context.SaveChanges();
        }

        private static void SeedCategories(MediaBizzareContext context)
        {
            if (context.Categories.Any())
                return;

            var electronicsDept = context.Departments.First(d => d.Slug == "electronics");
            var computersDept = context.Departments.First(d => d.Slug == "computers");

            context.Categories.AddRange(
                new Category
                {
                    Name = "TVs",
                    Slug = "tvs",
                    DepartmentId = electronicsDept.Id
                },
                new Category
                {
                    Name = "Audio",
                    Slug = "audio",
                    DepartmentId = electronicsDept.Id
                },
                new Category
                {
                    Name = "Laptops",
                    Slug = "laptops",
                    DepartmentId = computersDept.Id
                }
            );

            context.SaveChanges();
        }

        private static void SeedProducts(MediaBizzareContext context)
        {
            if (context.Products.Any())
                return;

            var tvCategory = context.Categories.First(c => c.Slug == "tvs");
            var audioCategory = context.Categories.First(c => c.Slug == "audio");
            var laptopCategory = context.Categories.First(c => c.Slug == "laptops");

            context.Products.AddRange(
                new Product
                {
                    Name = "LG C3 OLED 55",
                    Slug = "lg-c3-oled-55",
                    Description = "55 inch OLED television",
                    CategoryId = tvCategory.Id
                },
                new Product
                {
                    Name = "Samsung Q600 Soundbar",
                    Slug = "samsung-q600-soundbar",
                    Description = "Soundbar with wireless subwoofer",
                    CategoryId = audioCategory.Id
                },
                new Product
                {
                    Name = "Lenovo ThinkPad E14",
                    Slug = "lenovo-thinkpad-e14",
                    Description = "14 inch business laptop",
                    CategoryId = laptopCategory.Id
                }
            );

            context.SaveChanges();
        }

        private static void SeedProductVariations(MediaBizzareContext context)
        {
            if (context.ProductVariations.Any())
                return;

            var lgTv = context.Products.First(p => p.Slug == "lg-c3-oled-55");
            var soundbar = context.Products.First(p => p.Slug == "samsung-q600-soundbar");
            var laptop = context.Products.First(p => p.Slug == "lenovo-thinkpad-e14");

            context.ProductVariations.AddRange(
                new ProductVariations
                {
                    ProductId = lgTv.Id,
                    SKU = "TV-LG-C3-55",
                    Price = 1299.99m,
                    Stock = 12
                },
                new ProductVariations
                {
                    ProductId = soundbar.Id,
                    SKU = "AUD-SAM-Q600",
                    Price = 349.99m,
                    Stock = 20
                },
                new ProductVariations
                {
                    ProductId = laptop.Id,
                    SKU = "LAP-LEN-E14-I5-16GB",
                    Price = 899.99m,
                    Stock = 15
                },
                new ProductVariations
                {
                    ProductId = laptop.Id,
                    SKU = "LAP-LEN-E14-I7-32GB",
                    Price = 1199.99m,
                    Stock = 8
                }
            );

            context.SaveChanges();
        }

        private static void SeedEmployees(MediaBizzareContext context)
        {
            if (context.Employees.Any())
                return;

            var electronicsDept = context.Departments.First(d => d.Slug == "electronics");
            var computersDept = context.Departments.First(d => d.Slug == "computers");

            var johnUser = context.User.First(u => u.username == "jdoe");
            var janeUser = context.User.First(u => u.username == "janed");
            var markUser = context.User.First(u => u.username == "mdoe");

            context.Employees.AddRange(
                new Employee
                {
                    UserId = johnUser.Id,
                    DepartmentId = electronicsDept.Id,
                    JobTitle = "Department Manager",
                    EmergencyContact = "John Doe",
                    EmergencyPhone = "0611111111"
                },
                new Employee
                {
                    UserId = janeUser.Id,
                    DepartmentId = computersDept.Id,
                    JobTitle = "Department Manager",
                    EmergencyContact = "Jane Doe",
                    EmergencyPhone = "0622222222"
                },
                new Employee
                {
                    UserId = markUser.Id,
                    DepartmentId = computersDept.Id,
                    JobTitle = "Sales Employee",
                    EmergencyContact = "Mark Doe",
                    EmergencyPhone = "0633333333"
                }
            );

            context.SaveChanges();
        }

        private static void SeedRoles(MediaBizzareContext context)
        {
            if (context.Roles.Any())
                return;

            context.Roles.AddRange(
                new Role
                {
                    Name = "Manager",
                    Description = "Can manage department-level data and staff"
                },
                new Role
                {
                    Name = "Sales",
                    Description = "Can view and update product and sales-related data"
                }
            );

            context.SaveChanges();
        }

        private static void SeedEmployeeRoles(MediaBizzareContext context)
        {
            if (context.EmployeeRoles.Any())
                return;

            var managerRole = context.Roles.First(r => r.Name == "Manager");
            var salesRole = context.Roles.First(r => r.Name == "Sales");

            var johnUserId = context.User.First(u => u.username == "jdoe").Id;
            var janeUserId = context.User.First(u => u.username == "janed").Id;
            var markUserId = context.User.First(u => u.username == "mdoe").Id;

            var johnEmployee = context.Employees.First(e => e.UserId == johnUserId);
            var janeEmployee = context.Employees.First(e => e.UserId == janeUserId);
            var markEmployee = context.Employees.First(e => e.UserId == markUserId);

            context.EmployeeRoles.AddRange(
                new EmployeeRole
                {
                    EmployeeId = johnEmployee.Id,
                    RoleId = managerRole.Id
                },
                new EmployeeRole
                {
                    EmployeeId = janeEmployee.Id,
                    RoleId = managerRole.Id
                },
                new EmployeeRole
                {
                    EmployeeId = markEmployee.Id,
                    RoleId = salesRole.Id
                }
            );

            context.SaveChanges();
        }

        private static void SeedEmployeeContracts(MediaBizzareContext context)
        {
            if (context.EmployeeContracts.Any())
                return;

            var johnUserId = context.User.First(u => u.username == "jdoe").Id;
            var janeUserId = context.User.First(u => u.username == "janed").Id;
            var markUserId = context.User.First(u => u.username == "mdoe").Id;

            var johnEmployee = context.Employees.First(e => e.UserId == johnUserId);
            var janeEmployee = context.Employees.First(e => e.UserId == janeUserId);
            var markEmployee = context.Employees.First(e => e.UserId == markUserId);

            context.EmployeeContracts.AddRange(
                new Employee_Contract
                {
                    EmployeeId = johnEmployee.Id,
                    signature_date = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc),
                    start_date = DateTime.SpecifyKind(new DateTime(2025, 1, 15), DateTimeKind.Utc),
                    end_date = DateTime.SpecifyKind(new DateTime(2026, 1, 15), DateTimeKind.Utc ),
                    salary = 3500,
                    hours_per_week = 40,
                    contract_type = "Full-time"
                },
                new Employee_Contract
                {
                    EmployeeId = janeEmployee.Id,
                    signature_date = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc),
                    start_date = DateTime.SpecifyKind(new DateTime(2025, 1, 15), DateTimeKind.Utc),
                    end_date = DateTime.SpecifyKind(new DateTime(2026, 1, 15), DateTimeKind.Utc ),
                    salary = 3500,
                    hours_per_week = 40,
                    contract_type = "Full-time"
                },
                new Employee_Contract
                {
                    EmployeeId = markEmployee.Id,
                    signature_date = DateTime.SpecifyKind(new DateTime(2025, 2, 1), DateTimeKind.Utc),
                    start_date = DateTime.SpecifyKind(new DateTime(2025, 2, 15), DateTimeKind.Utc),
                    end_date = DateTime.SpecifyKind(new DateTime(2026, 2, 15), DateTimeKind.Utc),
                    salary = 2600,
                    hours_per_week = 32,
                    contract_type = "Part-time"
                }
            );

            context.SaveChanges();
        }

        private static void AssignDepartmentManagers(MediaBizzareContext context)
        {
            var electronicsDept = context.Departments.First(d => d.Slug == "electronics");
            var computersDept = context.Departments.First(d => d.Slug == "computers");

            var johnUserId = context.User.First(u => u.username == "jdoe").Id;
            var janeUserId = context.User.First(u => u.username == "janed").Id;

            var johnEmployee = context.Employees.First(e => e.UserId == johnUserId);
            var janeEmployee = context.Employees.First(e => e.UserId == janeUserId);

            var changed = false;

            if (electronicsDept.ManagerId != johnEmployee.Id)
            {
                electronicsDept.ManagerId = johnEmployee.Id;
                changed = true;
            }

            if (computersDept.ManagerId != janeEmployee.Id)
            {
                computersDept.ManagerId = janeEmployee.Id;
                changed = true;
            }

            if (changed)
                context.SaveChanges();
        }
    }
}