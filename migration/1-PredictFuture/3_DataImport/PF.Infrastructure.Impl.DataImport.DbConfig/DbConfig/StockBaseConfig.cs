using System.Data.Entity.ModelConfiguration;
using PF.DataImport.Domain;

namespace PF.Infrastructure.Impl.DataImport.DbConfig
{
    public class StockBaseConfig : EntityTypeConfiguration<StockBase>
    {
        public StockBaseConfig()
        {
            HasKey(s => s.Id);
            Property(s => s.Id).IsRequired().HasMaxLength(36);
            Property(s => s.Name).IsOptional().HasMaxLength(36);
            Property(s => s.Symbol).IsRequired().HasMaxLength(36);
        }
    }
}