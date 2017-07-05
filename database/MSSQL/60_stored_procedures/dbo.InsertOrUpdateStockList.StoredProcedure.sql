/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockList]    Script Date: 07/15/2013 20:52:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateStockList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertOrUpdateStockList]
GO

/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockList] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2017-07-05
-- Description: Enable stock status or insert a new one
-- Revision:
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrUpdateStockList]
@pStockNo VARCHAR(10), 
@pStockName VARCHAR(50)
AS
BEGIN
	DECLARE @TRUE BIT = 1
	IF EXISTS(SELECT [StockID] FROM [Stock] WHERE [StockNo] = @pStockNo)
		UPDATE [Stock] SET [Enable] = @TRUE WHERE [StockNo] = @pStockNo
	ELSE
		INSERT INTO [Stock](
			[StockNo],
			[StockName])
		VALUES(
			@pStockNo,
			@pStockName);
END
GO
