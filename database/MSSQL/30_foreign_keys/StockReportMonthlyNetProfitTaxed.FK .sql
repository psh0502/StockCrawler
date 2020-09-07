ALTER TABLE [dbo].[StockReportPerSeason]  WITH CHECK ADD  CONSTRAINT [FK_StockReportPerSeason_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockReportPerSeason] CHECK CONSTRAINT [FK_StockReportPerSeason_Stock]
GO
