using Core.Infrastructure.Crosscutting.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PF.DistributedService.Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================================");
            Console.WriteLine("");
            Console.WriteLine("=================================");

            if (Process.GetProcessesByName("PF.DistributedService.Hosting").Length > 1)
            {
                Console.WriteLine("The PF.DistributedService.Hosting is already running");
                Console.WriteLine("Press any key to exit....");
                Console.Read();
            }
            else
            {
                ServiceInitialize.Init();

                var serviceManager =
                    new ServiceManager(new List<object>
                    {
                        typeof (AuthenticationService),
                        typeof (OAuth2AuthorizationServer),
                        typeof (CrosscuttingService),
                        typeof (FinanceDataService),
                        typeof (PriceDataService)
                    });
                serviceManager.Open();
                Console.WriteLine("service start sucessfully.");
                Console.Read();
            }
        }
    }
}
