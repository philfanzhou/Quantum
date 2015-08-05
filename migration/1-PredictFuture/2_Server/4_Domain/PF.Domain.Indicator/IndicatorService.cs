//namespace PF.Domain.Indicator
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.Composition;
//    using System.ComponentModel.Composition.Hosting;
//    using System.Linq;
//    using System.Reflection;

//    public class IndicatorService
//    {
//        [ImportMany(typeof(Indicator))]
//        private List<Indicator> indicators;

//        //PriceDataService priceService;
//        public IndicatorService(/*PriceDataService priceService*/)
//        {
//            //this.priceService = priceService;

//            indicators = new List<Indicator>();
//            var ac = new AssemblyCatalog(Assembly.GetAssembly(GetType()));
//            var container = new CompositionContainer(ac);
//            container.ComposeParts(this);
//            ac.Dispose();
//        }

//        /// <summary>
//        /// 获取系统支持的所有指标
//        /// </summary>
//        /// <returns></returns>
//        public List<Indicator> GetSupportedIndicators()
//        {
//            return indicators;
//        }

//        public Indicator QueryIndicator(string name, IndicatorCycle cycle, DateTime? endTime = null, TimeSpan? Offset = null)
//        {
//            var item = indicators.SingleOrDefault(p => p.Name == name);
//            if (item == null)
//                throw new NotSupportedException("不支持的指标");

//            var newItem = (Indicator)item.Clone();
//            newItem.Cycle = cycle;
//            //newItem.Offset
//            //newItem.FillValue(priceService);
//            return item;

//        }
//    }
//}
