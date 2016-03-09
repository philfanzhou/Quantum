namespace Quantum.Domain.Decision
{
    public interface ITradingAction
    {
        ActionType Type { get; }

        int Quantity { get; }
    }

    /// <summary>
    /// 交易类型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 不进行操作
        /// </summary>
        None,
        /// <summary>
        /// 买入
        /// </summary>
        Buy,
        /// <summary>
        /// 卖出
        /// </summary>
        Sell
    }

    internal class TradingAction : ITradingAction
    {
        public int Quantity { get; set; }

        public ActionType Type { get; set; }
    }
}
