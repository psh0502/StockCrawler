ALTER TABLE [dbo].[StockReportMonthlyNetProfitTaxed]  WITH CHECK ADD  CONSTRAINT [FK_StockReportMonthlyNetProfitTaxed_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockReportMonthlyNetProfitTaxed] CHECK CONSTRAINT [FK_StockReportMonthlyNetProfitTaxed_Stock]
GO
