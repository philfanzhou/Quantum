using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Domain.Decision
{
    public abstract class Battleship : IBattleship
    {
        private ISecurity _security;
        private DateTime _tradingStartTime;
        private Link _link = new Link();

        public Link Link { get { return _link; } }

        public Battleship(ISecurity security, DateTime tradingStartTime)
        {
            _security = security;
            _tradingStartTime = tradingStartTime;
        }

        public void UpgradeOperator(Neo neo)
        {
            if (neo.Security.Code != _security.Code)
            {
                throw new ArgumentOutOfRangeException("neo");
            }

            var keys = neo.Keys.ToList();
            foreach (var key in keys)
            {
                // 获取Key对数据的要求
                var kLineType = key.DataType;
                var dataStartTime = key.GetDataStartTime(_tradingStartTime);
                // 获取数据
                var datas = GetKLines(_security, kLineType, dataStartTime, _tradingStartTime).ToList();
                if (datas == null)
                {
                    datas = new List<IStockKLine>();
                }

                _link.AddDatas(kLineType, datas);
            }
        }

        protected abstract IEnumerable<IStockKLine> GetKLines(ISecurity security, KLineType type, DateTime startTime, DateTime endTime);
    }
}
