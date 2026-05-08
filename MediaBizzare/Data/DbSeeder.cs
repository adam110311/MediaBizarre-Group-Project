using MediaBizzare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MediaBizzare.Data
{
    public static class DbSeeder
    {
        // Async because UserManager / RoleManager methods are async.
        public static async Task SeedAsync(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            await context.Database.MigrateAsync();

            await SeedIdentityRolesAsync(roleManager);
            await SeedUsersAsync(context, userManager);

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

        // Identity roles ("Admin", "Customer") — these are the AUTH roles.
        // Distinct from the existing Role/EmployeeRole entities which are HR job roles.
        private static async Task SeedIdentityRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            foreach (var roleName in new[] { "Admin", "Customer" })
            {
                if (await roleManager.RoleExistsAsync(roleName))
                    return;

                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }

        // Seeds 3 Identity users (the same john/jane/mark from before).
        // All three are seeded as Admins because they're staff in this dataset.
        // Default password for all seeded users: ChangeMe123!  -> change in production.
        private static async Task SeedUsersAsync(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.Users.Any())
                return;

            var seedUsers = new (ApplicationUser user, string password)[]
            {
                (new ApplicationUser
                {
                    UserName = "jdoe",
                    Email = "john@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0611111111",
                    Name = "John",
                    Surname = "Doe",
                    BankAccount = "NL00BANK0123456789",
                    Street = "Main Street",
                    StreetNumber = "1",
                    PostalCode = "1234AB",
                    City = "Oosterhout",
                    Country = "Netherlands"
                }, "ChangeMe123!"),
                (new ApplicationUser
                {
                    UserName = "janed",
                    Email = "jane@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0622222222",
                    Name = "Jane",
                    Surname = "Doe",
                    BankAccount = "NL00BANK9876543210",
                    Street = "Second Street",
                    StreetNumber = "2",
                    PostalCode = "5678CD",
                    City = "Breda",
                    Country = "Netherlands"
                }, "ChangeMe123!"),
                (new ApplicationUser
                {
                    UserName = "mdoe",
                    Email = "mark@example.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0633333333",
                    Name = "Mark",
                    Surname = "Doe",
                    BankAccount = "NL00BANK1234567890",
                    Street = "Third Street",
                    StreetNumber = "3",
                    PostalCode = "9012EF",
                    City = "Tilburg",
                    Country = "Netherlands"
                }, "ChangeMe123!")
            };

            foreach (var (user, password) in seedUsers)
            {
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Seeding user {user.UserName} failed: {errors}");
                }

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        private static void SeedDepartments(AppDbContext context)
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

        private static void SeedCategories(AppDbContext context)
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

        private static void SeedProducts(AppDbContext context)
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

        private static void SeedProductVariations(AppDbContext context)
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

        private static void SeedEmployees(AppDbContext context)
        {
            if (context.Employees.Any())
                return;

            var electronicsDept = context.Departments.First(d => d.Slug == "electronics");
            var computersDept = context.Departments.First(d => d.Slug == "computers");

            // UserName replaces the old `username` field
            var johnUser = context.Users.First(u => u.UserName == "jdoe");
            var janeUser = context.Users.First(u => u.UserName == "janed");
            var markUser = context.Users.First(u => u.UserName == "mdoe");

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

        // HR Roles (job roles), distinct from Identity Admin/Customer roles
        private static void SeedRoles(AppDbContext context)
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

        private static void SeedEmployeeRoles(AppDbContext context)
        {
            if (context.EmployeeRoles.Any())
                return;

            var managerRole = context.Roles.First(r => r.Name == "Manager");
            var salesRole = context.Roles.First(r => r.Name == "Sales");

            var johnUserId = context.Users.First(u => u.UserName == "jdoe").Id;
            var janeUserId = context.Users.First(u => u.UserName == "janed").Id;
            var markUserId = context.Users.First(u => u.UserName == "mdoe").Id;

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

        private static void SeedEmployeeContracts(AppDbContext context)
        {
            if (context.EmployeeContracts.Any())
                return;

            var johnUserId = context.Users.First(u => u.UserName == "jdoe").Id;
            var janeUserId = context.Users.First(u => u.UserName == "janed").Id;
            var markUserId = context.Users.First(u => u.UserName == "mdoe").Id;

            var johnEmployee = context.Employees.First(e => e.UserId == johnUserId);
            var janeEmployee = context.Employees.First(e => e.UserId == janeUserId);
            var markEmployee = context.Employees.First(e => e.UserId == markUserId);

            context.EmployeeContracts.AddRange(
                new Employee_Contract
                {
                    EmployeeId = johnEmployee.Id,
                    signature_date = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc),
                    start_date = DateTime.SpecifyKind(new DateTime(2025, 1, 15), DateTimeKind.Utc),
                    end_date = DateTime.SpecifyKind(new DateTime(2026, 1, 15), DateTimeKind.Utc),
                    salary = 3500,
                    hours_per_week = 40,
                    contract_type = "Full-time"
                },
                new Employee_Contract
                {
                    EmployeeId = janeEmployee.Id,
                    signature_date = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc),
                    start_date = DateTime.SpecifyKind(new DateTime(2025, 1, 15), DateTimeKind.Utc),
                    end_date = DateTime.SpecifyKind(new DateTime(2026, 1, 15), DateTimeKind.Utc),
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

        private static void AssignDepartmentManagers(AppDbContext context)
        {
            var electronicsDept = context.Departments.First(d => d.Slug == "electronics");
            var computersDept = context.Departments.First(d => d.Slug == "computers");

            var johnUserId = context.Users.First(u => u.UserName == "jdoe").Id;
            var janeUserId = context.Users.First(u => u.UserName == "janed").Id;

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