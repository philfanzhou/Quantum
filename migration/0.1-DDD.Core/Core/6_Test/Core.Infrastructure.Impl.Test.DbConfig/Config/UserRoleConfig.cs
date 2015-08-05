namespace Core.Infrastructure.Impl.Test.DbConfig
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.UserContext;

    public class UserRoleConfig : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfig()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.UserId)
                .IsRequired();

            this.Property(t => t.RoleId)
                .IsRequired();
        }
    }
}