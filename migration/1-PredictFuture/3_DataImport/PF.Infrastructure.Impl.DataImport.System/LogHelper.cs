using System;
using log4net;

namespace PF.Infrastructure.Impl.DataImport.System
{
    public class LogHelper
    {
        private readonly ILog _logger;

        public LogHelper(ILog log)
        {
            _logger = log;
        }

        public void Info(object message)
        {
            _logger.Info(message);
        }

        public void Info(object message, Exception e)
        {
            _logger.Info(message, e);
        }

        public void Debug(object message)
        {
            _logger.Debug(message);
        }

        public void Debug(object message, Exception e)
        {
            _logger.Debug(message, e);
        }

        public void Warming(object message)
        {
            _logger.Warn(message);
        }

        public void Warming(object message, Exception e)
        {
            _logger.Warn(message, e);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void Error(object message, Exception e)
        {
            _logger.Error(message, e);
        }

        public void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(object message, Exception e)
        {
            _logger.Fatal(message, e);
        }
    }

    public class LogFactory
    {
        public static LogHelper GetLogger(Type type)
        {
            return new LogHelper(LogManager.GetLogger(type));
        }

        public static LogHelper GetLogger(string str)
        {
            return new LogHelper(LogManager.GetLogger(str));
        }
    }
}