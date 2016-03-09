using Quantum.Domain.Decision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ore.Infrastructure.MarketData;
using Quantum.Domain.Decision.Keys;

namespace Quantum.Application.Simulation
{
    public class SimulationBattleship : IBattleship
    {
        public Link GetLink(Neo neo)
        {
            var security = neo.Security;
            var keys = neo.Keys.ToList();

            throw new NotImplementedException();
        }
    }
}
