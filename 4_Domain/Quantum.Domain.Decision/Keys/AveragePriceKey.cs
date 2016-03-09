using Ore.Infrastructure.MarketData;
using Quantum.Domain.MarketData;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Quantum.Domain.Decision.Keys
{
    [Serializable]
    public class AveragePriceKey : IKey
    {
        private double _buyArgs;
        private double _sellArgs;

        public AveragePriceKey(double buyArgs, double sellArgs)
        {
            _buyArgs = buyArgs;
            _sellArgs = sellArgs;
        }

        public KLineType DataType { get { return KLineType.Min1; } }

        /// <summary>
        /// 均价指标判断，不需要数据具有提前量
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DateTime GetDataStartTime(DateTime time)
        {
            return time;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public ActionType Match(Link link)
        {
            var latestMin1Data = link.GetData(this.DataType).Last().AveragePrice();

            if(latestMin1Data.Eccentricity > _sellArgs)
            {
                return ActionType.Sell;
            }
            else if(latestMin1Data.Eccentricity < _buyArgs)
            {
                return ActionType.Buy;
            }
            else
            {
                return ActionType.None;
            }
        }
    }
}
