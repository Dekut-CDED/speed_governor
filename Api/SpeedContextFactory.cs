using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Persistence;

namespace Api
{
    public class SpeedContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=41.89.227.168;Database=speed;User=sa;Password=Password_123;MultipleActiveResultSets=true");
            return new DataContext(optionsBuilder.Options);
        }
    }
}