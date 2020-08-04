using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services;
using System;
using System.Linq;

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
                            int stock_id = -1;
                            if (args.Length > 1)
                            {
                                var stock = StockDataService.GetServiceInstance().GetStocks()
                                    .Where(d => d.StockNo == args[1]).FirstOrDefault();

                                if (null != stock)
                                    stock_id = stock.StockID;
                            }
                            job = new StockPriceHistoryInitJob() { ProcessingStockID = stock_id };
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
            Console.WriteLine(" <mode>: -i [StockNo]");
            Console.WriteLine("     Initialize only one specified stock history data. It will drop all old data by this stock.");
            Console.WriteLine(" <mode>: -u");
            Console.WriteLine("     Append the latest price data in database.");
        }
    }
}
