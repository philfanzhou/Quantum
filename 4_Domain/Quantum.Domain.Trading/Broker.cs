namespace Quantum.Domain.Trading
{
    /// <summary>
    /// 券商
    /// </summary>
    public static class Broker
    {
        #region Field
        private static decimal _commissionRate = 0.0003m;
        private static decimal _stampDutyRate = 0.001m;
        private static decimal _transferFeesRate = 0.00002m;
        #endregion

        #region Property
        /// <summary>
        /// 获取或设置佣金费率
        /// </summary>
        public static decimal CommissionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }

        /// <summary>
        /// 获取或设置印花税率
        /// </summary>
        public static decimal StampDutyRate
        {
            get { return _stampDutyRate; }
            set { _stampDutyRate = value; }
        }

        /// <summary>
        /// 获取或设置过户费率
        /// </summary>
        public static decimal TransferFeesRate
        {
            get { return _transferFeesRate; }
            set { _transferFeesRate = value; }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 创建交易账户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IAccount CreateAccount(string name)
        {
            return new Account(name);
        }
        #endregion
    }
}
