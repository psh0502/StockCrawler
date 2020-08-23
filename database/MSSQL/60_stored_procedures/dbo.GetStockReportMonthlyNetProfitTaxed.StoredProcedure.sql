/****** Object:  StoredProcedure [dbo].[GetStockReportMonthlyNetProfitTaxed] Script Date: 2020-08-22 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-08-23
-- Description: Get stocks monthly net profit taxed report
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockReportMonthlyNetProfitTaxed]
@pStockNo VARCHAR(10),
@pYear SMALLINT,
@pMonth SMALLINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT a.StockName, *
	FROM [dbo].[Stock] a(NOLOCK) 
		INNER JOIN [dbo].[StockReportMonthlyNetProfitTaxed](NOLOCK) b ON a.StockNo = b.StockNo
	WHERE
		a.StockNo = @pStockNo
		AND (@pYear = -1 OR b.[Year] = @pYear)
		AND (@pMonth = -1 OR b.[Month] = @pMonth)
		AND a.[Enable] = @TRUE
END
GO
