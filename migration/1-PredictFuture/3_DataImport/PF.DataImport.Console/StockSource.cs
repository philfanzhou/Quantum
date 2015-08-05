using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DataImport.Console
{
    public abstract class StockSource
    {
        public abstract string StockDirectory { get; }

        public List<string> GetStockCodes()
        {
            List<string> stocks = new List<string>();
            string dataPath = StockDirectory;
            if (Directory.Exists(dataPath))
            {
                GetPathCode(stocks, dataPath);
            }
            return stocks;
        }

        private void GetPathCode(List<string> stocks, string dataPath)
        {
            string[] directories = Directory.GetDirectories(dataPath);
            if (directories != null && directories.Length > 0)
            {
                for (int i = 0; i < directories.Length; i++)
                {
                    GetPathCode(stocks, directories[i]);
                }
            }

            FileInfo[] files = new DirectoryInfo(dataPath).GetFiles();
            if (files != null && files.Length > 0)
            {
                stocks.AddRange(files.Select<FileInfo, string>(p => p.Name.ToLower().Replace(".day", string.Empty)));
            }
        }
    }

    public class ShStockSource : StockSource
    {
        public override string StockDirectory
        {
            get { return AppConfigIO.ShStockCodesDirectory; }
        }
    }

    public class SzStockSource : StockSource
    {
        public override string StockDirectory
        {
            get { return AppConfigIO.SzStockCodesDirectory; }
        }
    }
}
