/****** Object:  StoredProcedure [dbo].[InsertOrUpdateMarketNews] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-10-28
-- Description: Insert Or Update Market News
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateMarketNews]
@pNewsDate DATE, 
@pSubject NVARCHAR(100),
@pUrl NVARCHAR(100)
AS
BEGIN
	IF NOT EXISTS(SELECT [NewsDate] FROM [dbo].[MarketNews] WHERE [NewsDate] = @pNewsDate AND [Subject] = @pSubject)
		INSERT INTO [dbo].[MarketNews](
			[NewsDate],
			[Subject],
			[Url])
		VALUES(
			@pNewsDate,
			@pSubject,
			@pUrl);
END
GO
