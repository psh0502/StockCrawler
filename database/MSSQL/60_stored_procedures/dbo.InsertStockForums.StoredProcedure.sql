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
@pMeta NVARCHAR(500),
@pUrl VARCHAR(200),
@pArticleDate DATE,
@oID BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO [dbo].[StockForums]
		([Source]
		,[Subject]
		,[Meta]
		,[Url]
		,[ArticleDate])
	VALUES
		(@pSource
		,@pSubject
		,@pMeta
		,@pUrl
		,@pArticleDate)
	SET @oID = SCOPE_IDENTITY()
END
GO
