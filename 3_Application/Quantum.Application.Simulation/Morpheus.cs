using Ore.Infrastructure.MarketData;
using Quantum.Domain.Decision;
using Quantum.Domain.Decision.Keys;
using System.Collections.Generic;

namespace Quantum.Application.Simulation
{
    /// <summary>
    /// 在希腊神话中，墨菲斯是梦神，拥有改变梦境的能力。在电影中，墨菲斯是把人们从梦境般的虚幻世界中唤醒的指路人
    /// 墨菲斯是从众多普通人中，寻找出那个唯一的救世主Neo的人
    /// </summary>
    public class Morpheus
    {
        /// <summary>
        /// 寻找出候选人
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Neo> FindCandidate()
        {
            var keys = new KeyMaker().CreateKeys();
            ISecurity security = new Security
            {
                Code = "600036",
                Market = Market.XSHG,
                ShortName = "招商银行",
                Type = SecurityType.Sotck
            };

            Neo neo = new Neo(security, keys);

            return new List<Neo> { neo };
        }
    }
}
