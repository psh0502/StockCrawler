using System.Diagnostics;
using System.IO;
using log4net;
using log4net.Config;

namespace StockCrawler.Services
{
    public abstract class JobBase
    {
        protected readonly ILog _logger = null;
        protected JobBase()
        {
            FileInfo fi = new FileInfo("log4net.config");
            if (fi.Exists) XmlConfigurator.ConfigureAndWatch(fi); else Trace.WriteLine(string.Format("{0} is not existing, use itself config to instead of.", fi.FullName));
            _logger = LogManager.GetLogger(GetType());
        }
    }
}
