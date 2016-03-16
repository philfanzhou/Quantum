using Ore.Infrastructure.MarketData;
using Quantum.Domain.Decision;
using System;
using System.Collections.Generic;

namespace Quantum.Application.Simulation
{
    public class SimulationBattleship : Battleship
    {
        public SimulationBattleship(ISecurity security, DateTime tradingStartTime) : base(security, tradingStartTime) { }

        public override IEnumerable<IStockKLine> GetKLines(ISecurity security, KLineType type, DateTime startTime, DateTime endTime)
        {
            return Domain.MarketData.Simulation.CreateRandomKLines(type, startTime, endTime);
        }
    }
}
