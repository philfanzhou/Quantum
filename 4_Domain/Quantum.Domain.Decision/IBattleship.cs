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
    /// Nebuchadnezzar号飞船
    /// Neo可以通过飞船联系到接线员
    /// </summary>
    public interface IBattleship
    {
        Link GetLink(ISecurity security, IEnumerable<IKey> keys);
    }
}
