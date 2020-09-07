/****** Object:  StoredProcedure [dbo].[InsertStockPriceHistoryData] Script Date: 07/15/2013 20:52:04 ******/
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
CREATE OR ALTER PROCEDURE [dbo].[InsertStockPriceHistoryData]
@pStockNo VARCHAR(20), 
@pStockDT DATE, 
@pOpenPrice DECIMAL(10, 4),
@pHighPrice DECIMAL(10, 4),
@pLowPrice DECIMAL(10, 4),
@pClosePrice DECIMAL(10, 4),
@pVolume BIGINT,
@pAdjClosePrice DECIMAL(10, 4)
AS
BEGIN
	INSERT INTO [StockPriceHistory](
		[StockNo],
        [StockDT],
        [OpenPrice], 
        [HighPrice],
        [LowPrice],
        [ClosePrice],
        [Volume],
        [AdjClosePrice]) 
	VALUES(
		@pStockNo, 
        @pStockDT, 
        @pOpenPrice,
        @pHighPrice,
        @pLowPrice,
        @pClosePrice,
        @pVolume,
        @pAdjClosePrice);

	UPDATE [StockBasicInfo] 
	SET MarketValue = ReleaseStockCount * @pClosePrice 
	WHERE StockNo = @pStockNo
END
GO
