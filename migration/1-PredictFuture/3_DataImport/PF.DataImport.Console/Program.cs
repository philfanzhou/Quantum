using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PF.DataImport.Console
{
    class Program
    {
        static bool AppEventRunning = true;
        public static bool ResponseReadLine = true;
        static SyncWebService syncService = new SyncWebService();

        static void Main(string[] args)
        {
            System.Console.CancelKeyPress += (sender, e) => { e.Cancel = true; };

            ConsoleForm.InitConsoleInfo();
            ConsoleForm.HideConsoleForm();

            Thread inputThread = new Thread(new ThreadStart(MonitorInput));
            inputThread.IsBackground = true;
            inputThread.Start();

            System.Windows.Forms.Timer m_AutoSyncTimer = new System.Windows.Forms.Timer();
            m_AutoSyncTimer.Tick += TimerTick;
            m_AutoSyncTimer.Interval += 1000 * 60 * 60;
            m_AutoSyncTimer.Enabled = true;
            m_AutoSyncTimer.Start();

            while (AppEventRunning)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }          
        }

        static void MonitorInput()
        {
            while (true)
            {
                string readStr = null;
                //若不需响应Console.ReadLine，表示自动同步，焦点需交给Console.ReadKey
                if (ResponseReadLine)
                {
                    readStr = System.Console.ReadLine();
                }

                if (!string.IsNullOrEmpty(readStr))
                {
                    readStr = readStr.ToLower();
                    switch (readStr)
                    {
                        case "exit":
                            AppEventRunning = false;
                            Thread.CurrentThread.Abort();
                            return;
                        case "hide":
                            //隐藏控制台程序
                            ConsoleForm.HideConsoleForm();
                            break;
                        case "sync":
                            //手动同步时，窗体焦点已交给Console.ReadKey，故不需要将ResponseReadLine赋值false;
                            ConsoleForm.InitConsoleInfo();
                            syncService.StartSynchronize("手动");
                            break;
                        default:
                            break;
                    }
                }

                Thread.Sleep(100);
            }
        }

        static void TimerTick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == int.Parse(AppConfigIO.StockSyncTime))
            {
                //若自动同步，则先取消Console.ReadLine等待，将焦点交给Console.ReadKey，0xD表示模拟的Enter键
                ConsoleForm.keybd_event(0xD, 0, 0, 0);
                ConsoleForm.keybd_event(0xD, 0, 2, 0);
                ResponseReadLine = false;
                syncService.StartSynchronize("自动");
            }
        }
    }
}
