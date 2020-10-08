ALTER TABLE [dbo].[StockAveragePrice]  WITH CHECK ADD  CONSTRAINT [FK_StockAveragePrice_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockAveragePrice] CHECK CONSTRAINT [FK_StockAveragePrice_Stock]
GO
