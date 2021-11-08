ALTER TABLE [dbo].[StockBasicInfo] DROP CONSTRAINT [FK_StockBasicInfo_Stock]
GO
ALTER TABLE [dbo].[StockAveragePrice] DROP CONSTRAINT [FK_StockAveragePrice_Stock]
GO
ALTER TABLE [dbo].[StockFinancialReport] DROP CONSTRAINT [FK_StockFinancialReport_Stock]
GO
ALTER TABLE [dbo].[StockForumRelations] DROP CONSTRAINT [FK_StockForumRelations_Stock]
GO
ALTER TABLE [dbo].[StockForumRelations] DROP CONSTRAINT [FK_StockForumRelations_StockForums]
GO
ALTER TABLE [dbo].[StockInterestIssuedInfo] DROP CONSTRAINT [FK_StockInterestIssuedInfo_Stock]
GO
ALTER TABLE [dbo].[StockMarketNews] DROP CONSTRAINT [FK_StockMarketNews_Stock]
GO
ALTER TABLE [dbo].[StockMonthlyIncome] DROP CONSTRAINT [FK_StockMonthlyIncome_Stock]
GO
ALTER TABLE [dbo].[StockPriceHistory] DROP CONSTRAINT [FK_StockPriceHistory_Stock]
GO

