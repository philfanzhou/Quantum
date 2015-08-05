using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PF.DataImport.AppService;
using PF.DataImport.Domain;

namespace PF.IFS.TongHua.Presentation.WinForm
{
    using PF.IFS.TongHua.DataReader;
    using PF.Infrastructure.Impl.DataImport.System;

    public class Importer
    {
        private const int START = 10;
        private const int END = 99;

        private static readonly object _incrementLock = new object();
        private static readonly object _dayLineLoggerLock = new object();
        private static readonly object _divedendLoggerLock = new object();

        private static int _stepCount;
        private static int _currentCount;
        private static bool _hasError;

        private readonly LogHelper _otherLogger;
        private readonly LogHelper _dayLineLogger;
        private readonly LogHelper _divedendLogger;

        private DataReader _dataReader;
        private TaskFactory _taskFactory;
        private static CancellationTokenSource _tokenSource;

        public string ShaseDayLineFolder { get; set; }

        public string SznseDayLineFolder { get; set; }

        public string DivedendFile { get; set; }

        public bool ImportShase { get; set; }

        public bool ImportSznse { get; set; }

        public bool ImportDivedend { get; set; }

        public event EventHandler<ProgressArgs> ProgressChanged;

        public Importer()
        {
            Initializer.Initialize();
            _otherLogger = LogFactory.GetLogger("other");
            _dayLineLogger = LogFactory.GetLogger("daylinedatalogger");
            _divedendLogger = LogFactory.GetLogger("dividenddatalogger");
        }

        public void StartImport()
        {
            _hasError = false;
            _stepCount = 0;
            _currentCount = 0;
            _dataReader = new DataReader();
            _tokenSource = new CancellationTokenSource();
            _taskFactory = new TaskFactory(_tokenSource.Token, TaskCreationOptions.None, TaskContinuationOptions.None, new LimitedConcurrencyLevelTaskScheduler(5));
            var importtask = Task.Factory.StartNew(InnerImport);
            importtask.ContinueWith(ProcessException);
        }

        private void InnerImport()
        {
            _otherLogger.Info("开始导入....");
            ReportProgress(0, "开始导入....");

            _otherLogger.Info("预处理文件");
            ReportProgress(5, "预处理文件....");

            if (ImportShase)
            {
                _otherLogger.Info(string.Format("\t上证：{0}", ShaseDayLineFolder));
            }

            if (ImportSznse)
            {
                _otherLogger.Info(string.Format("\t深证：{0}", SznseDayLineFolder));
            }

            if (ImportDivedend)
            {
                _otherLogger.Info(string.Format("\t除权：{0}", DivedendFile));
            }

            var importdayline = ImportShase || ImportSznse;
            if (importdayline)
            {
                var files = ImportShase && ImportSznse
                    ? new[] { ShaseDayLineFolder, SznseDayLineFolder }
                    : ImportShase
                        ? new[] { ShaseDayLineFolder }
                        : ImportSznse
                            ? new[] { SznseDayLineFolder }
                            : new string[] { };
                _dataReader.AnalyseDayLineFiles(files);
            }

            if (ImportDivedend)
            {
                _dataReader.AnalyseDividendFile(DivedendFile);
            }

            _stepCount = _dataReader.DayLineSymbols.Count() + _dataReader.DividendSymbols.Count();
            _currentCount = 0;

            if (_stepCount == 0 || (importdayline == false && ImportDivedend == false))
            {
                _otherLogger.Info("完成但没有导入任何数据。");
                ReportProgress(100, "完成但没有导入任何数据。");
                return;
            }

            if (importdayline)
            {
                DayLineDataImport();
            }

            if (ImportDivedend)
            {
                DividendDataImport();
            }
        }

        private void DayLineDataImport()
        {
            _dayLineLogger.Info("\t导入日线数据");
            ReportProgress(10, "导入日线数据....");

            var symbols = _dataReader.DayLineSymbols.ToArray();
            _dayLineLogger.Info(string.Format("\t{0}个日线文件待导入", symbols.Length));

            var symbolqueue = new Queue<string>(symbols);
            while (symbolqueue.Count > 0)
            {
                var symbol = symbolqueue.Dequeue();
                var task = _taskFactory.StartNew<string>(DayLineDataImport, symbol);
                task.ContinueWith(TaskEnd);
            }
        }

        private string DayLineDataImport(object state)
        {
            var importmessage = string.Empty;
            if (_tokenSource.IsCancellationRequested)
            {
                return importmessage;
            }

            var symbol = (string)state;
            var messages = new List<string>();

            //var domainservice = CreateDomainService();
            //var stockservice = new ImportStockService(domainservice);
            //var dailypriceservice = new ImportDailyPriceDataService(domainservice);
            var stockservice = new ImportStockService();
            var dailypriceservice = new ImportDailyPriceDataService();

            messages.Add(string.Format("\t解析文件：{0}.day", symbol));
            var stock = stockservice.GetStockBySymbol(symbol);
            if (stock == null)
            {
                messages.Add("\t\t开始创建股票对象");
                stock = new Stock { Symbol = symbol, Name = string.Empty };
                stockservice.ImportStock(stock);
                messages.Add("\t\t完成创建股票对象");
            }

            var starttime = dailypriceservice.GetLatestDailyPriceDataImportDateTime(symbol);
            messages.Add(string.Format("\t\t获取最新记录的时间：{0}", starttime));
            importmessage = string.Format("股票代码：{0}；最新记录的时间：{1}", symbol, starttime);

            messages.Add("\t\t获取待导入数据");
            var originaldata = _dataReader.GetDaylineData(symbol, starttime);
            if (originaldata == null || originaldata.Items.Count == 0)
            {
                messages.Add("\t\t无数据需要导入");
                LogDayLineInfoBatch(messages);
                return importmessage;
            }

            messages.Add(string.Format("\t\t开始导入日线数据{0}条", originaldata.Items.Count));
            dailypriceservice.ImportPriceData(originaldata.Items.Select(item => DataUtil.CopyFrom(item, stock)));
            messages.Add(string.Format("\t\t完成导入日线数据：{0}", symbol));

            LogDayLineInfoBatch(messages);
            return importmessage;
        }

