using Microsoft.EntityFrameworkCore;

namespace Devon4Net.Infrastructure.Test
{
    public abstract class DatabaseManagementTest<T> : BaseManagementTest, IDisposable where T : DbContext
    {
        public T Context { get; set; }

        protected DbContextOptions<T> ContextOptions { get; set; }

        public DatabaseManagementTest()
        {
            ConfigureContext();
        }

        public void Dispose() => Context.Dispose();

        public abstract void ConfigureContext();
    }
}
