ALTER TABLE [dbo].[StockForumRelations]  WITH CHECK ADD  CONSTRAINT [FK_StockForumRelations_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO

ALTER TABLE [dbo].[StockForumRelations] CHECK CONSTRAINT [FK_StockForumRelations_Stock]
GO

ALTER TABLE [dbo].[StockForumRelations]  WITH CHECK ADD  CONSTRAINT [FK_StockForumRelations_StockForums] FOREIGN KEY([ID])
REFERENCES [dbo].[StockForums] ([ID])
GO

ALTER TABLE [dbo].[StockForumRelations] CHECK CONSTRAINT [FK_StockForumRelations_StockForums]
GO
