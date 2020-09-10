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
@oMostEarlyDataDate DATE OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT @oAvgClosePrice = AVG(ClosePrice) FROM(
		SELECT TOP (@pTop) ClosePrice FROM StockPriceHistory(NOLOCK)
		WHERE StockNo = @pStockNo AND StockDT BETWEEN @pDateBegin AND @pDateEnd AND [Period] = @pPeriod
		ORDER BY StockDT DESC
	) t
	SET @oAvgClosePrice = ISNULL(@oAvgClosePrice, 0)
	SELECT TOP 1 @oMostEarlyDataDate = StockDT FROM StockPriceHistory(NOLOCK)
END
GO