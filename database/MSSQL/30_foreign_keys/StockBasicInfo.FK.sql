ALTER TABLE [dbo].[StockBasicInfo]  WITH CHECK ADD  CONSTRAINT [FK_StockBasicInfo_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockBasicInfo] CHECK CONSTRAINT [FK_StockBasicInfo_Stock]
GO
