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

        public IEnumerable<KLineType> DataTypes
        {
            get { return _history.Keys; }
        }
        
        /// <summary>
        /// 判断数据是否已经存在
        /// </summary>
        /// <param name="type"></param>
        /// <param name="kLine"></param>
        /// <returns></returns>
        public bool ExistData(KLineType type, IStockKLine kLine)
        {
            if (_history.ContainsKey(type) 
                && _history[type].Contains(kLine))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="kLine"></param>
        public void AddNewData(KLineType type, IStockKLine kLine)
        {
            if(!_history.ContainsKey(type))
            {
                _history.Add(type, new List<IStockKLine>());
            }

            _history[type].Add(kLine);
        }

        internal void AddDatas(KLineType type, IEnumerable<IStockKLine> kLines)
        {
            var datas = kLines.ToList();
            if (_history.ContainsKey(type))
            {
                // 将数据补充进去
                var existData = _history[type];
                var needAddData = datas.Except(existData);
                _history[type].AddRange(needAddData);
            }
            else
            {
                _history.Add(type, datas);
            }
        }

        internal bool ContainsType(KLineType type)
        {
            return _history.ContainsKey(type);
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
