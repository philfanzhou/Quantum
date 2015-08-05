using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PF.Application.Dto.StockData
{
    [DataContract]
    public class PriceDataItemDto : StockDataItemDtoBase
    {
        /// <summary>
        /// 日期
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        [DataMember]
        public double Open { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        [DataMember]
        public double High { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        [DataMember]
        public double Low { get; set; }

        /// <summary>
        /// 收盘价
        /// </summary>
        [DataMember]
        public double Close { get; set; }

        /// <summary>
        /// 成交金额
        /// </summary>
        [DataMember]
        public double Amount { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        [DataMember]
        public double Volume { get; set; }

    }
}
