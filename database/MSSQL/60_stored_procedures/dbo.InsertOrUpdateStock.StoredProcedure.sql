/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStock] Script Date: 07/15/2013 20:52:04 ******/
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
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStock]
@pStockNo VARCHAR(10), 
@pStockName NVARCHAR(50),
@pCategoryNo VARCHAR(10),
@pType SMALLINT
AS
BEGIN
	DECLARE @TRUE BIT = 1
	IF EXISTS(SELECT [StockNo] FROM [Stock] WHERE [StockNo] = @pStockNo)
		UPDATE [Stock] 
		SET [Enable] = @TRUE, 
			StockName = ISNULL(@pStockName, StockName),
			CategoryNo = ISNULL(@pCategoryNo, CategoryNo)
		WHERE [StockNo] = @pStockNo
	ELSE
		INSERT INTO [Stock](
			[StockNo],
			[StockName],
			[CategoryNo],
			[Type],
			[Enable])
		VALUES(
			@pStockNo,
			@pStockName,
			ISNULL(@pCategoryNo, ''),
			@pType,
			@TRUE);
END
GO
