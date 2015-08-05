namespace Demo.Infrastructure.Data.UserContext
{
    using Extension.Infrastructure.Data.UserContext;
    using System.Data.Entity;

    public class DemoUserUnitOfWork : UserUnitOfWork
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new DemoUserEntityConfig());
        }
    }
}