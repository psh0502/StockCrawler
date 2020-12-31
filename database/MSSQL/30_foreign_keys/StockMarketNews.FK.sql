ALTER TABLE [dbo].[StockMarketNews]  WITH CHECK ADD  CONSTRAINT [FK_StockMarketNews_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockMarketNews] CHECK CONSTRAINT [FK_StockMarketNews_Stock]
GO
