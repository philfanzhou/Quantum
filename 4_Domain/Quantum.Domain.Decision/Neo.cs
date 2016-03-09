using Ore.Infrastructure.MarketData;
using Quantum.Domain.Decision.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.Decision
{
    /// <summary>
    /// Matrix中的救世主，依靠他的能力来做出判断
    /// 组成Neo（尼奥）的这三个字母掉转顺序后就可以组成“one”，表示他就是那个拯救人类的救世主“The One”。
    /// 而“基督”一词在希伯来语中的本意就是“被指定的那个人”——The One
    /// </summary>
    [Serializable]
    public class Neo
    {
        #region Field
        private ISecurity _security;

        private List<IKey> _keys;

        [NonSerialized]
        private bool _logined;
        [NonSerialized]
        private Link _link;
        #endregion

        #region Constructor
        public Neo(ISecurity security, IEnumerable<IKey> keys)
        {
            _security = security;
            _keys = keys.ToList();
        }
        #endregion

        #region Property
        /// <summary>
        /// 获取Neo持有的所有能打开Matrix后门的钥匙
        /// </summary>
        public IEnumerable<IKey> Keys
        {
            get { return _keys; }
        }
        #endregion

        #region Event
        public EventHandler<ITradingAction> TradingRequested;
        #endregion

        #region Public Method
        /// <summary>
        /// 通过接线员登陆到Matrix
        /// </summary>
        /// <param name="link"></param>
        public void Login(Link link)
        {
            _link = link;
            _logined = true;
        }

        public void OnKLineComing(IStockKLine kLine, KLineType type)
        {
            // 未登陆和没有设定接线员，都无法处理新数据
            if (!_logined || _link == null) return;

            // 存储新来的数据
            _link.AddNewData(type, kLine);

            Decide();
        }
        #endregion

        #region Private Method
        private void Decide()
        {
            // 运行决策判断，所有的买Key满足，就买，购买数量另外考虑
            // 所有的卖Key满足，就卖，卖出数量另外考虑
            var result = _keys.Select(p => p.Match(_link)).Distinct().ToList();
            if(result.Count > 1)
            {
                return;
            }

            if(result[0] == ActionType.None)
            {
                return;
            }

            TradingAction action = new TradingAction
            {
                Quantity = GetQuantity(result[0]),
                Type = result[0]
            };
            OnTradingRequested(action);
        }

        private int GetQuantity(ActionType type)
        {
            return 100;
        }

        private void OnTradingRequested(ITradingAction e)
        {
            EventHandler<ITradingAction> handler = TradingRequested;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion
    }
}
