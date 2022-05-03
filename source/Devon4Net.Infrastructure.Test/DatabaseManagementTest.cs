using Microsoft.EntityFrameworkCore;

namespace Devon4Net.Infrastructure.Test
{
    public abstract class DatabaseManagementTest<T> : BaseManagementTest where T : DbContext
    {
        public T Context { get; set; }

        protected DbContextOptions<T> ContextOptions { get; set; }

        public DatabaseManagementTest()
        {
            ConfigureContext();
            SeedData();
        }

        public abstract void ConfigureContext();

        public abstract void SeedData();
    }
}
