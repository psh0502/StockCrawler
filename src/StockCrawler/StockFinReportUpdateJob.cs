﻿using Common.Logging;
using Quartz;
using StockCrawler.Dao;
using StockCrawler.Services.StockFinanceReport;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace StockCrawler.Services
{
    public class StockFinReportUpdateJob : JobBase, IJob
    {
        internal static ILog Logger { get; set; } = LogManager.GetLogger(typeof(StockBasicInfoUpdateJob));

        public StockFinReportUpdateJob()
            : base()
        {
            if (null == Logger)
                Logger = LogManager.GetLogger(typeof(StockBasicInfoUpdateJob));
        }

        public StockFinReportUpdateJob(string collectorTypeName)
            : this()
        {
            CollectorTypeName = collectorTypeName;
        }

        public string CollectorTypeName { get; private set; }
        public short BeginYear { get; set; } = -1;

        #region IJob Members

        public void Execute(IJobExecutionContext context)
        {
            Logger.InfoFormat("Invoke [{0}]...", MethodBase.GetCurrentMethod().Name);
            try
            {
                using (var db = StockDataServiceProvider.GetServiceInstance())
                {
                    var collector = string.IsNullOrEmpty(CollectorTypeName) ? CollectorProviderService.GetFinanceReportCashFlowCollector() : CollectorProviderService.GetFinanceReportCashFlowCollector(CollectorTypeName);
                    foreach (var d in db.GetStocks().Where(d => !d.StockNo.StartsWith("0") && (int.TryParse(d.StockNo.Substring(0, 4), out _)))) // 排除非公司的基金型股票
                    {
                        short now_year = GetTaiwanYear();
                        short now_season = GetSeason();
                        short now_month = (short)SystemTime.Today.Month;
                        short season = (short)(now_season - 1); // 抓上一季報告
                        short year = now_year;
                        

                        if (season <= 0) { season = 4; year -= 1; }
                        
                        // 若外部指定特定起始年, 則以該起始年第一季開始抓取資料
                        if (BeginYear > 100)
                        {
                            year = BeginYear;
                            season = 1;
                        }

                        for (; year <= now_year; year++)
                        {
                            // 若循覽已到今年, 則季分不該尋找超過當季的資料
                            for (; season <= 4 && !(year == now_year && season == now_season); season++)
                            {
                                if (!GetCashflowIntoDatabase(db, collector, d.StockNo, year, season)) break;
                                if (!GetIncomeIntoDatabase(db, collector, d.StockNo, year, season)) break;
                                if (!GetBalanceIntoDatabase(db, collector, d.StockNo, year, season)) break;
                            }
                            season = 1;
                        }

                        year = now_year;
                        short month = (short)(now_month - 1); // 抓上月報告;
                        if (BeginYear > 100)
                        {
                            year = BeginYear;
                            month = 1;
                        }
                        if (month <= 0) { month = 1; year -= 1; }
                        for (; year <= now_year; year++)
                            for (; month <= 12 && !(year == now_year && month == now_month); month++)
                                if (!GetMonthlyNetProfitTaxedIntoDatabase(db, collector, d.StockNo, year, month)) break;

                        Thread.Sleep(1 * 1000); // Don't get target website pissed off...
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Job executing failed!", ex);
                throw;
            }
        }

        private static bool GetMonthlyNetProfitTaxedIntoDatabase(IStockDataService db, IStockReportCollector collector, string stockNo, short year, short month)
        {
            var info = collector.GetStockReportMonthlyNetProfitTaxed(stockNo, year, month);
            if (null != info)
            {
                db.UpdateStockMonthlyNetProfitTaxedReport(info);
                Logger.InfoFormat("[{0}] get its monthly net profit report(year={1}/month={2})", stockNo, year, month);
                return true;
            }
            else
            {
                Logger.InfoFormat("[{0}] has no monthly net profit report(year={1}/month={2})", stockNo, year, month);
                return false;
            }
        }

        private static bool GetIncomeIntoDatabase(IStockDataService db, IStockReportCollector collector, string stockNo, short year, short season)
        {
            var info = collector.GetStockReportIncome(stockNo, year, season);
            if (null != info)
            {
                db.UpdateStockIncomeReport(info);
                Logger.InfoFormat("[{0}] get its income report(year={1}/season={2})", stockNo, year, season);
                return true;
            }
            else
            {
                Logger.InfoFormat("[{0}] has no income report(year={1}/season={2})", stockNo, year, season);
                return false;
            }
        }

        private static bool GetCashflowIntoDatabase(IStockDataService db, IStockReportCollector collector, string stockNo, short year, short season)
        {
            var info = collector.GetStockReportCashFlow(stockNo, year, season);
            if (null != info)
            {
                db.UpdateStockCashflowReport(info);
                Logger.InfoFormat("[{0}] get its cashflow report(year={1}/season={2})", stockNo, year, season);
                return true;
            }
            else
            {
                Logger.InfoFormat("[{0}] has no cashflow report(year={1}/season={2})", stockNo, year, season);
                return false;
            }
        }
        private static bool GetBalanceIntoDatabase(IStockDataService db, IStockReportCollector collector, string stockNo, short year, short season)
        {
            var info = collector.GetStockReportBalance(stockNo, year, season);
            if (null != info)
            {
                db.UpdateStockBalanceReport(info);
                Logger.InfoFormat("[{0}] get its balance report(year={1}/season={2})", stockNo, year, season);
                return true;
            }
            else
            {
                Logger.InfoFormat("[{0}] has no balance report(year={1}/season={2})", stockNo, year, season);
                return false;
            }
        }

        private static short GetTaiwanYear()
        {
            return (short)(SystemTime.Today.Year - 1911);
        }
        private static short GetSeason()
        {
            var month = SystemTime.Today.Month;
            return (short)(month / 3 + 1);
        }
        #endregion
    }
}