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

    public class ImportDividendDataService
    {
        public DateTime GetLatestDividendDataImportDateTime(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException("smybol");
            }

            using (var context = ContainerHelper.Instance.Resolve<IRepositoryContext>())
            {
                var repository = new DividendDataItemRepository(context);
                return repository.GetLatestDividendDataImportDateTime(symbol);
            }
        }

        public void ImportDividendData(IEnumerable<DividendDataItem> dividendItems)
        {
            if (dividendItems == null || dividendItems.Count() == 0)
            {
                return;
            }

            using (var context = ContainerHelper.Instance.Resolve<IRepositoryContext>())
            {
                var repository = new DividendDataItemRepository(context);
                foreach (var dividendDataItem in dividendItems)
                {
                    //复合类在Commit时会抛主键重复异常，因此不能对Stock赋值
                    context.UnitOfWork.RegisterNew<DividendDataItem>(dividendDataItem);
                }
                context.UnitOfWork.Commit();
            }
        }
    }
}
