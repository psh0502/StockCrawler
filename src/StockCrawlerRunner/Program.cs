using Common.Logging;
using Quartz;
using StockCrawler.Services;
using System;
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
                            string stockNo = null;
                            if (args.Length > 1)
                                StockCrawler.Services.SystemTime.SetFakeTime(DateTime.Parse(args[1]));
                            if (args.Length > 2)
                                stockNo = args[2];

                            job = new StockPriceHistoryInitJob() { ProcessingStockNo = stockNo };
                            break;
                        case "-u":
                            if (args.Length > 1)
                                StockCrawler.Services.SystemTime.SetFakeTime(DateTime.Parse(args[1]));
                            job = new StockPriceUpdateJob();
                            break;
                        case "-b":
                            stockNo = null;
                            if (args.Length > 1)
                                job = new StockBasicInfoUpdateJob() { BeginStockNo = args[1] };
                            else
                                job = new StockBasicInfoUpdateJob();
                            break;
                        case "-f":
                            job = new StockFinReportUpdateJob();
                            break;
                        case "-n":
                            job = new MarketNewsUpdateJob();
                            break;
                        default:
                            ShowHelp();
                            break;
                    }
                    if (null != job)
                        job.Execute(null);
                    else
                        Console.WriteLine("No job execute.");
                }
                else
                {
                    ShowHelp();
                }
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
            Console.WriteLine("     Initialize all stock history data. It will drop all existing data.");
            Console.WriteLine(" <mode>: -i [Date:yyyy/MM/dd]");
            Console.WriteLine("     Initialize all stock history data by the specified date. It will drop all existing data.");
            Console.WriteLine(" <mode>: -i <Date:yyyy/MM/dd> [stock number]");
            Console.WriteLine("     Initialize only one specified stock history data. It will drop all old data by this stock.");
            Console.WriteLine(" <mode>: -u");
            Console.WriteLine("     Append the latest price data.");
            Console.WriteLine(" <mode>: -u [Date:yyyy/MM/dd]");
            Console.WriteLine("     Append the specified date price data.");
            Console.WriteLine(" <mode>: -b");
            Console.WriteLine("     Update the latest company basic information.");
            Console.WriteLine(" <mode>: -b <stock number>");
            Console.WriteLine("     Update the latest company basic information since the specified stock number.");
            Console.WriteLine(" <mode>: -f");
            Console.WriteLine("     Update the company finance report since 2015 to this year.");
            Console.WriteLine(" <mode>: -n");
            Console.WriteLine("     Get the latest Taiwan stock market news.");
        }
    }
}
