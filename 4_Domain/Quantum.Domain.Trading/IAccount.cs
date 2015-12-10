
//namespace Quantum.Domain.Trading
//{
//    public interface IAccount
//    {
//        string Id { get; }

//        string Name { get; }

//        /// <summary>
//        /// 总资产
//        /// </summary>
//        decimal TotalAssets { get; }

//        /// <summary>
//        /// 市值
//        /// </summary>
//        decimal MarketValue { get; }

//        /// <summary>
//        /// 冻结资金
//        /// </summary>
//        decimal FrozenFund { get; }

//        /// <summary>
//        /// 余额
//        /// </summary>
//        decimal Balance { get; }

//        /// <summary>
//        /// 可用资金
//        /// </summary>
//        decimal AvailableFund { get; }

//        bool Buy(string code, decimal price, int quantity);

//        int AvailableQuantityToBuy(string code, decimal price);

//        bool Sell(string code, decimal price, int quantity);

//        int AvailableQuantityToSell(string code);

//        bool TransferIn(decimal amount);

//        bool TransferOut(decimal amount);
//    }
//}
