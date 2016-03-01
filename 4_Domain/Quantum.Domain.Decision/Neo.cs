using Ore.Infrastructure.MarketData;
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
    public class Neo
    {
        private Link _link;
        
        /// <summary>
        /// 为Neo分配接线员，并接入Matrix
        /// </summary>
        /// <param name="link"></param>
        public void LoginMatrix(Link link)
        {
            if (null == link)
            {
                throw new ArgumentNullException("link");
            }

            _link = link;
        }

        public void OnRealTime(IStockRealTime realTime)
        {

        }

        public void OnKLine(IStockKLine kLine, KLineType type)
        {

        }

        public void Decide()
        {

        }
    }
}
