using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ore.Infrastructure.MarketData;
using Pitman.RESTful.Client;
using Quantum.Domain.MarketData;

namespace Test.Domain.MarketData
{
    [TestClass]
    public class TestIndication
    {
        private const string serverAddress = @"http://quantum1234.cloudapp.net:6688/api";

        private void SampleMacdDatas(List<IStockKLine> lstStockKLine,
            out List<double> lstEma12,
            out List<double> lstEma26,
            out List<double> lstDif,
            out List<double> lstDea,
            out List<double> lstMacd)
        {
            int SHORT = 12;
            int LONG = 26;
            int M = 9;
            lstEma12 = new List<double>();
            lstEma26 = new List<double>();
            lstDif = new List<double>();
            lstDea = new List<double>();
            lstMacd = new List<double>();
            for (int i = 0; i < lstStockKLine.Count; i++)
            {
                if (i == 0)
                {
                    lstEma12.Add(lstStockKLine[i].Close);
                    lstEma26.Add(lstStockKLine[i].Close);
                    lstDif.Add(lstEma12[i] - lstEma26[i]);
                    lstDea.Add(lstDif[i]);
                    lstMacd.Add(2.0 * (lstDif[i] - lstDea[i]));
                }
                else
                {
                    lstEma12.Add((2 * lstStockKLine[i].Close + (SHORT - 1) * lstEma12[i - 1]) / (SHORT + 1));
                    lstEma26.Add((2 * lstStockKLine[i].Close + (LONG - 1) * lstEma26[i - 1]) / (LONG + 1));
                    lstDif.Add(lstEma12[i] - lstEma26[i]);
                    lstDea.Add((2 * lstDif[i] + (M - 1) * lstDea[i - 1]) / (M + 1));
                    lstMacd.Add(2.0 * (lstDif[i] - lstDea[i]));
                }
            }
        }

        [TestMethod]
        public void TestMA()
        {
            using (var client = new ClientApi(serverAddress))
            {
                // 从Pitman获取K线数据
                var lstKLine = client.GetStockKLine(KLineType.Day, "603085", new DateTime(2015,6, 1), new DateTime(2016, 3, 4));
                
                // 调用K线扩展方法获取MA指标的值
                var lstMA5 = lstKLine.MA(5).ToList();
                var lstMA10 = lstKLine.MA(10).ToList();
                var lstMA20 = lstKLine.MA(20).ToList();
                var lstMA60 = lstKLine.MA(60).ToList();

                // 判断值的正确性
                Assert.AreEqual(lstMA5[0].Value, 12.79);
                Assert.AreEqual(lstMA5[1].Value, 14.06);
                Assert.AreEqual(lstMA5[5].Value, 20.59);
                
                Assert.AreEqual(lstMA10[0].Value, 16.69);
                Assert.AreEqual(lstMA10[1].Value, 18.36);
                Assert.AreEqual(lstMA10[10].Value, 30.77);
                
                Assert.AreEqual(lstMA20[0].Value, 23.73);
                Assert.AreEqual(lstMA20[1].Value, 25.19);
                Assert.AreEqual(lstMA20[40].Value, 33.01);
                
                Assert.AreEqual(lstMA60[0].Value, 34.99);
                Assert.AreEqual(lstMA60[1].Value, 35.42);
                Assert.AreEqual(lstMA60[2].Value, 35.78);

                lstKLine = client.GetStockKLine(KLineType.Day, "603085", new DateTime(2015, 6, 1), new DateTime(2016, 3, 8));

                // 调用K线扩展方法获取MA指标的值
                lstMA5 = lstKLine.MA(5, lstMA5).ToList();
                lstMA10 = lstKLine.MA(10, lstMA10).ToList();
                lstMA20 = lstKLine.MA(20, lstMA20).ToList();
                lstMA60 = lstKLine.MA(60, lstMA60).ToList();

                Assert.AreEqual(lstMA5[0].Value, 12.79);
                Assert.AreEqual(lstMA10[0].Value, 16.69);
                Assert.AreEqual(lstMA20[0].Value, 23.73);
                Assert.AreEqual(lstMA60[0].Value, 34.99);

                int count = lstKLine.ToList().Count;
                Assert.AreEqual(lstMA5[count - 5].Value, 30.45);
                Assert.AreEqual(lstMA10[count - 10].Value, 30.35);
                Assert.AreEqual(lstMA20[count - 20].Value, 31.37);
                Assert.AreEqual(lstMA60[count - 60].Value, 37.58);
            }
        }

