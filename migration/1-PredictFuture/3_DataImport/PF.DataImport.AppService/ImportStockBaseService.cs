using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DataImport.AppService
{
    using Core.Domain;
    using Core.Infrastructure.Crosscutting;
    using PF.DataImport.Domain;

    public class ImportStockBaseService
    {
        public StockBase GetStockBaseBySymbol(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException("smybol");
            }

            using (var context = ContainerHelper.Instance.Resolve<IRepositoryContext>())
            {
                try
                {
                    var repository = new StockBaseRepository(context);
                    return repository.GetStockBaseBySymbol(symbol);
                }
                catch
                {
                    //这里可能会因为Repository<StockBase>.Single()找不到记录抛出异常
                    return null;
                }
            }
        }

        public void ImportStockBase(StockBase stock)
        {
            using (var context = ContainerHelper.Instance.Resolve<IRepositoryContext>())
            {
                //添加新的股票基础数据
                context.UnitOfWork.RegisterNew<StockBase>(stock);
                context.UnitOfWork.Commit();
            }
        }

        public void UpdateStockBase(StockBase stock)
        {
            using (var context = ContainerHelper.Instance.Resolve<IRepositoryContext>())
            {
                //添加新的股票基础数据
                context.UnitOfWork.RegisterModified<StockBase>(stock);
                context.UnitOfWork.Commit();
            }
        }
    }
}
