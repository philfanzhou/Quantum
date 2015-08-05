using PF.DataImport.AppService;
using PF.DataImport.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PF.DataImport.Console
{
    public class Eastmoney
    {
        private string m_EastUrl;

        private string m_Symbol;

        private string m_Suffix = "sh"; //sz

        private ImportStockBaseService stockservice = new ImportStockBaseService();

        public Eastmoney(string suffix, string symbol) 
        {
            m_Symbol = symbol;

            if (suffix == ".sz")
            {
                m_Suffix = "sz";
            }

            m_EastUrl = new StringBuilder(@"http://quote.eastmoney.com/").Append(m_Suffix).Append(m_Symbol).Append(".html").ToString();
        }

        public void RefreshStockBase()
        {
            StockBase stockBase = stockservice.GetStockBaseBySymbol(m_Symbol);

            var request = WebRequest.Create(m_EastUrl) as HttpWebRequest;
            var response = request.GetResponse() as HttpWebResponse;
            var sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
            string html = sr.ReadToEnd();

            bool newStockBase = false;
            if (stockBase == null)
            {
                newStockBase = true;
                stockBase = new StockBase();
            }

            string reg = @"<title>.*?</title>";
            string name = Regex.Match(html, reg).Value.Split(new string[] { @"title>" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</title>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd"">上市时间</td><td class=""vds"">.*?</td>";
            string listingDate = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd"">总股本</td><td class=""vds""><span id=""prozgb"" .*?></span>.*?</td>";
            string totalCapital = Regex.Match(html, reg).Value.Split(new string[] { @"</span>" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd"">流通股</td><td class=""vds ls""><span id=""proltgb"" .*?></span>.*?</td>";
            string floatingCapital = Regex.Match(html, reg).Value.Split(new string[] { @"</span>" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd""><a href=.*? target=""_blank"">每股收益(.*?一.*?)</a></td><td class=""vds"">.*?</td>";
            string epsOne = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd""><a href=.*? target=""_blank"">每股净资产</a></td><td class=""vds"">.*?</td>";
            string naps = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd""><a href=.*? target=""_blank"">净资产收益率</a></td>\r\n.*?\t.*?<td>.*?</td>";
            string roe = Regex.Match(html, reg).Value.Split(new string[] { @"<td>" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd"">营业收入</td><td class=""vds"">.*?</td>";
            string br = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd""><a href=.*? target=""_blank"">营业收入增长率</a></td><td class=""vds"">.*?</td>";
            string irbr = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd""><a href=.*? target=""_blank"">销售毛利率</a></td><td class=""vds ls"">.*?</td>";
            string profitmargi = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds ls"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd"">净利润</td><td class=""vds"">.*?</td>";
            string np = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd"">净利润增长率</td><td class=""vds"">.*?</td>";
            string irnp = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            reg = @"<td class=""hd"">每股未分配利润</td><td class=""vds ls"">.*?</td></tr>";
            string upps = Regex.Match(html, reg).Value.Split(new string[] { @"<td class=""vds ls"">" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { @"</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];

            stockBase.Symbol = m_Symbol;
            stockBase.Name = name.Split('(')[0];
            stockBase.ListingDate = Convert.ToDateTime(listingDate);
            stockBase.TotalCapital = ChangeUnit(totalCapital);
            stockBase.FloatingCapital = ChangeUnit(floatingCapital);
            stockBase.EPSOne = ChangeUnit(epsOne);
            stockBase.NAPS = ChangeUnit(naps);
            stockBase.ROE = ChangeUnit(roe);
            stockBase.BR = ChangeUnit(br);
            stockBase.IRBR = ChangeUnit(irbr);
            stockBase.Profitmargi = ChangeUnit(profitmargi);
            stockBase.NP = ChangeUnit(np);
            stockBase.IRNP = ChangeUnit(irnp);
            stockBase.UPPS = ChangeUnit(upps);

            if (newStockBase)
            {
                stockservice.ImportStockBase(stockBase);
            }
            else
            {
                stockservice.UpdateStockBase(stockBase);
            }
        }

        public static double ChangeUnit(string value)
        {
            value = value.Replace("股", string.Empty);
            value = value.Replace("元", string.Empty);
            if (value.Contains("-"))
            {
                return 0;
            }

            if (value.Contains("%"))
            {
                double returnValue = Convert.ToDouble(value.Replace("%", string.Empty)) / 100;
                return returnValue;
            }

            if (value.Contains("亿"))
            {
                double returnValue = Convert.ToDouble(value.Replace("亿", string.Empty)) * 100000000;
                return returnValue;
            }
            if (value.Contains("万"))
            {
                double returnValue = Convert.ToDouble(value.Replace("万", string.Empty)) * 10000;
                return returnValue;
            }

            return double.Parse(value);
        }
    }
}
