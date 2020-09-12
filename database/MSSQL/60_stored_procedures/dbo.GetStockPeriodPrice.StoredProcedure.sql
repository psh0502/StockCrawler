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
@pEndDate DATE,
@pPeriod SMALLINT
AS
BEGIN
	SET NOCOUNT ON
	SELECT TOP (@pPeriod) *
	FROM StockPriceHistory(NOLOCK)
	WHERE StockNo = @pStockNo AND StockDT <= @pEndDate
	ORDER BY StockDT DESC
END
GO