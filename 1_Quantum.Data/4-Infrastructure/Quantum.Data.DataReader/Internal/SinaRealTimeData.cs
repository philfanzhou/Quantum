using Quantum.Data.Metadata;
using System;

namespace Quantum.Data.DataReader
{
    internal class SinaRealTimeData : IRealTimeData
    {
        #region Property

        public string Code { get; private set; }

        public string Name { get; private set; }

        public double TodayOpen { get; private set; }

        public double YesterdayClose { get; private set; }

        public double Price { get; private set; }

        public double High { get; private set; }

        public double Low { get; private set; }

        public double Volume { get; private set; }

        public double Amount { get; private set; }

        public double SellFivePrice { get; private set; }

        public double SellFiveVolume { get; private set; }

        public double SellFourPrice { get; private set; }

        public double SellFourVolume { get; private set; }

        public double SellThreePrice { get; private set; }

        public double SellThreeVolume { get; private set; }

        public double SellTwoPrice { get; private set; }

        public double SellTwoVolume { get; private set; }

        public double SellOnePrice { get; private set; }

        public double SellOneVolume { get; private set; }

        public double BuyOnePrice { get; private set; }

        public double BuyOneVolume { get; private set; }

        public double BuyTwoPrice { get; private set; }

        public double BuyTwoVolume { get; private set; }

        public double BuyThreePrice { get; private set; }

        public double BuyThreeVolume { get; private set; }

        public double BuyFourPrice { get; private set; }

        public double BuyFourVolume { get; private set; }

        public double BuyFivePrice { get; private set; }

        public double BuyFiveVolume { get; private set; }

        public DateTime Time { get; private set; }

        #endregion Porperty

        public SinaRealTimeData(string strData)
        {
            strData = strData.Remove(0, 11);
            this.Code = strData.Substring(0, 8);

            int startIndex = strData.IndexOf("\"") + 1;
            int length = strData.LastIndexOf("\"") - startIndex;
            strData = strData.Substring(startIndex, length);

            string[] fields = strData.Split(',');

            this.Name = fields[0];
            this.TodayOpen = Convert.ToDouble(fields[1]);
            this.YesterdayClose = Convert.ToDouble(fields[2]);
            this.Price = Convert.ToDouble(fields[3]);
            this.High = Convert.ToDouble(fields[4]);
            this.Low = Convert.ToDouble(fields[5]);
            this.Volume = Convert.ToDouble(fields[8]);
            this.Amount = Convert.ToDouble(fields[9]);

            this.BuyOneVolume = Convert.ToDouble(fields[10]);
            this.BuyOnePrice = Convert.ToDouble(fields[11]);

            this.BuyTwoVolume = Convert.ToDouble(fields[12]);
            this.BuyTwoPrice = Convert.ToDouble(fields[13]);

            this.BuyThreeVolume = Convert.ToDouble(fields[14]);
            this.BuyThreePrice = Convert.ToDouble(fields[15]);

            this.BuyFourVolume = Convert.ToDouble(fields[16]);
            this.BuyFourPrice = Convert.ToDouble(fields[17]);

            this.BuyFiveVolume = Convert.ToDouble(fields[18]);
            this.BuyFivePrice = Convert.ToDouble(fields[19]);

            this.SellOneVolume = Convert.ToDouble(fields[20]);
            this.SellOnePrice = Convert.ToDouble(fields[21]);

            this.SellTwoVolume = Convert.ToDouble(fields[22]);
            this.SellTwoPrice = Convert.ToDouble(fields[23]);

            this.SellThreeVolume = Convert.ToDouble(fields[24]);
            this.SellThreePrice = Convert.ToDouble(fields[25]);

            this.SellFourVolume = Convert.ToDouble(fields[26]);
            this.SellFourPrice = Convert.ToDouble(fields[27]);

            this.SellFiveVolume = Convert.ToDouble(fields[28]);
            this.SellFivePrice = Convert.ToDouble(fields[29]);

            this.Time = Convert.ToDateTime(fields[30] + " " + fields[31]);
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(this.Code) &&
                !string.IsNullOrWhiteSpace(this.Name))
            {
                return this.Code + " " + this.Name + " " + this.Price;
            }
            else
            {
                return base.ToString();
            }
        }
    }
}
