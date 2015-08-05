using PF.DataImport.Domain;
using PF.IFS.TongHua.DataReader;

namespace PF.IFS.TongHuaDataReader.Test
{
    public static class DataUtil
    {
        public static DividendDataItem CopyFrom(IDividendItem dividendItem)
        {
            return new DividendDataItem
            {
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
        
        public static DailyPriceDataItem CopyFrom(IKlineItem klineItem)
        {
            return new DailyPriceDataItem
            {
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
