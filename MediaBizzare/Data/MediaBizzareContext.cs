
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
    }
}