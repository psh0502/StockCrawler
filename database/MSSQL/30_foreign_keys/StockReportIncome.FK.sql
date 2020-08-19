ALTER TABLE [dbo].[StockReportIncome]  WITH CHECK ADD  CONSTRAINT [FK_StockReportIncome_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockReportIncome] CHECK CONSTRAINT [FK_StockReportIncome_Stock]
GO
