using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.IO;

namespace PF.DataImport.Console
{
    public class AppConfigIO
    {
        private static string m_StockSyncTime = ConfigurationManager.AppSettings["StockSyncTime"];
        private static string m_ShStockCodesDirectory = ConfigurationManager.AppSettings["ShStockCodesDirectory"];
        private static string m_SzStockCodesDirectory = ConfigurationManager.AppSettings["SzStockCodesDirectory"];
        private static string m_DividendFile = ConfigurationManager.AppSettings["DividendFile"];

        public static string CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        public static string StockSyncTime
        {
            get 
            {
                if (string.IsNullOrEmpty(m_StockSyncTime))
                {
                    return "16";
                }
                else
                {
                    return m_StockSyncTime;
                }
            }
        }

        public static string ShStockCodesDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(m_ShStockCodesDirectory))
                {
                    return CurrentDirectory;
                }
                else
                {
                    return Path.Combine(CurrentDirectory, m_ShStockCodesDirectory);
                }
            }
        }

        public static string SzStockCodesDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(m_SzStockCodesDirectory))
                {
                    return CurrentDirectory;
                }
                else
                {
                    return Path.Combine(CurrentDirectory, m_SzStockCodesDirectory);
                }
            }
        }

        private static string DividendFile
        {
            get
            {
                if (string.IsNullOrEmpty(m_DividendFile))
                {
                    return null;
                }
                else
                {
                    return m_DividendFile;
                }
            } 
        }
    }
}
