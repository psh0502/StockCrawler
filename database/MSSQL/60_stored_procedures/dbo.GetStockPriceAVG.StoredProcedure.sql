/****** Object:  StoredProcedure [dbo].[GetStockPriceAVG] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2019-09-10
-- Description:	Get stock close average price by specified record
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockPriceAVG]
@pStockNo VARCHAR(10),
@pDateBegin DATE,
@pDateEnd DATE,
@pPeriod SMALLINT,
@pTop INT,
@oAvgClosePrice MONEY OUTPUT,
@oAvgOpenPrice MONEY OUTPUT,
@oAvgHighPrice MONEY OUTPUT,
@oAvgLowPrice MONEY OUTPUT,
@oTotalVolume BIGINT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		@oAvgClosePrice = AVG(ClosePrice),  
		@oAvgOpenPrice = AVG(OpenPrice), 
		@oAvgHighPrice = AVG(HighPrice), 
		@oAvgLowPrice = AVG(LowPrice),
		@oTotalVolume = SUM(Volume)
	FROM(
		SELECT TOP (@pTop) ClosePrice, OpenPrice, HighPrice, LowPrice, Volume
		FROM StockPriceHistory(NOLOCK)
		WHERE StockNo = @pStockNo AND StockDT BETWEEN @pDateBegin AND @pDateEnd AND [Period] = @pPeriod
		ORDER BY StockDT DESC
	) t
	SET @oAvgClosePrice = ISNULL(@oAvgClosePrice, 0)
	SET @oAvgOpenPrice = ISNULL(@oAvgOpenPrice, 0)
	SET @oAvgHighPrice = ISNULL(@oAvgHighPrice, 0)
	SET @oAvgLowPrice = ISNULL(@oAvgLowPrice, 0)
	SET @oTotalVolume = ISNULL(@oTotalVolume, 0)
END
GO