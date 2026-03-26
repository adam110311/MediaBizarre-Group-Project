using MediaBizzare.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaBizzare.Data
{
    public static class DbSeeder
    {
        public static void Seed(MediaBizzareContext context)
        {
            context.Database.Migrate();

            if (!context.User.Any())
            {
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

            if (!context.Departments.Any())
            {
                var electronics = new Department
                {
                    Name = "Electronics",
                    Slug = "electronics",
                    Description = "Electronics department"
                };

                var computers = new Department
                {
                    Name = "Computers",
                    Slug = "computers",
                    Description = "Computers department"
                };

                context.Departments.AddRange(electronics, computers);
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
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

            if (!context.Employees.Any())
            {
                var electronicsDept = context.Departments.First(d => d.Slug == "electronics");
                var computersDept = context.Departments.First(d => d.Slug == "computers");

                var johnUser = context.User.First(u => u.username == "jdoe");
                var janeUser = context.User.First(u => u.username == "janed");
                var markUser = context.User.First(u => u.username == "mdoe");

                var emp1 = new Employee
                {
                    UserId = johnUser.Id,
                    DepartmentId = electronicsDept.Id,
                    JobTitle = "Department Manager",
                    EmergencyContact = "John Doe",
                    EmergencyPhone = "0611111111",
                    Role = "Manager"
                };

                var emp2 = new Employee
                {
                    UserId = janeUser.Id,
                    DepartmentId = computersDept.Id,
                    JobTitle = "Department Manager",
                    EmergencyContact = "Jane Doe",
                    EmergencyPhone = "0622222222",
                    Role = "Manager"
                };

                var emp3 = new Employee
                {
                    UserId = markUser.Id,
                    DepartmentId = computersDept.Id,
                    JobTitle = "Sales Employee",
                    EmergencyContact = "Mark Doe",
                    EmergencyPhone = "0633333333",
                    Role = "Employee"
                };

                context.Employees.AddRange(emp1, emp2, emp3);
                context.SaveChanges();

                electronicsDept.ManagerId = emp1.Id;
                computersDept.ManagerId = emp2.Id;

                context.SaveChanges();
            }
        }
    }
}