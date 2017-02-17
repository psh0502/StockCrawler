using log4net;

#if(DEBUG)
namespace StockCrawler
{
    public class UnitTestLogger : ILog
    {
        #region ILog Members

        public void Debug(object message, System.Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("[DEBUG] {0}\n{1}", message, exception));
        }

        public void Debug(object message)
        {
            Debug(message, null);
        }

        public void DebugFormat(System.IFormatProvider provider, string format, params object[] args)
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

        public void Error(object message, System.Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("[ERROR] {0}\n{1}", message, exception));
        }

        public void Error(object message)
        {
            Error(message, null);
        }

        public void ErrorFormat(System.IFormatProvider provider, string format, params object[] args)
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

        public void Fatal(object message, System.Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("[FATAL] {0}\n{1}", message, exception));
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

        public void Info(object message, System.Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("[INFO] {0}\n{1}", message, exception));
        }

        public void Info(object message)
        {
            Info(message, null);
        }

        public void InfoFormat(System.IFormatProvider provider, string format, params object[] args)
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

        public bool IsDebugEnabled
        {
            get { return true; }
        }

        public bool IsErrorEnabled
        {
            get { return true; }
        }

        public bool IsFatalEnabled
        {
            get { return true; }
        }

        public bool IsInfoEnabled
        {
            get { return true; }
        }

        public bool IsWarnEnabled
        {
            get { return true; }
        }

        public void Warn(object message, System.Exception exception)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("[WARN] {0}\n{1}", message, exception));
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

        #region ILoggerWrapper Members

        public log4net.Core.ILogger Logger
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}
#endif
