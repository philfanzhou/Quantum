using Quantum.Infrastructure.Trading.Repository;
using System.Data.Entity.ModelConfiguration;

namespace Quantum.Infrastructure.EntityFramework.Configuration
{
    public class AccountDataConfig : EntityTypeConfiguration<AccountData>
    {
        public AccountDataConfig()
        {
            this.HasKey(account => account.Id);

            this.Property(account => account.Name)
                .HasMaxLength(20)
                .IsRequired();

            this.Property(account => account.Balance)
                .IsRequired();
        }
    }
}
