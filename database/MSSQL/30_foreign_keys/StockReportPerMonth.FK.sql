ALTER TABLE [dbo].[StockReportPerMonth]  WITH CHECK ADD  CONSTRAINT [FK_StockReportPerMonth_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockReportPerMonth] CHECK CONSTRAINT [FK_StockReportPerMonth_Stock]
GO
