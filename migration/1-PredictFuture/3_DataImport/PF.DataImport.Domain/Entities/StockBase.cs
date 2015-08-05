using System;
using Core.Domain;

namespace PF.DataImport.Domain
{
    public class StockBase : Entity, IAggregateRoot
    {
        #region Constractor

        public StockBase()
        {
            Id = Guid.NewGuid().ToString();
        }

        #endregion

        /// <summary>
        /// 股票代码
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 上市时间
        /// </summary>
        public DateTime ListingDate { get; set; }

        /// <summary>
        /// 总股本
        /// </summary>
        public double TotalCapital { get; set; } 

        /// <summary>
        /// 流通股本
        /// </summary>
        public double FloatingCapital { get; set; }

        /// <summary>
        /// 每股收益1
        /// </summary>
        public double EPSOne { get; set; }

        /// <summary>
        /// 每股净资产
        /// </summary>
        public double NAPS { get; set; }

        /// <summary>
        /// 净资产收益率
        /// </summary>
        public double ROE { get; set; }

        /// <summary>
        /// 营业收入
        /// </summary>
        public double BR { get; set; }

        /// <summary>
        /// 营业收入增长率
        /// </summary>
        public double IRBR { get; set; } 

        /// <summary>
        /// 销售毛利率
        /// </summary>
        public double Profitmargi { get; set; }

        /// <summary>
        /// 净利润
        /// </summary>
        public double NP { get; set; }

        /// <summary>
        /// 净利润增长率
        /// </summary>
        public double IRNP { get; set; }

        /// <summary>
        /// 每股未分配利润
        /// </summary>
        public double UPPS { get; set; } 
    }
}
