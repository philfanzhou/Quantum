using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class HoldingsRecordData
    {
        public string AccountId { get; set; }

        public string StockCode { get; set; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
