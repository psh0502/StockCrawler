ALTER TABLE [dbo].[StockReportBalance]  WITH CHECK ADD  CONSTRAINT [FK_StockReportBalance_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockReportBalance] CHECK CONSTRAINT [FK_StockReportBalance_Stock]
GO
