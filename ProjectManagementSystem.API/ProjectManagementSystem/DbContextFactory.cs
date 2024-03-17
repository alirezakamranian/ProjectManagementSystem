using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProjectManagementSystem
{
    //public class DbContextFactory/*(IConfiguration configuration ) : IDesignTimeDbContextFactory<DataContext>*/
    //{
    //    private readonly IConfiguration _configuration = configuration;

    //    public DataContext CreateDbContext(string[] args)
    //    {
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build();
    //        var builder = new DbContextOptionsBuilder<DataContext>()
    //            .UseSqlServer(/*_configuration.GetConnectionString("SqlServer")*/"Server=127.0.0.1;Database=PMSDB;User Id=SA;Password=12230500o90P;TrustServerCertificate=True",
    //                b => b.MigrationsAssembly("ProjectManagementSystem"));
    //        return new DataContext(builder.Options);
    //    }
    //}
}
