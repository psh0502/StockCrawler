ALTER TABLE [dbo].[StockMonthlyIncome]  WITH CHECK ADD  CONSTRAINT [FK_StockMonthlyIncome_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockMonthlyIncome] CHECK CONSTRAINT [FK_StockMonthlyIncome_Stock]
GO
