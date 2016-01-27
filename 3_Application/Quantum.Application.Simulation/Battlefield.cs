﻿using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Application.Simulation
{
    internal class Battlefield
    {
        private readonly List<IStockRealTime> _realTimeItems;

        public Battlefield(IEnumerable<IStockRealTime> realTimeItems)
        {
            _realTimeItems = realTimeItems.ToList();
        }

        public void Practice()
        {
            /*
            在这里遍历每一条数据，每条数据的时间作为当前时间
            */
        }
    }
}
