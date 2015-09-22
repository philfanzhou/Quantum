using Pitman.DataReader;
using Pitman.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Quantum.Application.MarketData
{
    public class RealTimeDataMonitor
    {
        private readonly IRealTimeDataReader dataReader = DataReaderCreator.Create();

        /// <summary>
        /// 用于存储数据的缓存，最新数据与此缓存进行比较，从而判断数据是否已经更新
        /// </summary>
        private Dictionary<string, RealTimeData> buffer = new Dictionary<string, RealTimeData>();

        /// <summary>
        /// 分时数据的精度为5秒，这里取3秒时间差作为数据是否已经更新的依据
        /// </summary>
        private TimeSpan span = new TimeSpan(0, 0, 3);

        private readonly IEnumerable<string> stockCodes;

        private Timer queryTimer;

        public RealTimeDataMonitor(IEnumerable<string> stockCodes)
        {
            if (null == stockCodes)
                throw new ArgumentNullException("stockCodes");

            this.stockCodes = stockCodes;

            foreach(string stockCode in stockCodes)
            {
                this.buffer.Add(stockCode, null);
            }
        }

        public event EventHandler<DataChangedEventArgs> dataChangedHandler;

        public void Start()
        {
            queryTimer = new Timer(3000);
            queryTimer.Elapsed += CheckNewRealTimeData;
            queryTimer.Enabled = true;
        }

        public void Stop()
        {
            if (queryTimer != null)
            {
                queryTimer.Stop();
                queryTimer.Dispose();
                queryTimer = null;
            }
        }

        private void CheckNewRealTimeData(object sender, ElapsedEventArgs e)
        {
            List<RealTimeData> newDataList = new List<RealTimeData>();

            List<RealTimeData> datas = null;
            try
            {
                // 避免网络出现异常
                datas = this.dataReader.GetData(this.stockCodes).ToList();
            }
            finally
            {
                if (null != datas)
                {
                    // 找出有更新的数据
                    foreach (var data in datas)
                    {
                        var preData = this.buffer[data.Code];
                        if (null == preData ||
                            data.Time - preData.Time > this.span)
                        {
                            this.buffer[data.Code] = data;
                            newDataList.Add(data);
                        }
                    }

                    if (newDataList.Count > 0)
                    {
                        // 触发事件上报数据更新
                        RaiseChangedData(newDataList);
                    }
                }
            }
        }

        private void RaiseChangedData(List<RealTimeData> newDataList)
        {
            if(null != this.dataChangedHandler)
            {
                DataChangedEventArgs args = new DataChangedEventArgs(newDataList);
                this.dataChangedHandler.Invoke(null, args);
            }
        }
    }

    public class DataChangedEventArgs : EventArgs
    {
        public IEnumerable<RealTimeData> DataList
        {
            get;
            private set;
        }

        public DataChangedEventArgs(List<RealTimeData> newDataList)
        {
            DataList = newDataList;
        }
    }
}