        private void DividendDataImport()
        {
            _divedendLogger.Info("\t导入除权数据");
            ReportProgress(10, "导入除权数据....");

            var symbolqueue = new Queue<string>(_dataReader.DividendSymbols);
            while (symbolqueue.Count > 0)
            {
                var symbol = symbolqueue.Dequeue();
                var task = _taskFactory.StartNew<string>(DividendDataImport, symbol);
                task.ContinueWith(TaskEnd);
            }
        }

        private string DividendDataImport(object state)
        {
            var importmessage = string.Empty;
            if (_tokenSource.IsCancellationRequested)
            {
                return importmessage;
            }

            var symbol = (string)state;
            var messages = new List<string>();

            //var domainservice = CreateDomainService();
            //var stockservice = new ImportStockService(domainservice);
            //var dividenddataservice = new ImportDividendDataService(domainservice);
            var stockservice = new ImportStockService();
            var dividenddataservice = new ImportDividendDataService();

            messages.Add(string.Format("\t处理数据：{0}", symbol));
            var stock = stockservice.GetStockBySymbol(symbol);
            if (stock == null)
            {
                messages.Add("\t\t开始创建股票对象");
                stock = new Stock { Symbol = symbol, Name = string.Empty };
                stockservice.ImportStock(stock);
                messages.Add("\t\t完成创建股票对象");
            }

            var starttime = dividenddataservice.GetLatestDividendDataImportDateTime(symbol);
            messages.Add(string.Format("\t\t获取最新记录的时间：{0}", starttime));
            importmessage = string.Format("股票代码：{0}；最新记录的时间：{1}", symbol, starttime);

            messages.Add("\t\t获取待导入数据");
            var originaldata = _dataReader.GetDividendData(symbol, starttime);
            if (originaldata == null || originaldata.Items.Count == 0)
            {
                messages.Add("\t\t无数据需要导入");
                LogDividendInfoBatch(messages);
                return importmessage;
            }

            messages.Add(string.Format("\t\t开始导入除权数据{0}条", originaldata.Items.Count));
            dividenddataservice.ImportDividendData(originaldata.Items.Select(item => DataUtil.CopyFrom(item, stock)));
            messages.Add(string.Format("\t\t完成导入除权数据：{0}", symbol));

            LogDividendInfoBatch(messages);
            return importmessage;
        }

        //private IImportDomainService CreateDomainService()
        //{
        //    var unitofwork = new StockDataRepositoryContext();
        //    var stockrepository = new StockRepository(unitofwork);
        //    var dailypricedatarepository = new DailyPriceDataItemRepository(unitofwork);
        //    var dividenddatarepository = new DividendDataItemRepository(unitofwork);
        //    return new ImportDomainService(stockrepository, dailypricedatarepository, dividenddatarepository);
        //}

        private void TaskEnd(Task<string> task)
        {
            lock (_incrementLock)
            {
                if (_hasError)
                {
                    return;
                }

                if (task.Exception != null)
                {
                    _hasError = true;
                    _tokenSource.Cancel();
                    ProcessException(task);
                    return;
                }

                _currentCount++;
                if (_currentCount == _stepCount)
                {
                    _otherLogger.Info("导入完成。");
                    ReportProgress(100, "导入完成。");
                    return;
                }
            }

            if (string.IsNullOrEmpty(task.Result))
            {
                return;
            }

            var progress = GetProgress();
            ReportProgress(progress, task.Result);
        }

        private void LogDayLineInfoBatch(IEnumerable<string> messages)
        {
            lock (_dayLineLoggerLock)
            {
                foreach (var message in messages)
                {
                    _dayLineLogger.Info(message);
                }
            }
        }

        private void LogDividendInfoBatch(IEnumerable<string> messages)
        {
            lock (_divedendLoggerLock)
            {
                foreach (var message in messages)
                {
                    _divedendLogger.Info(message);
                }
            }
        }

        private int GetProgress()
        {
            var step = (END - START) / (double)_stepCount;
            return (int)(START + step * _currentCount);
        }

        private void ReportProgress(int progress, string message)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new ProgressArgs { Progress = progress, Message = message });
            }
        }

        private void ProcessException(Task task)
        {
            if (task.Exception == null)
            {
                return;
            }

            var message = task.Exception.Message;
            if (task.Exception.InnerExceptions.Count > 0)
            {
                message = GetExceptionMessage(task.Exception.InnerExceptions[0]);
                for (var i = 1; i < task.Exception.InnerExceptions.Count; i++)
                {
                    message += Environment.NewLine + GetExceptionMessage(task.Exception.InnerExceptions[i]);
                }
            }

            _otherLogger.Error("Task异常", task.Exception);
            ReportProgress(100, string.Format("导入错误：{0} 详见：logs\\Other\\{1}.log", message, DateTime.Now.ToString("yyyyMMdd")));
        }

        public string GetExceptionMessage(Exception exception)
        {
            return exception.InnerException == null ? exception.Message : GetExceptionMessage(exception.InnerException);
        }
    }

    public class ProgressArgs
    {
        public int Progress { get; set; }

        public string Message { get; set; }
    }
}