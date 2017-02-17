using log4net;
using log4net.Config;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace StockCrawler.Services.StockDailyPrice
{
#if(DEBUG)
    public abstract class StockHtmlInfoCollectorBase
#else
    internal abstract class StockHtmlInfoCollectorBase
#endif
    {
        protected static readonly ILog _logger = null;
        static StockHtmlInfoCollectorBase()
        {
            FileInfo fi = new FileInfo("log4net.config");
            if (fi.Exists) XmlConfigurator.ConfigureAndWatch(fi); else Trace.WriteLine(string.Format("{0} is not existing, use itself config to instead of.", fi.FullName));
#if(DEBUG)
            _logger = new UnitTestLogger();
#else
            _logger = LogManager.GetLogger(typeof(StockHtmlInfoCollectorBase));
#endif
        }

        public virtual string GetHtmlText(string url)
        {
            string rtnText = null;
            using (WebClient wc = new WebClient()) rtnText = wc.DownloadString(url);
            return rtnText;
        }

        protected static string StripHTML(string htmlString, char replacedChar)
        {
            //This pattern Matches everything found inside html tags;
            //(.|\n) - > Look for any character or a new line
            // *?  -> 0 or more occurences, and make a non-greedy search meaning
            //That the match will stop at the first available '>' it sees, and not at the last one
            //(if it stopped at the last one we could have overlooked
            //nested HTML tags inside a bigger HTML tag..)
            // Thanks to Oisin and Hugh Brown for helping on this one...

            string pattern = @"<(.|\n)*?>";

            return Regex.Replace(htmlString, pattern, replacedChar.ToString());
        }
#if(DEBUG)
        public static string MergeTabChar(string input)
#else
        protected static string MergeTabChar(string input)
#endif
        {
            CharStateEnum state = CharStateEnum.Normal;
            int merge_bgn = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                switch (state)
                {
                    case CharStateEnum.Normal:
                        if ('\t' == c)
                        {
                            state = CharStateEnum.FoundTab;
                            if (i > merge_bgn) sb.Append(input.Substring(merge_bgn, i - merge_bgn));
                            merge_bgn = i + 1;
                        }
                        break;
                    case CharStateEnum.FoundTab:
                        if ('\t' == c || ' ' == c)
                        {
                            merge_bgn = i + 1;
                        }
                        else
                        {
                            if (sb.Length > 0) sb.Append('\t');
                            merge_bgn = i;
                            state = CharStateEnum.Normal;
                        }
                        break;
                }
            }
            if (input.Length > merge_bgn)
                sb.Append(input.Substring(merge_bgn, input.Length - merge_bgn));

            return sb.ToString();
        }

        private enum CharStateEnum
        {
            Normal,
            FoundTab
        }
    }
}
