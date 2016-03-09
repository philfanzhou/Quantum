using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.Decision
{
    public abstract class Battleship : IBattleship
    {
        private DateTime _dataStartTime;

        public Battleship(DateTime dataStartTime)
        {
            _dataStartTime = dataStartTime;
        }

        public Link GetLink(Neo neo)
        {
            var security = neo.Security;
            var keys = neo.Keys.ToList();

            var historyData = new Dictionary<KLineType, IEnumerable<IStockKLine>>();

            foreach(var key in keys)
            {
                // 获取Key对数据的要求
                var kLineType = key.DataType;
                var dataStartTime = key.GetDataStartTime(_dataStartTime);
                // 获取数据
                var data = GetData(kLineType, dataStartTime).ToList();
                if(data == null)
                {
                    data = new List<IStockKLine>();
                }


                if(historyData.ContainsKey(kLineType))
                {
                    // 将数据补充进去
                    var existData = historyData[kLineType].ToList();
                    var needAddData = data.Except(existData);
                    var newData = existData;
                    newData.AddRange(needAddData);
                    historyData[kLineType] = newData;
                }
                else
                {
                    historyData.Add(kLineType, data);
                }
            }

            var link = new Link(historyData);
            return link;
        }

        protected abstract IEnumerable<IStockKLine> GetData(KLineType type, DateTime startTime);
    }
}
