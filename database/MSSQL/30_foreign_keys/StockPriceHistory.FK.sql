ALTER TABLE [dbo].[StockPriceHistory]  WITH CHECK ADD  CONSTRAINT [FK_StockPriceHistory_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockPriceHistory] CHECK CONSTRAINT [FK_StockPriceHistory_Stock]
GO
