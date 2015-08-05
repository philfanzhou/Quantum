namespace PF.Domain.Indicator
{
    using PF.Domain.StockData;
//using PF.Domain.StockData.Service;
    using System.ComponentModel.Composition;

    //[Export(typeof(Indicator))]
    [IndicatorCategory(Name = "技术指标")]
    [IndicatorCategory(Name = "行情指标")]
    public class OpenPriceIndicator : ITechnicalIndicator
    {
      
        private KLineCycle _cycle = KLineCycle.Daily;
        public OpenPriceIndicator()
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
            get { return "元"; }
        }

        public double GetValue()
        {
            var dataProvider = new IndicatorDataService();
            var priceData = dataProvider.GetPriceDate(Context.StockId, Cycle, ReferOffset, Context.EndDate);
            return priceData.Open;
        }

        public string Name
        {
            get { return "开盘价"; }
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
