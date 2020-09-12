/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockPriceAVG] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2019-09-12
-- Description:	insert or update stock close average price
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockPriceAVG]
@pStockNo VARCHAR(10),
@pStockDT DATE,
@pPeriod SMALLINT,
@pClosePrice MONEY
AS
BEGIN
	SET NOCOUNT ON
	IF EXISTS(SELECT [StockNo] FROM [StockAveragePrice](NOLOCK) WHERE [StockNo] = @pStockNo AND [StockDT] = @pStockDT AND [Period] = @pPeriod)
		UPDATE [StockAveragePrice]
		SET
			[ClosePrice] = ISNULL(@pClosePrice, [ClosePrice])
		WHERE [StockNo] = @pStockNo AND [StockDT] = @pStockDT AND [Period] = @pPeriod

	ELSE
		INSERT INTO [StockAveragePrice]
			([StockNo]
			,[StockDT]
			,[Period]
			,[ClosePrice])
		VALUES
			(@pStockNo
			,@pStockDT
			,@pPeriod
			,@pClosePrice)
END
GO