        [TestMethod]
        public void TestMACD()
        {
            using (var client = new ClientApi(serverAddress))
            {
                // 从Pitman获取K线数据
                //600036
                var lstKLine = client.GetStockKLine(KLineType.Day, "300500", new DateTime(2015, 6, 1), new DateTime(2016, 3, 1));

                //List<double> lstEma12;
                //List<double> lstEma26;
                //List<double> lstDif;
                //List<double> lstDea;
                //List<double> lstMacd;
                //SampleMacdDatas(lstKLine.ToList(), out lstEma12, out lstEma26, out lstDif, out lstDea, out lstMacd);

                // 调用K线扩展方法获取MACD指标的值
                var lstMACD = lstKLine.MACD().ToList();

                // 判断值的正确性
                //int i = 0;
                //foreach (var macd in lstMACD)
                //{
                //    Assert.AreEqual(macd.DIF, lstDif[i]);
                //    Assert.AreEqual(macd.DEA, lstDea[i]);
                //    Assert.AreEqual(macd.MACD, lstMacd[i]);
                //    i++;
                //}                

                //I DIF DEA MACD
                //0   0   0   0
                Assert.AreEqual(lstMACD[0].DIF, 0);
                Assert.AreEqual(lstMACD[0].DEA, 0);
                Assert.AreEqual(lstMACD[0].MACD, 0);
                //1           
                Assert.AreEqual(Math.Round(lstMACD[1].DIF, 2), 0.24);
                Assert.AreEqual(Math.Round(lstMACD[1].DEA, 2), 0.05);
                Assert.AreEqual(Math.Round(lstMACD[1].MACD, 2), 0.38);
                //2           
                Assert.AreEqual(Math.Round(lstMACD[2].DIF, 2), 0.69);
                Assert.AreEqual(Math.Round(lstMACD[2].DEA, 2), 0.18);
                Assert.AreEqual(Math.Round(lstMACD[2].MACD, 2), 1.03);
                //3           
                Assert.AreEqual(Math.Round(lstMACD[3].DIF, 2), 1.32);
                Assert.AreEqual(Math.Round(lstMACD[3].DEA, 2), 0.41);
                Assert.AreEqual(Math.Round(lstMACD[3].MACD, 2), 1.84);
                //4           
                Assert.AreEqual(Math.Round(lstMACD[4].DIF, 2), 2.13);
                Assert.AreEqual(Math.Round(lstMACD[4].DEA, 2), 0.75);
                Assert.AreEqual(Math.Round(lstMACD[4].MACD, 2), 2.75);
                //5           
                Assert.AreEqual(Math.Round(lstMACD[5].DIF, 2), 3.08);
                Assert.AreEqual(Math.Round(lstMACD[5].DEA, 2), 1.22);
                Assert.AreEqual(Math.Round(lstMACD[5].MACD, 2), 3.73);
                //6          
                Assert.AreEqual(Math.Round(lstMACD[6].DIF, 2), 4.18);
                Assert.AreEqual(Math.Round(lstMACD[6].DEA, 2), 1.81);
                Assert.AreEqual(Math.Round(lstMACD[6].MACD, 2), 4.75);
                //7           
                Assert.AreEqual(Math.Round(lstMACD[7].DIF, 2), 5.42);
                Assert.AreEqual(Math.Round(lstMACD[7].DEA, 2), 2.53);
                Assert.AreEqual(Math.Round(lstMACD[7].MACD, 2), 5.78);
                //8           
                Assert.AreEqual(Math.Round(lstMACD[8].DIF, 2), 6.80);
                Assert.AreEqual(Math.Round(lstMACD[8].DEA, 2), 3.39);
                Assert.AreEqual(Math.Round(lstMACD[8].MACD, 2), 6.83);
                //9           
                Assert.AreEqual(Math.Round(lstMACD[9].DIF, 2), 8.32);
                Assert.AreEqual(Math.Round(lstMACD[9].DEA, 2), 4.37);
                Assert.AreEqual(Math.Round(lstMACD[9].MACD, 2), 7.89);
                //10          
                Assert.AreEqual(Math.Round(lstMACD[10].DIF, 2), 9.98);
                Assert.AreEqual(Math.Round(lstMACD[10].DEA, 2), 5.49);
                Assert.AreEqual(Math.Round(lstMACD[10].MACD, 2), 8.97);
                //11         
                Assert.AreEqual(Math.Round(lstMACD[11].DIF, 2), 10.96);
                Assert.AreEqual(Math.Round(lstMACD[11].DEA, 2), 6.59);
                Assert.AreEqual(Math.Round(lstMACD[11].MACD, 2), 8.74);
                //12         
                Assert.AreEqual(Math.Round(lstMACD[12].DIF, 2), 11.00);
                Assert.AreEqual(Math.Round(lstMACD[12].DEA, 2), 7.47);
                Assert.AreEqual(Math.Round(lstMACD[12].MACD, 2), 7.06);
                //13         
                Assert.AreEqual(Math.Round(lstMACD[13].DIF, 2), 10.61);
                Assert.AreEqual(Math.Round(lstMACD[13].DEA, 2), 8.10);
                Assert.AreEqual(Math.Round(lstMACD[13].MACD, 2), 5.02);

                lstKLine = client.GetStockKLine(KLineType.Day, "300500", new DateTime(2015, 7, 1), new DateTime(2016, 3, 8));
                lstMACD = lstKLine.MACD(lstMACD).ToList();
                //13         
                Assert.AreEqual(Math.Round(lstMACD[13].DIF, 2), 10.61);
                Assert.AreEqual(Math.Round(lstMACD[13].DEA, 2), 8.10);
                Assert.AreEqual(Math.Round(lstMACD[13].MACD, 2), 5.02);
                //14         
                Assert.AreEqual(Math.Round(lstMACD[14].DIF, 2), 10.38);
                Assert.AreEqual(Math.Round(lstMACD[14].DEA, 2), 8.55);
                Assert.AreEqual(Math.Round(lstMACD[14].MACD, 2), 3.65);
                //15         
                Assert.AreEqual(Math.Round(lstMACD[15].DIF, 2), 10.07);
                Assert.AreEqual(Math.Round(lstMACD[15].DEA, 2), 8.86);
                Assert.AreEqual(Math.Round(lstMACD[15].MACD, 2), 2.42);
                //16          
                Assert.AreEqual(Math.Round(lstMACD[16].DIF, 2), 9.92);
                Assert.AreEqual(Math.Round(lstMACD[16].DEA, 2), 9.07);
                Assert.AreEqual(Math.Round(lstMACD[16].MACD, 2), 1.70);
                //17          
                Assert.AreEqual(Math.Round(lstMACD[17].DIF, 2), 9.60);
                Assert.AreEqual(Math.Round(lstMACD[17].DEA, 2), 9.17);
                Assert.AreEqual(Math.Round(lstMACD[17].MACD, 2), 0.85);
                //18       
                Assert.AreEqual(Math.Round(lstMACD[18].DIF, 2), 9.02);
                Assert.AreEqual(Math.Round(lstMACD[18].DEA, 2), 9.14);
                Assert.AreEqual(Math.Round(lstMACD[18].MACD, 2), -0.25);
            }
        }

