using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PF.Application.Dto.StockData
{
    [DataContract]
    public class DividendDataItemDto : StockDataItemDtoBase
    {
        /// <summary>
        /// 日期
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// 除权日
        /// </summary>
        [DataMember]
        public DateTime ExdividendDate { get; set; }

        /// <summary>
        /// 分红
        /// </summary>
        [DataMember]
        public double Cash { get; set; }

        /// <summary>
        /// 总拆股
        /// </summary>
        [DataMember]
        public double Split { get; set; }

        /// <summary>
        /// 转增股
        /// </summary>
        [DataMember]
        public double Bonus { get; set; }

        /// <summary>
        /// 配股
        /// </summary>
        [DataMember]
        public double Dispatch { get; set; }

        /// <summary>
        /// 配股价
        /// </summary>
        [DataMember]
        public double Price { get; set; }

        /// <summary>
        /// 登记日
        /// </summary>
        [DataMember]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 上市日
        /// </summary>
        [DataMember]
        public DateTime ListingDate { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
