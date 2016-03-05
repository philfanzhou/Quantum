using Ore.Infrastructure.MarketData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Domain.MarketData
{
    public static class KLineRestoration
    {
        public static IEnumerable<IStockKLine> KLineListPreRestoration(this IEnumerable<IStockKLine> selfKLines,
            IEnumerable<IStockBonus> selfBonus)
        {
            return null;
        }

        public static double KLinePreRestoration(this IStockKLine selfKLine,
            IEnumerable<IStockBonus> selfBonus)
        {
            DateTime mytime = selfKLine.Time;
            List<IStockBonus> selfList = selfBonus.ToList().OrderBy(p => p.ExdividendDate).ToList();
            double tvalue = selfKLine.Close;

            for (int i = 0; i < selfBonus.Count(); i++)
            {
                // 前复权：复权后价格＝[(复权前价格-现金红利)＋配(新)股价格×流通股份变动比例]÷(1＋流通股份变动比例) 
                // 权后价格=(价格-红利/10)/(1+送股数/10)
                // 除权除息日当天就需要开始计算，因此在前复权情况下判断条件是mytime < ctime
                DateTime ctime = selfList[i].ExdividendDate;
                if (mytime < ctime)
                {
                    double hongli = selfList[i].PreTaxDividend / 10;
                    double songgu = selfList[i].BonusRate / 10;
                    double liutongbdbl = selfList[i].IncreaseRate / 10 + songgu;
                    double peigujia = selfList[i].DispatchPrice;
                    double peigulv = selfList[i].DispatchRate / 10;
                    tvalue = (tvalue - hongli + peigujia * peigulv) / (1 + liutongbdbl + peigulv);
                }
            }

            return Math.Round(tvalue, 2);
        }
    }
}