        [TestMethod]
        public void TestKDJ()
        {
            using (var client = new ClientApi(serverAddress))
            {
                // 从Pitman获取K线数据
                var lstKLine = client.GetStockKLine(KLineType.Day, "300500", new DateTime(2015, 6, 1), new DateTime(2016, 3, 1));

                // 调用K线扩展方法获取MA指标的值
                var lstKDJ = lstKLine.KDJ().ToList();

                // 判断值的正确性
                Assert.AreEqual(lstKDJ[0].KValue, 100);
                Assert.AreEqual(lstKDJ[0].DValue, 100);
                Assert.AreEqual(lstKDJ[0].JValue, 100);

                Assert.AreEqual(lstKDJ[10].KValue, 100);
                Assert.AreEqual(lstKDJ[10].DValue, 100);
                Assert.AreEqual(lstKDJ[10].JValue, 100);

                Assert.AreEqual(Math.Round(lstKDJ[11].KValue, 2), 93.42);
                Assert.AreEqual(Math.Round(lstKDJ[11].DValue, 2), 97.81);
                Assert.AreEqual(Math.Round(lstKDJ[11].JValue, 2), 84.64);

                Assert.AreEqual(Math.Round(lstKDJ[12].KValue, 2), 82.10);
                Assert.AreEqual(Math.Round(lstKDJ[12].DValue, 2), 92.57);
                Assert.AreEqual(Math.Round(lstKDJ[12].JValue, 2), 61.16);

                Assert.AreEqual(Math.Round(lstKDJ[13].KValue, 2), 69.39);
                Assert.AreEqual(Math.Round(lstKDJ[13].DValue, 2), 84.84);
                Assert.AreEqual(Math.Round(lstKDJ[13].JValue, 2), 38.48);

                lstKLine = client.GetStockKLine(KLineType.Day, "300500", new DateTime(2015, 6, 1), new DateTime(2016, 3, 8));

                // 调用K线扩展方法获取MA指标的值
                lstKDJ = lstKLine.KDJ(lstKDJ).ToList();

                Assert.AreEqual(Math.Round(lstKDJ[13].KValue, 2), 69.39);
                Assert.AreEqual(Math.Round(lstKDJ[13].DValue, 2), 84.84);
                Assert.AreEqual(Math.Round(lstKDJ[13].JValue, 2), 38.48);

                Assert.AreEqual(Math.Round(lstKDJ[14].KValue, 2), 60.72);
                Assert.AreEqual(Math.Round(lstKDJ[14].DValue, 2), 76.80);
                Assert.AreEqual(Math.Round(lstKDJ[14].JValue, 2), 28.57);

                Assert.AreEqual(Math.Round(lstKDJ[15].KValue, 2), 50.70);
                Assert.AreEqual(Math.Round(lstKDJ[15].DValue, 2), 68.10);
                Assert.AreEqual(Math.Round(lstKDJ[15].JValue, 2), 15.89);

                Assert.AreEqual(Math.Round(lstKDJ[16].KValue, 2), 45.70);
                Assert.AreEqual(Math.Round(lstKDJ[16].DValue, 2), 60.63);
                Assert.AreEqual(Math.Round(lstKDJ[16].JValue, 2), 15.82);

                Assert.AreEqual(Math.Round(lstKDJ[17].KValue, 2), 40.70);
                Assert.AreEqual(Math.Round(lstKDJ[17].DValue, 2), 53.99);
                Assert.AreEqual(Math.Round(lstKDJ[17].JValue, 2), 14.13);

                Assert.AreEqual(Math.Round(lstKDJ[18].KValue, 2), 33.32);
                Assert.AreEqual(Math.Round(lstKDJ[18].DValue, 2), 47.10);
                Assert.AreEqual(Math.Round(lstKDJ[18].JValue, 2), 5.76);                
            }
        }
    }
}
