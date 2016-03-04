using GMSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.Application.GoldMiner
{
    class Program
    {
        private static string userName;
        private static string password;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            //if (IsAlreadyRunning())
            //{
            //    return;
            //}

            Console.WriteLine("=================================");
            Console.WriteLine("Quantum.GoldMiner");
            Console.WriteLine("=================================");

            WaitInputUsernameAndPwd();

            Console.WriteLine(Login());

            WaitingForExit();
        }

        /// <summary>
        /// 输入登陆掘金的用户名和密码
        /// </summary>
        private static void WaitInputUsernameAndPwd()
        {
            Console.WriteLine("Please input username and password for GoldMiner.");
            Console.Write("Username:");
            userName = Console.ReadLine();

            Console.Write("Password:");
            ConsoleKeyInfo info;
            do
            {
                info = Console.ReadKey(true);
                if (info.Key != ConsoleKey.Enter
                    && info.Key != ConsoleKey.Backspace
                    && info.Key != ConsoleKey.Escape
                    && info.Key != ConsoleKey.Tab
                    && info.KeyChar != '\0')
                {
                    password += info.KeyChar;
                    Console.Write('*');
                }
            } while (info.Key != ConsoleKey.Enter);

            Console.WriteLine();
        }

        private static bool Login()
        {
            int ret = MdApi.Instance.Init(userName, password);

            return ret == 0;
        }

        /// <summary>
        /// 等待程序退出
        /// </summary>
        private static void WaitingForExit()
        {
            Console.WriteLine();
            Console.WriteLine("\r\npress exit to close the application....");
            while (true)
            {
                if (Console.ReadLine().ToLower() == "exit")
                {
                    break;
                }
            }
        }
    }
}
