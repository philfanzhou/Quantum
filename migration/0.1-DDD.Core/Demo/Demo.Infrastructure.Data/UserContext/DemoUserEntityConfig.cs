namespace Demo.Infrastructure.Data.UserContext
{
    using Domain.UserContext;
    using Extension.Infrastructure.Data.UserContext;

    public class DemoUserEntityConfig : UserConfig<DemoUser>
    {
         public DemoUserEntityConfig() : base()
         {
             this.Property(user => user.Email)
                 .HasMaxLength(50);
         }
    }
}