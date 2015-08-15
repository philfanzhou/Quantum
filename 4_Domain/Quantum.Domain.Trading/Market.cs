using System;

namespace Quantum.Domain.Trading
{
    internal class Market
    {
        private static bool isVirtual = false;
        private static DateTime virtualTime = DateTime.MinValue;

        /// <summary>
        /// 一手股票包含100股
        /// </summary>
        public const int OneHandStock = 100;

        /// <summary>
        /// 佣金费率
        /// </summary>
        public const decimal CommissionRate = 0.0003m;

        /// <summary>
        /// 印花税率
        /// </summary>
        public const decimal StampDutyRate = 0.001m;

        /// <summary>
        /// 交易费率
        /// </summary>
        public const decimal TransferFeesRate = 0.00002m;

        public static bool IsVirtual
        {
            get { return isVirtual; }
            set { isVirtual = value; }
        }

        public static DateTime Time
        {
            get
            {
                if (isVirtual)
                {
                    return virtualTime;
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        public static void SetVirtualMarketTime(DateTime time)
        {
            virtualTime = time;
        }
    }
}
