using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quantum.Infrastructure.MarketData.Repository;
using Pitman.DataReader;
using Pitman.Metadata;
using System.Diagnostics;
using Quantum.Application.MarketData;
using System.Linq;

namespace Quantum.MarketData.Test
{
    /// <summary>
    /// Summary description for RealTimeFileTest
    /// </summary>
    [TestClass]
    public class RealTimeDataTest
    {
        private RealTimeData realTimedata;
        private bool dataUpdated = false;

        [TestMethod]
        public void TestCreateFile()
        {
            var moniter = new RealTimeDataMonitor(new List<string> { "sh600036" });
            moniter.dataChangedHandler += Moniter_dataChangedHandler;
            var repository = new RealTimeDataRepository();

            moniter.Start();
            while (DateTime.Now.Hour < 16)
            {
                System.Threading.Thread.Sleep(2000);

                if (dataUpdated)
                {
                    var realTimeItem = RealTimeItemConverter.Convert(realTimedata);
                    repository.Add(MarketType.Shanghai, "600036", realTimeItem);
                    Debug.WriteLine(realTimeItem.ToString());
                    dataUpdated = false;
                }
            }
        }


        private void Moniter_dataChangedHandler(object sender, DataChangedEventArgs e)
        {
            realTimedata = e.DataList.First();
            dataUpdated = true;
        }

        [TestMethod]
        public void TestReadData()
        {
            var repository = new RealTimeDataRepository();
            var dataList = repository.GetOneDayData(MarketType.Shanghai, "600036", DateTime.Now);
        }
    }
}


