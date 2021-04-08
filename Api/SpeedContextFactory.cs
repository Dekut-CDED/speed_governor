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
            optionsBuilder.UseMySql("Server=41.89.227.168;Database=speedgovernor;Uid=root;Pwd=example;");
            return new DataContext(optionsBuilder.Options);
        }
    }
}