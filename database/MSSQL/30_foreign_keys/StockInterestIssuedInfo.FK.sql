ALTER TABLE [dbo].[StockInterestIssuedInfo]  WITH CHECK ADD  CONSTRAINT [FK_StockInterestIssuedInfo_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockInterestIssuedInfo] CHECK CONSTRAINT [FK_StockInterestIssuedInfo_Stock]
GO
