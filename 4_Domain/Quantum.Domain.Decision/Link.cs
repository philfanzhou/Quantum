using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Decision
{
    /// <summary>
    /// 继坦克（Tank）之后担任墨菲斯的Nebuchadnezzar号飞船的新接线员。
    /// 在Quantum中，Tank的作为是为Neo提供数据支持
    /// </summary>
    public class Link
    {
        /// <summary>
        /// 不同类型的历史数据
        /// </summary>
        private readonly Dictionary<KLineType, List<IStockKLine>> _history
            = new Dictionary<KLineType, List<IStockKLine>>();

        public Link(IDictionary<KLineType, IEnumerable<IStockKLine>> history)
        {
            foreach(var item in history)
            {
                _history.Add(item.Key, item.Value.ToList());
            }
        }

        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="kLine"></param>
        internal void AddNewData(KLineType type, IStockKLine kLine)
        {
            if(!_history.ContainsKey(type))
            {
                _history.Add(type, new List<IStockKLine>());
            }

            _history[type].Add(kLine);
        }

        /// <summary>
        /// 获取指定类型的数据，如果无法获取到数据，返回null
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal IEnumerable<IStockKLine> GetData(KLineType type)
        {
            if(_history.ContainsKey(type))
            {
                return _history[type];
            }
            else
            {
                return null;
            }
        }
    }
}
