ALTER TABLE [dbo].[StockPriceHistory]  WITH CHECK ADD  CONSTRAINT [FK_StockPriceHistory_Stock] FOREIGN KEY([StockID])
REFERENCES [dbo].[Stock] ([StockID])
GO

ALTER TABLE [dbo].[StockPriceHistory] CHECK CONSTRAINT [FK_StockPriceHistory_Stock]
GO
