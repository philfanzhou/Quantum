using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quantum.ConsoleApp
{
    class Program
    {
        #region Field
        private static Mutex mutex = new Mutex(true, "Quantum.OnlyRun");
        #endregion

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if (IsAlreadyRunning())
            {
                return;
            }

            Console.WriteLine("=================================");
            Console.WriteLine("Quantum.ConsoleApp");
            Console.WriteLine("=================================");

            try
            {
                // Run Service
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }

            WaitingForExit();
        }

        private static bool IsAlreadyRunning()
        {
            if (!mutex.WaitOne(0, false))
            {
                Console.WriteLine("Quantum is already running, please don't run the application again.");
                Console.WriteLine("Press any key to exit....");
                Console.ReadKey();
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void WaitingForExit()
        {
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
