using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF.Services
{
    public class DbService : DbContext
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<Coverage> Coverages { get; set; }

        public DbService(DbContextOptions options) : base(options)
        {
            if (Coverages.Count() == 0)
            {
                Coverages.AddRange([new Coverage()
                {
                    Id = (int)CoverageType.Surgery,
                    Min = 5000,
                    Max = 500000000,
                    Type = CoverageType.Surgery,
                    Coefficient = 0.0052m
                },
                new Coverage()
                {
                    Id = (int)CoverageType.Dental,
                    Min = 4000,
                    Max = 400000000,
                    Type = CoverageType.Dental,
                    Coefficient = 0.0042m
                },
                new Coverage()
                {
                    Id = (int)CoverageType.Hospital,
                    Min = 2000,
                    Max = 200000000,
                    Type = CoverageType.Hospital,
                    Coefficient = 0.005m
                }]);

                SaveChanges();
            }
        }
    }
}
