using Ore.Infrastructure.MarketData;
using Quantum.Domain.MarketData;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Quantum.Domain.Decision.Keys
{
    /// <summary>
    /// 根据1分钟K线上的平均价格线来进行交易决策
    /// </summary>
    [Serializable]
    public class AveragePriceKey : IKey, IKeyDescription
    {
        private double _flag;
        private readonly KeyType _keyType;

        public AveragePriceKey(double flag, KeyType keyType)
        {
            _flag = flag;
            _keyType = keyType;
        }

        public KLineType DataType { get { return KLineType.Min1; } }

        public KeyType KeyType { get { return _keyType; } }

        public string Name { get { return "AveragePrice"; } }

        internal double Step { get { return 1; } }

        /// <summary>
        /// 均价指标判断，不需要数据具有提前量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DateTime GetDataStartTime(DateTime time)
        {
            return time;
        }

        public bool Match(Link link)
        {
            var latestAverageData = link.GetData(this.DataType).Last().AveragePrice();

            if(_keyType == KeyType.Buy &&
                latestAverageData.Eccentricity < _flag)
            {
                return true;
            }

            if (_keyType == KeyType.Sell &&
                latestAverageData.Eccentricity > _flag)
            {
                return true;
            }

            return false;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
