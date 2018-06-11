using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syste.Work.Log
{
    public class NLogLogger : ILogger
    {
        #region 变量
        private Logger source;
        #endregion

        #region 构造函数
        public NLogLogger()
        {
            source = LogManager.GetCurrentClassLogger();
        }

        public NLogLogger(string appsettingString)
        {
            source = LogManager.GetLogger(appsettingString);
        }
        #endregion

        #region Debug
        public void Debug(object item)
        {
            if (item != null)
            {
                source.Log(LogLevel.Trace, item.ToString());
            }
        }

        public void Debug(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Debug, messageInfo);
            }
        }

        public void Debug(string message, Exception exception, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message)
                &&
                exception != null)
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);

                var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception
                source.Log(LogLevel.Debug, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageInfo, exceptionData));
            }
        }
        #endregion

        #region LogInfo
        public void Info(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Info, messageInfo);
            }
        }
        #endregion

        #region LogWarning
        public void Warning(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Warn, messageInfo);
            }
        }
        #endregion

        #region LogError
        public void Error(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Error, messageInfo);
            }
        }

        public void Error(string message, Exception exception, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message)
                &&
                exception != null)
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);

                var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception

                source.Log(LogLevel.Info, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageInfo, exceptionData));
            }
        }
        #endregion

        #region Fatal
        public void Fatal(string message, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);
                source.Log(LogLevel.Fatal, messageInfo);
            }
        }

        public void Fatal(string message, Exception exception, params object[] args)
        {
            if (!String.IsNullOrWhiteSpace(message)
                &&
                exception != null)
            {
                var messageInfo = string.Format(CultureInfo.InvariantCulture, message, args);

                var exceptionData = exception.ToString(); // The ToString() create a string representation of the current exception
                source.Log(LogLevel.Fatal, string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageInfo, exceptionData));
            }
        }
        #endregion
    }
}
