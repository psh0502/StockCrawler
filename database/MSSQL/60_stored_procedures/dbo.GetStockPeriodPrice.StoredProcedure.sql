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
@pBeginDate DATE,
@pEndDate DATE
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM StockPriceHistory(NOLOCK)
	WHERE StockNo = @pStockNo AND StockDT BETWEEN @pBeginDate AND @pEndDate
	ORDER BY StockDT DESC
END
GO