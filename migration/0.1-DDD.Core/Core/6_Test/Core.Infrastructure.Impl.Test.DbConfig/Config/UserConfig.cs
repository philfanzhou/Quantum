namespace Core.Infrastructure.Impl.Test.DbConfig
{
    using Domain.UserContext;
    using System.Data.Entity.ModelConfiguration;

    public class UserConfig<TUserEntity> : EntityTypeConfiguration<TUserEntity>
        where TUserEntity : User
    {
         public UserConfig()
         {
             this.HasKey(user => user.Id);

             this.Property(user => user.UserMail)
                 .HasMaxLength(20)
                 .IsRequired();

             this.Property(user => user.EncryptedPassword)
                 .HasMaxLength(32)
                 .IsRequired();

             this.ToTable("Users");
         }
    }
}