using PF.DataImport.AppService;
using PF.DataImport.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DataImport.Console
{
    public abstract class StockBusiness
    {
        public abstract string Suffix { get; }

        private string m_StockCode;

        public StockBusiness()
        { 
        }

        public StockBusiness(string stockCode)
        {
            m_StockCode = stockCode;
        }

        public string StockSync()
        {
            StringBuilder returnStr = new StringBuilder();

            //获取股票Domain服务
            var stockservice = new ImportStockService();
            var dailypriceservice = new ImportDailyPriceDataService();

            //获取股票对象
            var stock = stockservice.GetStockBySymbol(m_StockCode);
            if (stock == null)
            {
                //创建股票对象
                stock = new Stock { Symbol = m_StockCode, Name = string.Empty };
                stockservice.ImportStock(stock);
            }

            //获取股票日线最新记录时间
            var starttime = dailypriceservice.GetLatestDailyPriceDataImportDateTime(m_StockCode);

            //创建雅虎财经实例，并获取数据库未记录的历史数据
            IEnumerable<DailyPriceDataItem> dayItemList;
            YahooFinance yahoo = new YahooFinance(stock);
            yahoo.Suffix = Suffix;
            if (starttime < new DateTime(1990, 12, 1))
            {
                dayItemList = yahoo.GetDayLineHistory();
            }
            else
            {
                dayItemList = yahoo.GetDayLineHistory(starttime.AddDays(1), DateTime.Now);
            }

            if (dayItemList == null)
            {
                returnStr.Append(string.Format("获取雅虎财经：{0}{1}失败，没有新数据或网络异常！\n", m_StockCode, Suffix));
                return returnStr.ToString();
            }

            //插入最新的日线数据
            if (dayItemList.Count() > 0)
            {
                dailypriceservice.ImportPriceData(dayItemList);
                returnStr.Append(string.Format("股票：{0}{1}插入{2}条日线数据。\n", m_StockCode, Suffix, dayItemList.Count()));
            }

            //如果今天是星期六，则获取股票基础面信息
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                Eastmoney eastMoney = new Eastmoney(Suffix, m_StockCode);
                eastMoney.RefreshStockBase();
            }

            return returnStr.ToString();
        }
    }

    public class SzStockBusiness : StockBusiness
    {
        public override string Suffix { get { return ".sz"; } }

        public SzStockBusiness(string stockCode)
            : base(stockCode)
        {

        }
    }

    public class ShStockBusiness : StockBusiness
    {
        public override string Suffix { get { return ".ss"; } }

        public ShStockBusiness(string stockCode)
            : base(stockCode)
        {

        }
    }
}
