using PF.DataImport.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PF.DataImport.Console
{
    public class YahooFinance
    {
        private Stock m_Stock;
        private string m_Suffix = ".ss";

        public YahooFinance(Stock stock)
        {
            m_Stock = stock;
        }

        public string Suffix
        {
            get { return m_Suffix; }
            set { m_Suffix = value; }
        }

        public IEnumerable<DailyPriceDataItem> GetDayLineHistory(DateTime dtStart, DateTime dtEnd)
        {            
            string webSerUri = string.Format("http://ichart.yahoo.com/table.csv?s={0}{1}&a={2}&b={3}&c={4}&d={5}&e={6}&f={7}&g=d&ignore=.csv",
                m_Stock.Symbol, m_Suffix, dtStart.Month-1, dtStart.Day, dtStart.Year, dtEnd.Month-1, dtEnd.Day, dtEnd.Year);
            return GetDayLineHistory(webSerUri);
        }

        public IEnumerable<DailyPriceDataItem> GetDayLineHistory()
        {
            string webSerUri = string.Format("http://ichart.yahoo.com/table.csv?s={0}{1}", 
                m_Stock.Symbol, m_Suffix);
            return GetDayLineHistory(webSerUri);
        }

        public IEnumerable<DailyPriceDataItem> GetDayLineHistory(string webSerUri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webSerUri);
                request.Method = "GET";
                Stream stream = request.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string[] stockHistory = reader.ReadToEnd().Split('\n');

                var dayItemList = new List<DailyPriceDataItem>();
                for (int i = 1; i < stockHistory.Length - 1; i++)
                {
                    DateTime date = Convert.ToDateTime(stockHistory[i].Split(',')[0]);
                    double open = Double.Parse(stockHistory[i].Split(',')[1]);
                    double high = Double.Parse(stockHistory[i].Split(',')[2]);
                    double low = Double.Parse(stockHistory[i].Split(',')[3]);
                    double close = Double.Parse(stockHistory[i].Split(',')[4]);
                    double volume = Double.Parse(stockHistory[i].Split(',')[5]);

                    dayItemList.Add(new DailyPriceDataItem
                    {
                        StockId = m_Stock.Id,
                        Date = date,
                        Open = open,
                        High = high,
                        Low = low,
                        Close = close,
                        Volume = volume,
                        Amount = (high + low) * volume / 2,
                    });
                }
                return dayItemList;
            }
            catch
            {
                return null;
            }
        }
    }
}
