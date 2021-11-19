using Common.Logging;
using Quartz;
using StockCrawler.Services;
using System;
using System.Linq;
using System.Threading;

namespace StockCrawlerRunner
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("**StockCrawlerRunner**");
                Console.WriteLine(" License: LGPL v3");
                Console.WriteLine(" Author: Tom Tang <tomtang0406@gmail.com>");
                Console.WriteLine("==========================================");
                if (args.Length > 0)
                {
                    IJob job = null;
                    switch (args[0])
                    {
                        case "-i":
                            job = new StockPriceHistoryInitJob();
                            break;
                        case "-u":
                            job = new StockPriceUpdateJob();
                            break;
                        case "-c":
                            job = new StockCaculationJob();
                            break;
                        case "-b":
                            job = new StockBasicInfoUpdateJob();
                            break;
                        case "-f":
                            job = new StockFinReportUpdateJob();
                            break;
                        case "-n":
                            job = new StockMarketNewsUpdateJob();
                            break;
                        case "-ptt":
                            job = new StockForumsUpdateJob();
                            break;
                        case "-is":
                            job = new StockInterestIssuedUpdateJob();
                            break;
                        case "-a":
                            job = new StockAnalysisUpdateJob();
                            break;
                        case "-m":
                            job = new StockMonthlyIncomeUpdateJob();
                            break;
                        default:
                            ShowHelp();
                            break;
                    }
                    var jobContext = new ArgumentJobExecutionContext(job);
                    jobContext.Put("args", args.Skip(1).ToArray());
                    if (null != job)
                        job.Execute(jobContext);
                    else
                        Console.WriteLine("No job execute.");
                }
                else
                    ShowHelp();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                _logger.Info("END");
                Thread.Sleep(500);
                Console.WriteLine("END");
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("You didn't specify crawler mode.");
            Console.WriteLine("Usage:");
            Console.WriteLine("StockCrawlerRunner.exe <mode>");
            Console.WriteLine(" <mode>: -i");
            Console.WriteLine("     Initialize all stock history data. It deletes all existing data.");
            Console.WriteLine(" <mode>: -i [Date:yyyy/MM/dd]");
            Console.WriteLine("     Initialize all stock history data by the specified date. It deletes existing data after the specified date.");
            Console.WriteLine(" <mode>: -i <Date:yyyy/MM/dd> [stock number]");
            Console.WriteLine("     Initialize only one specified stock history data. It deletes existing data with this stock.");
            Console.WriteLine(" <mode>: -u");
            Console.WriteLine("     Append the latest price data.");
            Console.WriteLine(" <mode>: -u [Date:yyyy/MM/dd]");
            Console.WriteLine("     Append the specified date price data.");
            Console.WriteLine(" <mode>: -c [Date:yyyy/MM/dd]");
            Console.WriteLine("     Caculate SMA and Technical Indicators(KD, MACD...etc) from the specified date to today.");
            Console.WriteLine(" <mode>: -c [Date:yyyy/MM/dd] [stock no1] [stock no2]... [stock noX]");
            Console.WriteLine("     Caculate SMA and Technical Indicators(KD, MACD...etc) only for these specified stocks from the specified date to today.");
            Console.WriteLine(" <mode>: -c [Date:yyyy/MM/dd] [stock no1] [stock no2]... [stock noX] [KD|MACD|RSI] [KD|MACD|RSI] [KD|MACD|RSI]");
            Console.WriteLine("     Caculate Technical Indicators(KD, MACD...etc) only for these specified stocks from the specified date to today.");
            Console.WriteLine(" <mode>: -b");
            Console.WriteLine("     Update the latest company basic information.");
            Console.WriteLine(" <mode>: -b <stock number>");
            Console.WriteLine("     Update the latest company basic information since the specified stock number.");
            Console.WriteLine(" <mode>: -f");
            Console.WriteLine("     Update the company finance report this year.");
            Console.WriteLine(" <mode>: -n");
            Console.WriteLine("     Get the latest Taiwan stock market news.");
            Console.WriteLine(" <mode>: -m");
            Console.WriteLine("     Get the latest 12 monthy income data by each stock.");
            Console.WriteLine(" <mode>: -is");
            Console.WriteLine("     Get the latest interest issued result.");
            Console.WriteLine(" <mode>: -a");
            Console.WriteLine("     Analyze stock health according to the latest finance reports.");
            Console.WriteLine(" <mode>: -ptt [Date:yyyy/MM/dd]");
            Console.WriteLine("     Get the latest articles from PTT stock forum. If you assign date, it grab articles by the date.");
        }
    }
}
