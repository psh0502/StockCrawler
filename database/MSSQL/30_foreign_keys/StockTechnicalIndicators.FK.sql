ALTER TABLE [dbo].[StockTechnicalIndicators]  WITH CHECK ADD  CONSTRAINT [FK_StockTechnicalIndicators_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockTechnicalIndicators] CHECK CONSTRAINT [FK_StockTechnicalIndicators_Stock]
GO
