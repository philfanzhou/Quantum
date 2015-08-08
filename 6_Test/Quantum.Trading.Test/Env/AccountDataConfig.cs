using Quantum.Infrastructure.Trading.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Trading.Test.Env
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
