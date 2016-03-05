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
        /// 获取决策所需数据的所有类型
        /// </summary>
        public IEnumerable<IKey> Keys
        {
            get { return _keys; }
        }
        #endregion

        #region Event
        // 下单事件
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

            // 运行决策判断

            // 触发下单事件
        }
        #endregion

        private void Decide()
        {

        }
    }
}
