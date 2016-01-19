using System.Data.Entity.ModelConfiguration;

namespace Quantum.Infrastructure.EF.Trading.Config
{
    internal class AccountConfig : EntityTypeConfiguration<AccountDbo>
    {
        public AccountConfig()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.Name).IsRequired().HasMaxLength(10);
            this.Property(p => p.Principal).IsRequired();
            this.Property(p => p.Balance).IsRequired();
        }
    }
}
