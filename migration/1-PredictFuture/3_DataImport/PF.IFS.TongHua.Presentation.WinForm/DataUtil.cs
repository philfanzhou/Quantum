using PF.DataImport.Domain;
using PF.IFS.TongHua.DataReader;

namespace PF.IFS.TongHua.Presentation.WinForm
{
    public static class DataUtil
    {
        public static DividendDataItem CopyFrom(IDividendItem dividendItem, Stock stock)
        {
            return new DividendDataItem
            {
                Stock = stock,
                Date = dividendItem.Date,
                ExdividendDate = dividendItem.ExdividendDate,
                Cash = dividendItem.Cash,
                Split = dividendItem.Split,
                Bonus = dividendItem.Bonus,
                Dispatch = dividendItem.Dispatch,
                Price = dividendItem.Price,
                RegisterDate = dividendItem.RegisterDate,
                ListingDate = dividendItem.ListingDate,
                Description = dividendItem.Description
            };
        }

        public static DailyPriceDataItem CopyFrom(IKlineItem klineItem, Stock stock)
        {
            return new DailyPriceDataItem
            {
                Stock = stock,
                Date = klineItem.Date,
                Open = klineItem.Open,
                High = klineItem.High,
                Low = klineItem.Low,
                Close = klineItem.Close,
                Amount = klineItem.Amount,
                Volume = klineItem.Volume,
            };
        }
    }
}
