using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PF.DataImport.Console
{
    public class ConsoleForm
    {
        static string m_ConsoleTitle = "PF Stock Web-Sync";
        static NotifyIcon m_NotifyIcon = new NotifyIcon();

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("user32.dll")]
        //模拟键盘输入函数 dwFlags：0表示KeyDown按下 2表示KeyUp
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo); 

        static ConsoleForm()
        {
            //设置控制台标题名
            System.Console.Title = m_ConsoleTitle;
            DisableCloseButton();

            m_NotifyIcon.Icon = Properties.Resources.SyncWS;
            m_NotifyIcon.Visible = false;
            m_NotifyIcon.Text = m_ConsoleTitle;
            m_NotifyIcon.MouseDoubleClick += m_NotifyIcon_MouseDoubleClick;
        }

        public static void InitConsoleInfo()
        {
            System.Console.Clear();
            System.Console.WriteLine("自动同步时间：每日{0}点 *********** 命令说明 ******", AppConfigIO.StockSyncTime);
            System.Console.WriteLine("***************** exit：关闭服务 *****************");
            System.Console.WriteLine("***************** hide：隐藏服务 *****************");
            System.Console.WriteLine("***************** sync：手动同步 *****************");
            System.Console.WriteLine("*****************  F12：暂停同步 *****************");
            System.Console.WriteLine("**************************************************");
            System.Console.WriteLine();
        }

        /// <summary>
        /// 禁用关闭按钮
        /// </summary>
        private static void DisableCloseButton()
        {            
            //线程睡眠，确保closebtn中能够正常FindWindow，否则有时会Find失败。。
            Thread.Sleep(100);
            //根据控制台标题找控制台
            IntPtr WINDOW_HANDLER = FindWindow(null, m_ConsoleTitle);
            //找关闭按钮
            IntPtr CLOSE_MENU = GetSystemMenu((IntPtr)WINDOW_HANDLER, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            //关闭按钮禁用
            RemoveMenu(CLOSE_MENU, SC_CLOSE, 0x0);
        }

        /// <summary>
        /// 隐藏控制台程序
        /// </summary>
        public static void HideConsoleForm()
        {
            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr et = new IntPtr(0);
            ParenthWnd = FindWindow(null, m_ConsoleTitle);
            //隐藏Dos窗体, 0: 后台执行；1:正常启动；2:最小化到任务栏；3:最大化
            ShowWindow(ParenthWnd, 0); 
            m_NotifyIcon.Visible = true;
            m_NotifyIcon.ShowBalloonTip(1000, "", m_ConsoleTitle, ToolTipIcon.Info);
        }
        
        static void m_NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            IntPtr ParenthWnd = new IntPtr(0);
            IntPtr et = new IntPtr(0);
            ParenthWnd = FindWindow(null, m_ConsoleTitle);
            //显示Dos窗体, 0: 后台执行；1:正常启动；2:最小化到任务栏；3:最大化
            ShowWindow(ParenthWnd, 1);
            m_NotifyIcon.Visible = false;
        } 
    }
}
