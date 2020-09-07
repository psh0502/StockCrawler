/****** Object:  StoredProcedure [dbo].[GetStockReportPerMonth] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-09-07
-- Description: Get other stocks report per month
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockReportPerMonth]
@pStockNo VARCHAR(10),
@pYear SMALLINT,
@pMonth SMALLINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT a.StockName, b.*
	FROM [Stock] a(NOLOCK)
		INNER JOIN [dbo].[StockReportPerMonth] b(NOLOCK) ON a.StockNo = b.StockNo
    WHERE
		a.StockNo = @pStockNo
		AND (@pYear = -1 OR b.[Year] = @pYear)
		AND (@pMonth = -1 OR b.[Month] = @pMonth)
		AND a.[Enable] = @TRUE
END
GO
