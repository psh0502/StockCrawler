ALTER TABLE [dbo].[StockPriceHistory] DROP CONSTRAINT [FK_StockPriceHistory_Stock]
GO
ALTER TABLE [dbo].[StockBasicInfo] DROP CONSTRAINT [FK_StockBasicInfo_Stock]
GO
ALTER TABLE [dbo].[StockReportCashFlow] DROP CONSTRAINT [FK_StockReportCashFlow_Stock]
GO
ALTER TABLE [dbo].[StockReportIncome] DROP CONSTRAINT [FK_StockReportIncome_Stock]
GO
ALTER TABLE [dbo].[StockReportBalance] DROP CONSTRAINT [FK_StockReportBalance_Stock]
GO
ALTER TABLE [dbo].[StockReportMonthlyNetProfitTaxed] DROP CONSTRAINT [FK_StockReportMonthlyNetProfitTaxed_Stock]
GO

