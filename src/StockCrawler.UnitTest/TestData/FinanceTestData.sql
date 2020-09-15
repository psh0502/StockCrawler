-- 此檔案是用來把 DB 資料做成 StockReportCollectorMock 體內寫死的假資料所使用
-- StockReportBalanc 資產負債表
SELECT 'new GetStockReportBalanceResult() { StockNo = "' + [StockNo] + '", Year = ' + CONVERT(VARCHAR, [Year]) + ' , Season = ' + CONVERT(VARCHAR, [Season])
+ ', CashAndEquivalents = ' + CONVERT(VARCHAR, [CashAndEquivalents]) + 'M'
+ ', ShortInvestments = ' + CONVERT(VARCHAR, [ShortInvestments]) + 'M'
+ ', BillsReceivable = ' + CONVERT(VARCHAR, [BillsReceivable])  + 'M'
+ ', Stock = ' + CONVERT(VARCHAR, [Stock])  + 'M'
+ ', OtherCurrentAssets = ' + CONVERT(VARCHAR, [OtherCurrentAssets]) + 'M'
+ ', CurrentAssets = ' + CONVERT(VARCHAR, [CurrentAssets])  + 'M'
+ ', LongInvestment = ' + CONVERT(VARCHAR, [LongInvestment])  + 'M'
+ ', FixedAssets = ' + CONVERT(VARCHAR, [FixedAssets])  + 'M'
+ ', OtherAssets = ' + CONVERT(VARCHAR, [OtherAssets]) + 'M'
+ ', TotalAssets = ' + CONVERT(VARCHAR, [TotalAssets])  + 'M'
+ ', ShortLoan = ' + CONVERT(VARCHAR, [ShortLoan])  + 'M'
+ ', ShortBillsPayable = ' + CONVERT(VARCHAR, [ShortBillsPayable])  + 'M'
+ ', AccountsAndBillsPayable = ' + CONVERT(VARCHAR, [AccountsAndBillsPayable]) + 'M'
+ ', AdvenceReceipt = ' + CONVERT(VARCHAR, [AdvenceReceipt])  + 'M'
+ ', LongLiabilitiesWithinOneYear = ' + CONVERT(VARCHAR, [LongLiabilitiesWithinOneYear])  + 'M'
+ ', OtherCurrentLiabilities = ' + CONVERT(VARCHAR, [OtherCurrentLiabilities]) + 'M'
+ ', CurrentLiabilities = ' + CONVERT(VARCHAR, [CurrentLiabilities])  + 'M'
+ ', LongLiabilities = ' + CONVERT(VARCHAR, [LongLiabilities])  + 'M'
+ ', OtherLiabilities = ' + CONVERT(VARCHAR, [OtherLiabilities])  + 'M'
+ ', TotalLiability = ' + CONVERT(VARCHAR, [TotalLiability]) + 'M'
+ ', NetWorth = ' + CONVERT(VARCHAR, [NetWorth]) + 'M' + ' },'
FROM [Stock].[dbo].[StockReportBalance]
GO
-- 現金流量表
SELECT 'new GetStockReportCashFlowResult() { StockNo = "' + [StockNo] + '", Year = ' + CONVERT(VARCHAR, [Year]) + ' , Season = ' + CONVERT(VARCHAR, [Season]) +
+ ', Depreciation = ' + CONVERT(VARCHAR, [Depreciation]) + 'M'
+ ', AmortizationFee = ' + CONVERT(VARCHAR, [AmortizationFee]) + 'M'
+ ', BusinessCashflow = ' + CONVERT(VARCHAR, [BusinessCashflow]) + 'M'
+ ', InvestmentCashflow = ' + CONVERT(VARCHAR, [InvestmentCashflow]) + 'M'
+ ', FinancingCashflow = ' + CONVERT(VARCHAR, [FinancingCashflow]) + 'M'
+ ', CapitalExpenditures = ' + CONVERT(VARCHAR, [CapitalExpenditures]) + 'M'
+ ', FreeCashflow = ' + CONVERT(VARCHAR, [FreeCashflow]) + 'M'
+ ', NetCashflow = ' + CONVERT(VARCHAR, [NetCashflow]) + 'M' + ' },'
FROM [dbo].[StockReportCashFlow]
GO
-- 綜合損益表
SELECT 'new GetStockReportIncomeResult() { StockNo = "' + [StockNo] + '", Year = ' + CONVERT(VARCHAR, [Year]) + ' , Season = ' + CONVERT(VARCHAR, [Season]) +
+ ', Revenue = ' + CONVERT(VARCHAR, [Revenue]) + 'M'
+ ', GrossProfit = ' + CONVERT(VARCHAR, [GrossProfit]) + 'M'
+ ', SalesExpense = ' + CONVERT(VARCHAR, [SalesExpense]) + 'M'
+ ', ManagementCost = ' + CONVERT(VARCHAR, [ManagementCost]) + 'M'
+ ', RDExpense = ' + CONVERT(VARCHAR, [RDExpense]) + 'M'
+ ', OperatingExpenses = ' + CONVERT(VARCHAR, [OperatingExpenses]) + 'M'
+ ', BusinessInterest = ' + CONVERT(VARCHAR, [BusinessInterest]) + 'M'
+ ', NetProfitTaxFree = ' + CONVERT(VARCHAR, [NetProfitTaxFree]) + 'M'
+ ', NetProfitTaxed = ' + CONVERT(VARCHAR, [NetProfitTaxed]) + 'M'
+ ', EPS = ' + CONVERT(VARCHAR, [EPS]) + 'M' + ' },'
FROM [dbo].[StockReportIncome]
GO
-- 每月營收報告
SELECT 'new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "' + [StockNo] + '", Year = ' + CONVERT(VARCHAR, [Year]) + ' , Month = ' + CONVERT(VARCHAR, [Month]) +
+ ', NetProfitTaxed = ' + CONVERT(VARCHAR, [NetProfitTaxed]) + 'M'
+ ', LastYearNetProfitTaxed = ' + CONVERT(VARCHAR, [LastYearNetProfitTaxed]) + 'M'
+ ', Delta = ' + CONVERT(VARCHAR, [Delta]) + 'M'
+ ', DeltaPercent = ' + CONVERT(VARCHAR, [DeltaPercent]) + 'M'
+ ', ThisYearTillThisMonth = ' + CONVERT(VARCHAR, [ThisYearTillThisMonth]) + 'M'
+ ', LastYearTillThisMonth = ' + CONVERT(VARCHAR, [LastYearTillThisMonth]) + 'M'
+ ', TillThisMonthDelta = ' + CONVERT(VARCHAR, [TillThisMonthDelta]) + 'M'
+ ', TillThisMonthDeltaPercent = ' + CONVERT(VARCHAR, [TillThisMonthDeltaPercent]) + 'M'
+ ', Remark = "' + [Remark] + '" },'
FROM [dbo].[StockReportMonthlyNetProfitTaxed]
GO
