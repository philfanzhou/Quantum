using Framework.Infrastructure.Repository.EntityFramework;

namespace Quantum.Infrastructure.EntityFramework.Configuration
{
    public static class DatabaseHelper
    {
        public static void Initialize(bool clearDatabase)
        {
            DbContextHelper.RegisterDbContext<QuantumDbContext>();

            if(clearDatabase)
            {
                QuantumDbContext dbContext = new QuantumDbContext();
                dbContext.Database.Delete();
                dbContext.Database.Create();
            }
        }
    }
}
