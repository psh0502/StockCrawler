/****** Object:  StoredProcedure [dbo].[GetStockAveragePrice] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2019-09-13
-- Description:	Get stock close average price by specified record
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockAveragePrice]
@pStockNo VARCHAR(10),
@pDateBegin DATE,
@pDateEnd DATE,
@pPeriod SMALLINT
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		a.StockName, b.*
	FROM
		Stock a(NOLOCK)
		INNER JOIN StockAveragePrice b(NOLOCK) ON a.StockNo = b.StockNo
	WHERE
		b.StockNo = @pStockNo
		AND b.StockDT BETWEEN @pDateBegin AND @pDateEnd
		AND (@pPeriod = -1 OR b.[Period] = @pPeriod)
END
GO