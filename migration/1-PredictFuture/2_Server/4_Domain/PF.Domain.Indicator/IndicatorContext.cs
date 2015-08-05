using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Domain.Indicator
{
    public class IndicatorContext : IIndicatorContext
    {
        public string StockId { get; private set; }

        public DateTime EndDate { get; private set; }

        public IndicatorContext(string stockId, DateTime endDate)
        {
            StockId = stockId;
            EndDate = endDate;
        }
    }
}
