using Ore.Infrastructure.MarketData;
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

        private Dictionary<KLineType, IEnumerable<IStockKLine>> _datas = new Dictionary<KLineType, IEnumerable<IStockKLine>>();

        public Battlefield(ISecurity security)
        {
            _security = security;
        }

        public void Practice(DateTime beginTime, DateTime endTime)
        {
            var battleship = new SimulationBattleship(beginTime);
            var morpheus = new Morpheus();
            var candidate = morpheus.FindCandidate(_security).ToList();

            candidate.ForEach(p => p.Login(battleship));

            /*
            在这里遍历每一条数据，每条数据的时间作为当前时间
            */
            var datas = Domain.MarketData.Simulation.CreateRandomKLines(KLineType.Min1, beginTime, endTime);
            foreach(var dataItem in datas)
            {
                // todo 进度修改
                candidate.ForEach(p => p.OnKLineComing(dataItem, KLineType.Min1));
            }
        }

        // 写一个时间机器，查询出所有数据的最小单位，以最小单位的数据，进行数据推送，当前时间以推送数据时间为准。
    }
}
