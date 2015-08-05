namespace Core.Infrastructure.Impl.Test.DbConfig
{
    using Domain.UserContext;
    using System.Data.Entity.ModelConfiguration;

    public class RoleConfig : EntityTypeConfiguration<Role>
    {
        public RoleConfig()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Name)
                .HasMaxLength(20)
                .IsRequired();

            this.Property(t => t.Description)
                .HasMaxLength(100);
        }
    }
}
