using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Persistence;

namespace Kaizen.Api
{
    public class SpeedContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=tcp:kaizenserver.database.windows.net,1433;Initial Catalog=cded;Persist Security Info=False;User ID=kaizenadmin;Password=pK}w/5cfF<RwvRjWGSB5RJ=^^;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            return new DataContext(optionsBuilder.Options);
        }
    }
}