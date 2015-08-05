namespace Core.Infrastructure.Impl.Test.DbConfig
{
    using Domain.UserContext;
    using System.Data.Entity.ModelConfiguration;

    public class PermissionConfig : EntityTypeConfiguration<Permission>
    {
        public PermissionConfig()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.PermissionName)
                .HasMaxLength(500);

            this.Property(t => t.Description)
                .HasMaxLength(500);

            this.Property(t => t.AccessUrl)
                .HasMaxLength(500);

            this.ToTable("Permissions");
            
            //this.HasMany(t => t.Roles)
            //    .WithMany(t => t.Permissions)
            //    .Map(m =>
            //        {
            //            m.ToTable("RolePermissionRef");
            //            m.MapLeftKey("PermissionId");
            //            m.MapRightKey("RoleId");
            //        });
        }
    }
}
