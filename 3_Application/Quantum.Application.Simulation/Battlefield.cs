using Ore.Infrastructure.MarketData;
using Quantum.Domain.Decision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Application.Simulation
{
    public class Battlefield
    {
        private readonly ISecurity _security;

        public Battlefield(ISecurity security)
        {
            _security = security;
        }

        public void Practice(IBattleship battleship, DateTime beginTime, DateTime endTime)
        {
            var morpheus = new Morpheus();

            // 创造出所有的候选人，并登录Matrix
            var candidate = morpheus.FindCandidate(_security).ToList();
            candidate.ForEach(p => p.Login(battleship));

            // 构造出未来的数据
            var futureDatas = GetFutureDatas(battleship, beginTime, endTime);
            
            //foreach(var dataItem in datas)
            //{
            //    if (!battleship.Link.ExistData(type, dataItem))
            //    {
            //        // 存储新来的数据
            //        battleship.Link.AddNewData(type, dataItem);
            //    }

            //    // todo 进度修改
            //    candidate.ForEach(p => p.OnKLineComing());
            //}
        }

        private Dictionary<KLineType, List<IStockKLine>> GetFutureDatas(IBattleship battleship, DateTime beginTime, DateTime endTime)
        {
            var result = new Dictionary<KLineType, List<IStockKLine>>();

            foreach(var kLineType in battleship.Link.DataTypes)
            {
                var datas = battleship.GetKLines(_security, kLineType, beginTime, endTime).ToList();
                result.Add(kLineType, datas);
            }

            return result;
        }

        // 写一个时间机器，查询出所有数据的最小单位，以最小单位的数据，进行数据推送，当前时间以推送数据时间为准。
    }
}
