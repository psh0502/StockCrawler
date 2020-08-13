ALTER TABLE [dbo].[StockReportCashFlow]  WITH CHECK ADD  CONSTRAINT [FK_StockReportCashFlow_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockReportCashFlow] CHECK CONSTRAINT [FK_StockReportCashFlow_Stock]
GO
