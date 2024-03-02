using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectManagementSystem
{
    public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(/*configuration.GetConnectionString("sqlConnection")*/"Server=127.0.0.1;Database=PMS;User Id=SA;Password=12230500o90P;TrustServerCertificate=True",
                    b => b.MigrationsAssembly("ProjectManagementSystem"));
            return new DataContext(builder.Options);
        }
    }
}
