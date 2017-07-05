using Quartz;
using StockCrawler.Services;
using System;

namespace StockCrawlerRunner
{
    class Program
    {
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
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
                throw;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("You didn't specify crawler mode.");
            Console.WriteLine("Usage:");
            Console.WriteLine("StockCrawlerRunner.exe <mode>");
            Console.WriteLine(" <mode>: -i");
            Console.WriteLine("     Initialize all stock history data. It will drop all existing data.");
            Console.WriteLine(" <mode>: -u");
            Console.WriteLine("     Append the latest price data in database.");
        }
    }
}
