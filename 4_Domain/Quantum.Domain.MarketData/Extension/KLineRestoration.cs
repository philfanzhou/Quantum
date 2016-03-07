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
            List<IStockKLine> arrStockKLines = new List<IStockKLine>();
            foreach (IStockKLine selfKLine in selfKLines)
            {
                arrStockKLines.Add(selfKLine.KLinePreRestoration(selfBonus));
            }
            return arrStockKLines;
        }

        /// <summary>
        /// 前复权算法
        /// </summary>
        /// <param name="selfKLine">某一天的日线数据</param>
        /// <param name="selfBonus">历史股票分红配股数据</param>
        /// <returns></returns>
        public static IStockKLine KLinePreRestoration(this IStockKLine selfKLine,
            IEnumerable<IStockBonus> selfBonus)
        {
            //double tvalue = selfKLine.Close;
            //DateTime mytime = selfKLine.Time;
            //List<IStockBonus> selfList = selfBonus.ToList().OrderBy(p => p.ExdividendDate).ToList();
            //for (int i = 0; i < selfBonus.Count(); i++)
            //{
            //    // 前复权：复权后价格＝[(复权前价格-现金红利)＋配(新)股价格×流通股份变动比例]÷(1＋流通股份变动比例) 
            //    // 权后价格=(价格-红利/10)/(1+送股数/10)
            //    // 除权除息日当天就需要开始计算，因此在前复权情况下判断条件是mytime < ctime
            //    DateTime ctime = selfList[i].ExdividendDate;
            //    if (mytime < ctime)
            //    {
            //        double hongli = selfList[i].PreTaxDividend / 10;
            //        double songgu = selfList[i].BonusRate / 10;
            //        double liutongbdbl = selfList[i].IncreaseRate / 10 + songgu;
            //        double peigujia = selfList[i].DispatchPrice;
            //        double peigulv = selfList[i].DispatchRate / 10;
            //        tvalue = (tvalue - hongli + peigujia * peigulv) / (1 + liutongbdbl + peigulv);
            //    }
            //}
            //return Math.Round(tvalue, 2);

            double tvalue = selfKLine.Close;
            DateTime mytime = selfKLine.Time;
            IEnumerable<IStockBonus> selfList = selfBonus.OrderBy(p => p.ExdividendDate);

            foreach (IStockBonus bonus in selfList)
            {
                // 前复权：复权后价格＝[(复权前价格-现金红利)＋配(新)股价格×流通股份变动比例]÷(1＋流通股份变动比例) 
                // 权后价格=(价格-红利/10)/(1+送股数/10)
                // 除权除息日当天就需要开始计算，因此在前复权情况下判断条件是mytime < ctime
                if (mytime < bonus.ExdividendDate)
                {
                    double hongli = bonus.PreTaxDividend / 10;
                    double songgu = bonus.BonusRate / 10;
                    double liutongbdbl = bonus.IncreaseRate / 10 + songgu;
                    double peigujia = bonus.DispatchPrice;
                    double peigulv = bonus.DispatchRate / 10;
                    tvalue = (tvalue - hongli + peigujia * peigulv) / (1 + liutongbdbl + peigulv);
                }
            }

            //计算复权因子
            double adj = tvalue / selfKLine.Close;
            StockKLine newSelfKLine = selfKLine.ToDataObject();
            newSelfKLine.Close = Math.Round(tvalue, 2);
            newSelfKLine.Open = Math.Round(adj * newSelfKLine.Open, 2);            
            newSelfKLine.High = Math.Round(adj * newSelfKLine.High, 2);
            newSelfKLine.Low = Math.Round(adj * newSelfKLine.Low, 2);
            return newSelfKLine;
        }
    }
}
