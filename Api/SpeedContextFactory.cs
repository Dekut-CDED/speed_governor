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
            optionsBuilder.UseSqlServer("Server=black;Database=speedgovernor3;User=sa;Password=Password_123;MultipleActiveResultSets=true");
            return new DataContext(optionsBuilder.Options);
        }
    }
}