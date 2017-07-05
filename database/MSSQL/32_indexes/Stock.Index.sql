/****** Object:  Index [IX_StockNo_Unique]    Script Date: 2017/7/3 ¤U¤È 09:16:21 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_StockNo_Unique] ON [dbo].[Stock]
(
	[StockNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ensure StockNo is the only value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Stock', @level2type=N'INDEX',@level2name=N'IX_StockNo_Unique'
GO
