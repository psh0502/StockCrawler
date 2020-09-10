using StockCrawler.Dao;
using StockCrawler.Services.StockFinanceReport;
using System.Collections.Generic;
using System.Linq;

namespace StockCrawler.UnitTest.Mocks
{
    internal class StockReportCollectorMock : IStockReportCollector
    {
        private static readonly List<GetStockReportBalanceResult> _StockBalance;
        private static readonly List<GetStockReportCashFlowResult> _StockCashFlow;
        private static readonly List<GetStockReportIncomeResult> _StockIncome;
        private static readonly List<GetStockReportMonthlyNetProfitTaxedResult> _StockMonthlyNetProfit;
        static StockReportCollectorMock()
        {
            _StockBalance = new List<GetStockReportBalanceResult>
            {
                new GetStockReportBalanceResult() { StockNo = "2330", Year = 109 , Season = 1, CashAndEquivalents = 430777229, ShortInvestments=1254253, BillsReceivable = 146420632,Stock = 78277834, OtherCurrentAssets = 16158252, CurrentAssets = 802470040, LongInvestment = 19381760, FixedAssets = 1438215285,OtherAssets = 83228611, TotalAssets = 2343295696, ShortLoan = 139310384, ShortBillsPayable = 2992858, AccountsAndBillsPayable = 39774214, AdvenceReceipt = 0, LongLiabilitiesWithinOneYear = 0, OtherCurrentLiabilities = 406300536, CurrentLiabilities = 589468059, LongLiabilities = 46475148, OtherLiabilities = 30323958, TotalLiability = 666267165, NetWorth = 1677028531 },
                new GetStockReportBalanceResult() { StockNo = "2330", Year = 108 , Season = 4, CashAndEquivalents = 455399336, ShortInvestments=326839, BillsReceivable = 139770659,Stock = 82981196, OtherCurrentAssets = 16361886, CurrentAssets = 822613914, LongInvestment = 18698788, FixedAssets = 1352377405,OtherAssets = 71114925, TotalAssets = 2264805032, ShortLoan = 118522290, ShortBillsPayable = 0, AccountsAndBillsPayable = 40205966, AdvenceReceipt = 0, LongLiabilitiesWithinOneYear = 0, OtherCurrentLiabilities = 431023298, CurrentLiabilities = 590735701, LongLiabilities = 25100000, OtherLiabilities = 26873905, TotalLiability = 642709606, NetWorth = 1622095426 },
                new GetStockReportBalanceResult() { StockNo = "2330", Year = 108 , Season = 3, CashAndEquivalents = 452430300, ShortInvestments=322089, BillsReceivable = 145421637,Stock = 96685730, OtherCurrentAssets = 22340644, CurrentAssets = 849427436, LongInvestment = 18193969, FixedAssets = 1197955298,OtherAssets = 68657747, TotalAssets = 2134234450, ShortLoan = 85573710, ShortBillsPayable = 0, AccountsAndBillsPayable = 36958217, AdvenceReceipt = 0, LongLiabilitiesWithinOneYear = 0, OtherCurrentLiabilities = 371727435, CurrentLiabilities = 494781125, LongLiabilities = 25100000, OtherLiabilities = 26856324, TotalLiability = 546737449, NetWorth = 1587497001 },
                new GetStockReportBalanceResult() { StockNo = "2330", Year = 108 , Season = 2, CashAndEquivalents = 649697262, ShortInvestments=1322756, BillsReceivable = 116130708,Stock = 108231879, OtherCurrentAssets = 19660171, CurrentAssets = 1010179338, LongInvestment = 17352733, FixedAssets = 1142871184,OtherAssets = 68940416, TotalAssets = 2239343671, ShortLoan = 78261120, ShortBillsPayable = 0, AccountsAndBillsPayable = 32570136, AdvenceReceipt = 0, LongLiabilitiesWithinOneYear = 0, OtherCurrentLiabilities = 511359813, CurrentLiabilities = 622256378, LongLiabilities = 35300000, OtherLiabilities = 27365766, TotalLiability = 684922144, NetWorth = 1554421527 },
                new GetStockReportBalanceResult() { StockNo = "2330", Year = 108 , Season = 1, CashAndEquivalents = 645670527, ShortInvestments=3084399, BillsReceivable = 106740970,Stock = 108682382, OtherCurrentAssets = 15604303, CurrentAssets = 991324815, LongInvestment = 18336458, FixedAssets = 1107651816,OtherAssets = 70123696, TotalAssets = 2187436785, ShortLoan = 76592550, ShortBillsPayable = 0, AccountsAndBillsPayable = 27661850, AdvenceReceipt = 0, LongLiabilitiesWithinOneYear = 0, OtherCurrentLiabilities = 273750781, CurrentLiabilities = 378267634, LongLiabilities = 35300000, OtherLiabilities = 30351549, TotalLiability = 443919183, NetWorth = 1743517602 },
            };
            _StockCashFlow = new List<GetStockReportCashFlowResult>()
            {
                new GetStockReportCashFlowResult() { StockNo = "2330", Year = 109 , Season = 1, Depreciation = 67083741, AmortizationFee = 1470736, BusinessCashflow = 203029442, InvestmentCashflow = -188993268, FinancingCashflow = -40757411, CapitalExpenditures = -192493759, FreeCashflow = 14036174, NetCashflow = -26721237 },
                new GetStockReportCashFlowResult() { StockNo = "2330", Year = 108 , Season = 4, Depreciation = 281411832, AmortizationFee = 5472409, BusinessCashflow = 615138744, InvestmentCashflow = -458801647, FinancingCashflow = -269638166, CapitalExpenditures = -460134832, FreeCashflow = 156337097, NetCashflow = -113301069 },
                new GetStockReportCashFlowResult() { StockNo = "2330", Year = 108 , Season = 3, Depreciation = 215274524, AmortizationFee = 4077932, BusinessCashflow = 412184327, InvestmentCashflow = -287196541, FinancingCashflow = -252455390, CapitalExpenditures = -290194249, FreeCashflow = 124987786, NetCashflow = -127467604 },
                new GetStockReportCashFlowResult() { StockNo = "2330", Year = 108 , Season = 2, Depreciation = 149897048, AmortizationFee = 2699106, BusinessCashflow = 270431306, InvestmentCashflow = -178906424, FinancingCashflow = -23343147, CapitalExpenditures = -192139466, FreeCashflow = 91524882, NetCashflow = 68181735 },
                new GetStockReportCashFlowResult() { StockNo = "2330", Year = 108 , Season = 1, Depreciation = 76192468, AmortizationFee = 1355336, BusinessCashflow = 152670278, InvestmentCashflow = -64188473, FinancingCashflow = -22412645, CapitalExpenditures = -75866976, FreeCashflow = 88481805, NetCashflow = 66069160 },
            };
            _StockIncome = new List<GetStockReportIncomeResult>() 
            {
                new GetStockReportIncomeResult() { StockNo = "2330", Year = 109 , Season = 1, Revenue = 310597183, GrossProfit = 160784181, SalesExpense = 1451102, ManagementCost = 5903061, RDExpense = 24968883, OperatingExpenses = 32323046, BusinessInterest = 128521637, NetProfitTaxFree = 132147178,    NetProfitTaxed = 117062893 },
                new GetStockReportIncomeResult() { StockNo = "2330", Year = 108 , Season = 4, Revenue = 1069985448, GrossProfit = 492698501, SalesExpense = 6348626, ManagementCost = 21737210, RDExpense = 91418746, OperatingExpenses = 119504582, BusinessInterest = 372701090, NetProfitTaxFree = 389845336, NetProfitTaxed = 345343809 },
                new GetStockReportIncomeResult() { StockNo = "2330", Year = 108 , Season = 3, Revenue = 293045439, GrossProfit = 139432161, SalesExpense = 1596829, ManagementCost = 5810048, RDExpense = 23972076, OperatingExpenses = 31378953, BusinessInterest = 107887292, NetProfitTaxFree = 112336271,    NetProfitTaxed = 101102454 },
                new GetStockReportIncomeResult() { StockNo = "2330", Year = 108 , Season = 2, Revenue = 240998475, GrossProfit = 103673230, SalesExpense = 1483004, ManagementCost = 4288263, RDExpense = 21393728, OperatingExpenses = 27164995, BusinessInterest = 76304053, NetProfitTaxFree = 80545440,      NetProfitTaxed = 66775851 },
                new GetStockReportIncomeResult() { StockNo = "2330", Year = 108 , Season = 1, Revenue = 218704469, GrossProfit = 90352125, SalesExpense = 1459973, ManagementCost = 4140729, RDExpense = 20417311, OperatingExpenses = 26018013, BusinessInterest = 64266023, NetProfitTaxFree = 68181652,       NetProfitTaxed = 61387310 },
            };
            _StockMonthlyNetProfit = new List<GetStockReportMonthlyNetProfitTaxedResult>() 
            {
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 109 , Month = 3, NetProfitTaxed = 113519599, LastYearNetProfitTaxed = 79721587, Delta = 33798012, DeltaPercent = 0.4240M, ThisYearTillThisMonth = 310597183, LastYearTillThisMonth = 218704469, TillThisMonthDelta = 91892714, TillThisMonthDeltaPercent = 0.4202M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 109 , Month = 2, NetProfitTaxed = 93394449, LastYearNetProfitTaxed = 60889055, Delta = 32505394, DeltaPercent = 0.5338M, ThisYearTillThisMonth = 197077584, LastYearTillThisMonth = 138982882, TillThisMonthDelta = 58094702, TillThisMonthDeltaPercent = 0.4180M, Remark = "因客戶需求增加所致。" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 109 , Month = 1, NetProfitTaxed = 103683135, LastYearNetProfitTaxed = 78093827, Delta = 25589308, DeltaPercent = 0.3277M, ThisYearTillThisMonth = 103683135, LastYearTillThisMonth = 78093827, TillThisMonthDelta = 25589308, TillThisMonthDeltaPercent = 0.3277M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 12, NetProfitTaxed = 103313138, LastYearNetProfitTaxed = 89830598, Delta = 13482540, DeltaPercent = 0.1501M, ThisYearTillThisMonth = 1069985448, LastYearTillThisMonth = 1031473557, TillThisMonthDelta = 38511891, TillThisMonthDeltaPercent = 0.0373M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 11, NetProfitTaxed = 107884396, LastYearNetProfitTaxed = 98389414, Delta = 9494982, DeltaPercent = 0.0965M, ThisYearTillThisMonth = 966672310, LastYearTillThisMonth = 941642959, TillThisMonthDelta = 25029351, TillThisMonthDeltaPercent = 0.0266M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 10, NetProfitTaxed = 106039531, LastYearNetProfitTaxed = 101550181, Delta = 4489350, DeltaPercent = 0.0442M, ThisYearTillThisMonth = 858787914, LastYearTillThisMonth = 843253545, TillThisMonthDelta = 15534369, TillThisMonthDeltaPercent = 0.0184M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 9, NetProfitTaxed = 102170096, LastYearNetProfitTaxed = 94921920, Delta = 7248176, DeltaPercent = 0.0764M, ThisYearTillThisMonth = 752748383, LastYearTillThisMonth = 741703364, TillThisMonthDelta = 11045019, TillThisMonthDeltaPercent = 0.0149M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 8, NetProfitTaxed = 106117619, LastYearNetProfitTaxed = 91055038, Delta = 15062581, DeltaPercent = 0.1654M, ThisYearTillThisMonth = 650578287, LastYearTillThisMonth = 646781444, TillThisMonthDelta = 3796843, TillThisMonthDeltaPercent = 0.0059M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 7, NetProfitTaxed = 84757724, LastYearNetProfitTaxed = 74370924, Delta = 10386800, DeltaPercent = 0.1397M, ThisYearTillThisMonth = 544460668, LastYearTillThisMonth = 555726406, TillThisMonthDelta = -11265738, TillThisMonthDeltaPercent = -0.0203M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 6, NetProfitTaxed = 85867929, LastYearNetProfitTaxed = 70438298, Delta = 15429631, DeltaPercent = 0.2191M, ThisYearTillThisMonth = 459702944, LastYearTillThisMonth = 481355482, TillThisMonthDelta = -21652538, TillThisMonthDeltaPercent = -0.0450M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 5, NetProfitTaxed = 80436931, LastYearNetProfitTaxed = 80968732, Delta = -531801, DeltaPercent = -0.0066M, ThisYearTillThisMonth = 373835015, LastYearTillThisMonth = 410917184, TillThisMonthDelta = -37082169, TillThisMonthDeltaPercent = -0.0902M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 4, NetProfitTaxed = 74693615, LastYearNetProfitTaxed = 81869781, Delta = -7176166, DeltaPercent = -0.0877M, ThisYearTillThisMonth = 293398084, LastYearTillThisMonth = 329948452, TillThisMonthDelta = -36550368, TillThisMonthDeltaPercent = -0.1108M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 3, NetProfitTaxed = 79721587, LastYearNetProfitTaxed = 103697437, Delta = -23975850, DeltaPercent = -0.2312M, ThisYearTillThisMonth = 218704469, LastYearTillThisMonth = 248078671, TillThisMonthDelta = -29374202, TillThisMonthDeltaPercent = -0.1184M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 2, NetProfitTaxed = 60889055, LastYearNetProfitTaxed = 64640562, Delta = -3751507, DeltaPercent = -0.0580M, ThisYearTillThisMonth = 138982882, LastYearTillThisMonth = 144381234, TillThisMonthDelta = -5398352, TillThisMonthDeltaPercent = -0.0374M, Remark = "" },
                new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "2330", Year = 108 , Month = 1, NetProfitTaxed = 78093827, LastYearNetProfitTaxed = 79740672, Delta = -1646845, DeltaPercent = -0.0207M, ThisYearTillThisMonth = 78093827, LastYearTillThisMonth = 79740672, TillThisMonthDelta = -1646845, TillThisMonthDeltaPercent = -0.0207M, Remark = "" },
            };
        }
        public GetStockReportBalanceResult GetStockReportBalance(string stockNo, short year, short season)
        {
            return _StockBalance.SingleOrDefault(d => d.StockNo == stockNo && d.Year == year && d.Season == season);
        }

        public GetStockReportCashFlowResult GetStockReportCashFlow(string stockNo, short year, short season)
        {
            return _StockCashFlow.SingleOrDefault(d => d.StockNo == stockNo && d.Year == year && d.Season == season);
        }

        public GetStockReportIncomeResult GetStockReportIncome(string stockNo, short year, short season)
        {
            return _StockIncome.SingleOrDefault(d => d.StockNo == stockNo && d.Year == year && d.Season == season);
        }

        public GetStockReportMonthlyNetProfitTaxedResult GetStockReportMonthlyNetProfitTaxed(string stockNo, short year, short month)
        {
            return _StockMonthlyNetProfit.SingleOrDefault(d => d.StockNo == stockNo && d.Year == year && d.Month == month);
        }
    }
}
