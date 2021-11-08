ALTER TABLE [dbo].[StockFinancialReport]  WITH CHECK ADD  CONSTRAINT [FK_StockFinancialReport_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockFinancialReport] CHECK CONSTRAINT [FK_StockFinancialReport_Stock]
GO
