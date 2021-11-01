/****** Object:  StoredProcedure [dbo].[GetStockBasicInfo] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2017-07-04
-- Description: Get all stocks
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockBasicInfo]
@pStockNo VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	DECLARE @vLatestPrice MONEY
	SET @vLatestPrice = (SELECT TOP 1 ClosePrice FROM StockPriceHistory WHERE StockNo = @pStockNo ORDER BY StockDT DESC)
	SELECT a.StockName, b.*, MarketValue = b.ReleaseStockCount * @vLatestPrice
	FROM [Stock] a(NOLOCK)
		INNER JOIN [StockBasicInfo] b(NOLOCK) ON a.StockNo = b.StockNo
    WHERE
		a.StockNo = @pStockNo
		AND a.[Enable] = @TRUE
END
GO
