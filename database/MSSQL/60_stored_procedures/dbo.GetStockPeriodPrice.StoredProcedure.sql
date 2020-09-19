/****** Object:  StoredProcedure [dbo].[GetStockPeriodPrice] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2019-09-10
-- Description:	Get stock close price by specified period
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockPeriodPrice]
@pStockNo VARCHAR(10),
@pPeriod SMALLINT,
@pBeginDate DATE,
@pEndDate DATE
AS
BEGIN
	SET NOCOUNT ON
	SELECT a.StockName, b.*
	FROM StockPriceHistory b(NOLOCK)
		INNER JOIN Stock a(NOLOCK) ON a.StockNo = b.StockNo 
	WHERE b.StockNo = @pStockNo 
		AND b.StockDT BETWEEN @pBeginDate AND @pEndDate
		AND b.[Period] = @pPeriod
	ORDER BY b.StockDT DESC
END
GO