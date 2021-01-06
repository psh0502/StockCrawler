/****** Object:  StoredProcedure [dbo].[InsertStockForums] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-12-31
-- Description: collect forums article about stock market
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertStockForums]
@pSource VARCHAR(10),
@pSubject NVARCHAR(50),
@pHash VARCHAR(50),
@pUrl VARCHAR(200),
@pArticleDate DATE,
@oID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SET @oID = 0
	IF NOT EXISTS(SELECT [Hash] FROM [dbo].[StockForums](NOLOCK) WHERE [Hash] = @pHash) BEGIN
		INSERT INTO [dbo].[StockForums]
			([Source]
			,[Subject]
			,[Hash]
			,[Url]
			,[ArticleDate])
		VALUES
			(@pSource
			,@pSubject
			,@pHash
			,@pUrl
			,@pArticleDate)
		SET @oID = SCOPE_IDENTITY()
	END
END
GO
