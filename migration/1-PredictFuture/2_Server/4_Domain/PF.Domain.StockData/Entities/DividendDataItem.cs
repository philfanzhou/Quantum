using System;

namespace PF.Domain.StockData.Entities
{
    /// <summary>
    /// 除权数据
    /// </summary>
    public class DividendDataItem : StockDataItemBase
    {
        /// <summary>
        /// 除权日
        /// </summary>
        public DateTime ExdividendDate { get; set; }

        /// <summary>
        /// 分红
        /// </summary>
        public double Cash { get; set; }

        /// <summary>
        /// 总拆股
        /// </summary>
        public double Split { get; set; }

        /// <summary>
        /// 转增股
        /// </summary>
        public double Bonus { get; set; }

        /// <summary>
        /// 配股
        /// </summary>
        public double Dispatch { get; set; }

        /// <summary>
        /// 配股价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 登记日
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 上市日
        /// </summary>
        public DateTime ListingDate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
