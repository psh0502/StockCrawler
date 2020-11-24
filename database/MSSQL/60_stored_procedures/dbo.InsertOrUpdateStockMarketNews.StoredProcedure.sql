/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockMarketNews] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-11-24
-- Description: IInsertOrUpdateStockMarketNews
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockMarketNews]
@pStockNo VARCHAR(10),
@pSource VARCHAR(10),
@pNewsDate DATE, 
@pSubject NVARCHAR(100),
@pUrl NVARCHAR(100)
AS
BEGIN
	IF NOT EXISTS(SELECT [NewsDate] FROM [dbo].[StockMarketNews] WHERE [StockNo] = @pStockNo AND [Source] = @pSource AND [NewsDate] = @pNewsDate AND [Subject] = @pSubject)
		INSERT INTO [dbo].[StockMarketNews](
			[StockNo],
			[Source],
			[NewsDate],
			[Subject],
			[Url])
		VALUES(
			@pStockNo,
			@pSource,
			@pNewsDate,
			@pSubject,
			@pUrl);
END
GO
