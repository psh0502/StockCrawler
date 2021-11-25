ALTER TABLE [dbo].[ETFIngredients] WITH CHECK ADD  CONSTRAINT [FK_ETFIngredients_Stock] FOREIGN KEY([StockNo])
REFERENCES [dbo].[Stock] ([StockNo])
GO
ALTER TABLE [dbo].[ETFIngredients] CHECK CONSTRAINT [FK_ETFIngredients_Stock]
GO
ALTER TABLE [dbo].[ETFIngredients] WITH CHECK ADD  CONSTRAINT [FK_ETFIngredients_ETFBasicInfo] FOREIGN KEY([ETFNo])
REFERENCES [dbo].[ETFBasicInfo] ([StockNo])
GO
ALTER TABLE [dbo].[ETFIngredients] CHECK CONSTRAINT [FK_ETFIngredients_ETFBasicInfo]
GO
