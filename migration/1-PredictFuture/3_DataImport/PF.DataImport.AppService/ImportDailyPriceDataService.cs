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

    public class ImportDailyPriceDataService
    {
        public DateTime GetLatestDailyPriceDataImportDateTime(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException("smybol");
            }

            using (var context = ContainerHelper.Instance.Resolve<IRepositoryContext>())
            {
                var repository = new DailyPriceDataItemRepository(context);
                return repository.GetLatestDailyPriceDataImportDateTime(symbol);
            }
        }

        public void ImportPriceData(IEnumerable<DailyPriceDataItem> dailyPriceDataItems)
        {
            if (dailyPriceDataItems == null || dailyPriceDataItems.Count() == 0)
            {
                return;
            }

            using (var context = ContainerHelper.Instance.Resolve<IRepositoryContext>())
            {
                var repository = new DailyPriceDataItemRepository(context);
                foreach (var dayLineItem in dailyPriceDataItems)
                {
                    //复合类在Commit时会抛主键重复异常，因此不能对Stock赋值
                    context.UnitOfWork.RegisterNew<DailyPriceDataItem>(dayLineItem);
                }
                context.UnitOfWork.Commit();
            }
        }
    }
}
