using PF.Infrastructure.Impl.DataImport.DbConfig;
using PF.Infrastructure.Impl.DataImport.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PF.DataImport.Console
{
    public class SyncWebService
    {
        //股票计数
        private int m_currentCount = 0;
        private int m_totalCount = 1;
        //初始化光标位置
        private int m_CursorTop = 0;
        //进度计数和互斥锁
        private int m_preProtoss = 0;
        private int m_totalProtoss = 50;
        private object m_object = new object();
        //等待界面响应
        private bool m_Wait4Key;
        private DateTime m_SyncStart;

        private ConsoleColor m_ColorBack = System.Console.BackgroundColor;
        private ConsoleColor m_ColorFore = System.Console.ForegroundColor;

        private TaskFactory m_taskFactory;
        private CancellationTokenSource m_tokenSource;

        private readonly LogHelper m_otherLogger;
        private readonly LogHelper m_dayLineLogger;
        
        public SyncWebService()
        {
            Initializer.Initialize();
            m_otherLogger = LogFactory.GetLogger("other");
            m_dayLineLogger = LogFactory.GetLogger("daylinelogger");           
        }

        private void InitEnvironment()
        {
            m_currentCount = 0;
            m_preProtoss = 0;            
            m_Wait4Key = true;
            m_SyncStart = DateTime.Now;
            m_CursorTop = System.Console.CursorTop;

            m_tokenSource = new CancellationTokenSource();
            m_taskFactory = new TaskFactory(m_tokenSource.Token, TaskCreationOptions.None,
                TaskContinuationOptions.None, new ConsoleTaskScheduler(4));
        }

        public void StartSynchronize(string syncType)
        {
            //如果已在同步数据，则退出
            if (m_Wait4Key) return;

            //初始化环境变量
            InitEnvironment();

            //统计股票总数量
            string logStart = string.Format("{0} {1}同步股票服务...", m_SyncStart, syncType);
            System.Console.WriteLine(logStart);
            m_otherLogger.Info(logStart);
            List<string> shCodes = new ShStockSource().GetStockCodes();
            List<string> szCodes = new SzStockSource().GetStockCodes();
            m_totalCount = shCodes.Count() + szCodes.Count();
            string logCount = string.Format("股票总数：{0}，上证股票：{1}，深证股票：{2}",
                m_totalCount, shCodes.Count(), szCodes.Count());
            System.Console.WriteLine(logCount);
            m_otherLogger.Info(logCount);

            //初始化进度条颜色            
            System.Console.BackgroundColor = ConsoleColor.DarkGreen;
            for (int i = 0; i < m_totalProtoss; i++)
            {
                System.Console.Write(" ");
            }
            System.Console.WriteLine();
            System.Console.BackgroundColor = m_ColorBack;

            //调度上证日线任务
            var shQueue = new Queue<string>(shCodes);
            while (shQueue.Count > 0)
            {
                var shCode = shQueue.Dequeue();
                StockBusiness stockBusiness = new ShStockBusiness(shCode);
                var shTask = m_taskFactory.StartNew<string>(new Func<string>(() => { return stockBusiness.StockSync(); }));
                shTask.ContinueWith(TaskEnd);
            }

            //调度深证日线任务
            var szQueue = new Queue<string>(szCodes);
            while (szQueue.Count > 0)
            {
                var szCode = szQueue.Dequeue();
                StockBusiness stockBusiness = new SzStockBusiness(szCode);
                var szTask = m_taskFactory.StartNew<string>(new Func<string>(() => { return stockBusiness.StockSync(); }));
                szTask.ContinueWith(TaskEnd);
            }

            while (m_Wait4Key)
            {
                if (System.Console.KeyAvailable && System.Console.ReadKey(true).Key == ConsoleKey.F12)
                {                    
                    string logStop = string.Format("中断同步，已同步股票{0}个！", m_currentCount);
                    m_otherLogger.Info(logStop);

                    //将焦点交给Console.ReadLine
                    Program.ResponseReadLine = true;
                    m_Wait4Key = false;                    
                    m_tokenSource.Cancel();

                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.SetCursorPosition(0, m_CursorTop + 3);
                    System.Console.Write("正在取消请求，请等待...");
                    System.Console.ForegroundColor = m_ColorFore;
                }

                Thread.Sleep(50);
            }
        }

        private void TaskEnd(Task<string> task)
        {
            lock (m_object)
            {
                try
                {
                    if (task != null && task.Exception == null && !String.IsNullOrEmpty(task.Result))
                    {
                        m_dayLineLogger.Info(task.Result);
                    }
                }
                catch
                { 
                    //在取消同步时，会有异常信息
                }

                int curProgress = ++m_currentCount * m_totalProtoss / m_totalCount;
                if (curProgress != m_preProtoss)
                {
                    m_preProtoss = curProgress;
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                    System.Console.SetCursorPosition(curProgress - 1, m_CursorTop + 2);
                    System.Console.Write(" ");
                    System.Console.BackgroundColor = m_ColorBack;
                }

                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.SetCursorPosition(0, m_CursorTop + 3);
                System.Console.Write("同步进度：{0}/{1}       ", m_currentCount, m_totalCount);
                System.Console.ForegroundColor = m_ColorFore;

                if (m_currentCount == m_totalCount)
                {
                    TimeSpan span = DateTime.Now - m_SyncStart;
                    int consumeSeconds = Convert.ToInt32(span.TotalSeconds);

                    if (m_Wait4Key)
                    {
                        string logFinish = string.Format("同步完成，总计耗时：{0}秒", consumeSeconds);
                        System.Console.Write(logFinish);
                        m_otherLogger.Info(logFinish);
                        m_Wait4Key = false;
                    }
                    else
                    {
                        string logCancel = string.Format("任务取消，总计耗时：{0}秒", consumeSeconds);
                        System.Console.Write(logCancel);
                        m_otherLogger.Info(logCancel);
                    }
                }

                System.Console.WriteLine();
            }            
        }
    }
}
