/****** Object:  StoredProcedure [dbo].[CalculateStockPriceAverage] ******/
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
CREATE OR ALTER PROCEDURE [dbo].[CalculateStockPriceAverage]
@pStockNo VARCHAR(10),
@pDateEnd DATE,
@pPeriod SMALLINT,
@oAvgClosePrice MONEY OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		@oAvgClosePrice = AVG(ClosePrice)
	FROM(
		SELECT TOP (@pPeriod) ClosePrice
		FROM StockPriceHistory(NOLOCK)
		WHERE StockNo = @pStockNo AND StockDT <= @pDateEnd
		ORDER BY StockDT DESC
	) t
	SET @oAvgClosePrice = ISNULL(@oAvgClosePrice, 0)
END
GO