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

            /*
            在这里遍历每一条数据，每条数据的时间作为当前时间
            */
            KLineType type = KLineType.Min1;
            var datas = Domain.MarketData.Simulation.CreateRandomKLines(type, beginTime, endTime);
            foreach(var dataItem in datas)
            {
                if (!battleship.Link.ExistData(type, dataItem))
                {
                    // 存储新来的数据
                    battleship.Link.AddNewData(type, dataItem);
                }

                // todo 进度修改
                candidate.ForEach(p => p.OnKLineComing());
            }
        }

        // 写一个时间机器，查询出所有数据的最小单位，以最小单位的数据，进行数据推送，当前时间以推送数据时间为准。
    }
}
