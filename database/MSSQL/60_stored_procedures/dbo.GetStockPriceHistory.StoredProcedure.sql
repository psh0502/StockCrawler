/****** Object:  StoredProcedure [dbo].[GetStockPriceHistory] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2019-09-10
-- Description:	Get stock close price
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockPriceHistory]
@pStockNo VARCHAR(10),
@pBeginDate DATE,
@pEndDate DATE
AS
BEGIN
	SET NOCOUNT ON
	SELECT a.StockName, b.*
	FROM StockPriceHistory b(NOLOCK)
		INNER JOIN Stock a(NOLOCK) ON a.StockNo = b.StockNo 
	WHERE (@pStockNo IS NULL OR @pStockNo = '' OR b.StockNo = @pStockNo)
		AND b.StockDT BETWEEN @pBeginDate AND @pEndDate
	ORDER BY b.StockDT DESC
END
GO