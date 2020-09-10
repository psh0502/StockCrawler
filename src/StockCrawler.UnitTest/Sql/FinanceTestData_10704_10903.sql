-- 此檔案是用來把 DB 資料做成 StockReportCollectorMock 體內寫死的假資料所使用
-- StockReportBalanc 資產負債表
SELECT 'new GetStockReportBalanceResult() { StockNo = "' + [StockNo] + '",Year = ' + CONVERT(VARCHAR, [Year]) + ' , Season = ' + CONVERT(VARCHAR, [Season]) + ', CashAndEquivalents = ' + CONVERT(VARCHAR, [CashAndEquivalents])
+ ', ShortInvestments = ' + CONVERT(VARCHAR, [ShortInvestments]) + ', BillsReceivable = ' + CONVERT(VARCHAR, [BillsReceivable]) + ',Stock = ' + CONVERT(VARCHAR, [Stock]) + ', OtherCurrentAssets = ' + CONVERT(VARCHAR, [OtherCurrentAssets])
+ ', CurrentAssets = ' + CONVERT(VARCHAR, [CurrentAssets]) + ', LongInvestment = ' + CONVERT(VARCHAR, [LongInvestment]) + ', FixedAssets = ' + CONVERT(VARCHAR, [FixedAssets]) + ',OtherAssets = ' + CONVERT(VARCHAR, [OtherAssets])
+ ', TotalAssets = ' + CONVERT(VARCHAR, [TotalAssets]) + ', ShortLoan = ' + CONVERT(VARCHAR, [ShortLoan]) + ', ShortBillsPayable = ' + CONVERT(VARCHAR, [ShortBillsPayable]) + ', AccountsAndBillsPayable = ' + CONVERT(VARCHAR, [AccountsAndBillsPayable])
+ ', AdvenceReceipt = ' + CONVERT(VARCHAR, [AdvenceReceipt]) + ', LongLiabilitiesWithinOneYear = ' + CONVERT(VARCHAR, [LongLiabilitiesWithinOneYear]) + ', OtherCurrentLiabilities = ' + CONVERT(VARCHAR, [OtherCurrentLiabilities])
+ ', CurrentLiabilities = ' + CONVERT(VARCHAR, [CurrentLiabilities]) + ', LongLiabilities = ' + CONVERT(VARCHAR, [LongLiabilities]) + ', OtherLiabilities = ' + CONVERT(VARCHAR, [OtherLiabilities]) + ', TotalLiability = ' + CONVERT(VARCHAR, [TotalLiability])
+ ', NetWorth = ' + CONVERT(VARCHAR, [NetWorth]) + '},'
FROM [Stock].[dbo].[StockReportBalance]
GO
-- 現金流量表
SELECT 'new GetStockReportCashFlowResult() { StockNo = "' + [StockNo] + '", Year = ' + CONVERT(VARCHAR, [Year]) + ' , Season = ' + CONVERT(VARCHAR, [Season]) +
+ ', Depreciation = ' + CONVERT(VARCHAR, [Depreciation])
+ ', AmortizationFee = ' + CONVERT(VARCHAR, [AmortizationFee])
+ ', BusinessCashflow = ' + CONVERT(VARCHAR, [BusinessCashflow])
+ ', InvestmentCashflow = ' + CONVERT(VARCHAR, [InvestmentCashflow])
+ ', FinancingCashflow = ' + CONVERT(VARCHAR, [FinancingCashflow])
+ ', CapitalExpenditures = ' + CONVERT(VARCHAR, [CapitalExpenditures])
+ ', FreeCashflow = ' + CONVERT(VARCHAR, [FreeCashflow])
+ ', NetCashflow = ' + CONVERT(VARCHAR, [NetCashflow]) + ' }.'
FROM [dbo].[StockReportCashFlow]
GO
-- 綜合損益表
SELECT 'new GetStockReportIncomeResult() { StockNo = "' + [StockNo] + '", Year = ' + CONVERT(VARCHAR, [Year]) + ' , Season = ' + CONVERT(VARCHAR, [Season]) +
+ ', Revenue = ' + CONVERT(VARCHAR, [Revenue])
+ ', GrossProfit = ' + CONVERT(VARCHAR, [GrossProfit])
+ ', SalesExpense = ' + CONVERT(VARCHAR, [SalesExpense])
+ ', ManagementCost = ' + CONVERT(VARCHAR, [ManagementCost])
+ ', RDExpense = ' + CONVERT(VARCHAR, [RDExpense])
+ ', OperatingExpenses = ' + CONVERT(VARCHAR, [OperatingExpenses])
+ ', BusinessInterest = ' + CONVERT(VARCHAR, [BusinessInterest])
+ ', NetProfitTaxFree = ' + CONVERT(VARCHAR, [NetProfitTaxFree])
+ ', NetProfitTaxed = ' + CONVERT(VARCHAR, [NetProfitTaxed]) + ' },'
FROM [dbo].[StockReportIncome]
GO
-- 每月營收報告
SELECT 'new GetStockReportMonthlyNetProfitTaxedResult() { StockNo = "' + [StockNo] + '", Year = ' + CONVERT(VARCHAR, [Year]) + ' , Month = ' + CONVERT(VARCHAR, [Month]) +
+ ', NetProfitTaxed = ' + CONVERT(VARCHAR, [NetProfitTaxed])
+ ', LastYearNetProfitTaxed = ' + CONVERT(VARCHAR, [LastYearNetProfitTaxed])
+ ', Delta = ' + CONVERT(VARCHAR, [Delta])
+ ', DeltaPercent = ' + CONVERT(VARCHAR, [DeltaPercent])
+ ', ThisYearTillThisMonth = ' + CONVERT(VARCHAR, [ThisYearTillThisMonth])
+ ', LastYearTillThisMonth = ' + CONVERT(VARCHAR, [LastYearTillThisMonth])
+ ', TillThisMonthDelta = ' + CONVERT(VARCHAR, [TillThisMonthDelta])
+ ', TillThisMonthDeltaPercent = ' + CONVERT(VARCHAR, [TillThisMonthDeltaPercent])
+ ', Remark = "' + [Remark] + '" },'
FROM [dbo].[StockReportMonthlyNetProfitTaxed]
GO






