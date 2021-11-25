ALTER TABLE [dbo].[ETFBasicInfo] WITH CHECK ADD  CONSTRAINT [FK_ETFBasicInfo_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[ETFBasicInfo] CHECK CONSTRAINT [FK_ETFBasicInfo_Stock]
GO
