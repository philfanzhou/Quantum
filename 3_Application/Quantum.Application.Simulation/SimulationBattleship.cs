using Ore.Infrastructure.MarketData;
using Quantum.Domain.Decision;
using System;
using System.Collections.Generic;

namespace Quantum.Application.Simulation
{
    public class SimulationBattleship : Battleship
    {
        public SimulationBattleship(DateTime tradingStartTime) : base(tradingStartTime) { }

        protected override IEnumerable<IStockKLine> GetData(KLineType type, DateTime startTime, DateTime endTime)
        {
            return Domain.MarketData.Simulation.CreateRandomKLines(type, startTime, endTime);
        }
    }
}
