﻿using Framework.Infrastructure.Repository;

namespace Quantum.Infrastructure.Trading.Repository
{
    public class HoldingsRecordData : Entity
    {
        public string AccountId { get; set; }

        public string StockCode { get; set; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
