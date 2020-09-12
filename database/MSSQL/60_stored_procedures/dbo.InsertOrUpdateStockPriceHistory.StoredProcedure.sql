/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockPriceHistory] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2017-07-05
-- Description: insert stock daily trade information
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockPriceHistory]
@pStockNo VARCHAR(20), 
@pStockDT DATE, 
@pPeriod SMALLINT,
@pOpenPrice DECIMAL(10, 4),
@pHighPrice DECIMAL(10, 4),
@pLowPrice DECIMAL(10, 4),
@pClosePrice DECIMAL(10, 4),
@pVolume BIGINT,
@pAdjClosePrice DECIMAL(10, 4)
AS
BEGIN
	DECLARE @DAILY_PERIOD SMALLINT = 1

	IF EXISTS(SELECT [StockNo] FROM [StockPriceHistory](NOLOCK) WHERE [StockNo] = @pStockNo AND [StockDT] = @pStockDT AND [Period] = @pPeriod)
		UPDATE [StockPriceHistory]
		SET
			[OpenPrice] = ISNULL(@pOpenPrice, [OpenPrice])
			,[HighPrice] = ISNULL(@pHighPrice, [HighPrice])
			,[LowPrice] = ISNULL(@pLowPrice, [LowPrice])
			,[ClosePrice] = ISNULL(@pClosePrice, [ClosePrice])
			,[Volume] = ISNULL(@pVolume, [Volume])
			,[AdjClosePrice] = ISNULL(@pAdjClosePrice, [AdjClosePrice])
		WHERE [StockNo] = @pStockNo AND [StockDT] = @pStockDT AND [Period] = @pPeriod

	ELSE
		INSERT INTO [StockPriceHistory](
			[StockNo],
			[StockDT],
			[Period],
			[OpenPrice], 
			[HighPrice],
			[LowPrice],
			[ClosePrice],
			[Volume],
			[AdjClosePrice]) 
		VALUES(
			@pStockNo, 
			@pStockDT, 
			@pPeriod,
			@pOpenPrice,
			@pHighPrice,
			@pLowPrice,
			@pClosePrice,
			@pVolume,
			@pAdjClosePrice)

	IF (@pPeriod = @DAILY_PERIOD)
		UPDATE [StockBasicInfo] 
		SET MarketValue = ReleaseStockCount * @pClosePrice 
		WHERE StockNo = @pStockNo
END
GO
