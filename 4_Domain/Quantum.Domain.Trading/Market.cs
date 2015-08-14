using System;

namespace Quantum.Domain.Trading
{
    internal class Market
    {
        private static bool isVirtual = false;
        private static DateTime virtualTime = DateTime.MinValue;

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
