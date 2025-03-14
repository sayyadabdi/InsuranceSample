using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF.Services
{
    public class DbService : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public DbService(DbContextOptions options) : base(options)
        {

        }
    }
}
