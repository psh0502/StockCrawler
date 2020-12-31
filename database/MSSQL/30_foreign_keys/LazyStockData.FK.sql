ALTER TABLE [dbo].[LazyStockData]  WITH CHECK ADD  CONSTRAINT [FK_LazyStockData_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[LazyStockData] CHECK CONSTRAINT [FK_LazyStockData_Stock]
GO
