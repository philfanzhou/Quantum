namespace PF.Domain.Indicator
{
    using PF.Domain.StockData;
    using System;
    //using PF.Domain.StockData.Service;
    //using System.ComponentModel.Composition;

    //[Export(typeof(Indicator))]
    [IndicatorCategory(Name = "技术指标")]
    [IndicatorCategory(Name = "行情指标")]
    public class AmountIndicator : ITechnicalIndicator
    {
        private KLineCycle _cycle = KLineCycle.Daily;

        public AmountIndicator()
        {
            ReferOffset = 0;
        }

        public KLineCycle Cycle
        {
            get { return this._cycle; }
            set { this._cycle = value; }
        }

        public int ReferOffset { get; set; }

        public string Unit
        {
            get { return "股"; }
        }

        public double GetValue()
        {
            var dataProvider = new IndicatorDataService();
            var priceData = dataProvider.GetPriceDate(Context.StockId, Cycle, ReferOffset, Context.EndDate);

            // todo:这里要确认一下，数据那边成交量的单位是不是手，1手=100股
            return Convert.ToInt32(priceData.Amount*100);
        }

        public string Name
        {
            get { return "成交量"; }
        }

        public string Description
        {
            get { return this.Unit; }
        }

        public string ShownText
        {
            get { return string.Format("{0}({1})", this.Name, this.Description); }
        }

        public IIndicatorContext Context { get; set; }
    }
}
