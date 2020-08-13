ALTER TABLE [dbo].[StockPriceHistory] DROP CONSTRAINT [FK_StockPriceHistory_Stock]
GO
ALTER TABLE [dbo].[StockBasicInfo] DROP CONSTRAINT [FK_StockBasicInfo_Stock]
GO
ALTER TABLE [dbo].[StockReportCashFlow] DROP CONSTRAINT [FK_StockReportCashFlow_Stock]
GO
