using Common.Logging;
using StockCrawler.Services;
using System;

namespace StockCrawler.UnitTest
{
    public class UnitTestLogger : ILog
    {
        #region ILog Members

        public void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
                System.Diagnostics.Debug.WriteLine($"[DEBUG] [{SystemTime.Now.ToDateTimeText()}] {message}{((exception == null) ? "" : "\r\n")}{exception}");
        }

        public void Debug(object message)
        {
            Debug(message, null);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Debug(string.Format(format, args), null);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Debug(string.Format(format, arg0, arg1, arg2), null);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            Debug(string.Format(format, arg0, arg1), null);
        }

        public void DebugFormat(string format, object arg0)
        {
            Debug(string.Format(format, arg0), null);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Debug(string.Format(format, args), null);
        }

        public void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
                System.Diagnostics.Debug.WriteLine(string.Format("[ERROR] [{2}] {0}\n{1}", message, exception, DateTime.Now).Trim());
        }

        public void Error(object message)
        {
            Error(message, null);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Error(string.Format(format, args), null);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Error(string.Format(format, arg0, arg1, arg2), null);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            Error(string.Format(format, arg0, arg1), null);
        }

        public void ErrorFormat(string format, object arg0)
        {
            Error(string.Format(format, arg0), null);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            Error(string.Format(format, args), null);
        }

        public void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
                System.Diagnostics.Debug.WriteLine(string.Format("[FATAL] [{2}] {0}\n{1}", message, exception, DateTime.Now).Trim());
        }

        public void Fatal(object message)
        {
            Fatal(message, null);
        }

        public void FatalFormat(System.IFormatProvider provider, string format, params object[] args)
        {
            Fatal(string.Format(format, args), null);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Fatal(string.Format(format, arg0, arg1, arg2), null);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            Fatal(string.Format(format, arg0, arg1), null);
        }

        public void FatalFormat(string format, object arg0)
        {
            Fatal(string.Format(format, arg0), null);
        }

        public void FatalFormat(string format, params object[] args)
        {
            Fatal(string.Format(format, args), null);
        }

        public void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
                System.Diagnostics.Debug.WriteLine(string.Format("[INFO] [{2}] {0}\n{1}", message, exception, DateTime.Now).Trim());
        }

        public void Info(object message)
        {
            Info(message, null);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Info(string.Format(format, args), null);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Info(string.Format(format, arg0, arg1, arg2), null);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            Info(string.Format(format, arg0, arg1), null);
        }

        public void InfoFormat(string format, object arg0)
        {
            Info(string.Format(format, arg0), null);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Info(string.Format(format, args), null);
        }

        public bool IsDebugEnabled { get; set; } = true;

        public bool IsErrorEnabled { get; set; } = true;

        public bool IsFatalEnabled { get; set; } = true;

        public bool IsInfoEnabled { get; set; } = true;

        public bool IsWarnEnabled { get; set; } = true;

        public void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
                System.Diagnostics.Debug.WriteLine(string.Format("[WARN] [{2}] {0}\n{1}", message, exception, DateTime.Now).Trim());
        }

        public void Warn(object message)
        {
            Warn(message, null);
        }

        public void WarnFormat(System.IFormatProvider provider, string format, params object[] args)
        {
            Warn(string.Format(format, args), null);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Warn(string.Format(format, arg0, arg1, arg2), null);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            Warn(string.Format(format, arg0, arg1), null);
        }

        public void WarnFormat(string format, object arg0)
        {
            Warn(string.Format(format, arg0), null);
        }

        public void WarnFormat(string format, params object[] args)
        {
            Warn(string.Format(format, args), null);
        }

        #endregion

        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Debug(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void Debug(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Debug(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Error(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void Error(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Error(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Fatal(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void Fatal(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Fatal(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public IVariablesContext GlobalVariablesContext
        {
            get { throw new NotImplementedException(); }
        }
        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Info(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void Info(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Info(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public bool IsTraceEnabled { get; set; }
        public IVariablesContext ThreadVariablesContext
        {
            get { throw new NotImplementedException(); }
        }

        public INestedVariablesContext NestedThreadVariablesContext => throw new NotImplementedException();

        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Trace(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void Trace(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Trace(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void Trace(object message, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Trace(object message)
        {
            throw new NotImplementedException();
        }
        public void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void TraceFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void TraceFormat(string format, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Warn(IFormatProvider formatProvider, Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void Warn(Action<FormatMessageHandler> formatMessageCallback, Exception exception)
        {
            throw new NotImplementedException();
        }
        public void Warn(Action<FormatMessageHandler> formatMessageCallback)
        {
            throw new NotImplementedException();
        }
        public void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
