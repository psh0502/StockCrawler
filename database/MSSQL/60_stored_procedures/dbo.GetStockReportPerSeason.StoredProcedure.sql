/****** Object:  StoredProcedure [dbo].[GetStockReportPerSeason] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-09-07
-- Description: Get other stocks report per season
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockReportPerSeason]
@pStockNo VARCHAR(10),
@pYear SMALLINT,
@pSeason SMALLINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT a.StockName, b.*
	FROM [Stock] a(NOLOCK)
		INNER JOIN [dbo].[StockReportPerSeason] b(NOLOCK) ON a.StockNo = b.StockNo
    WHERE
		a.StockNo = @pStockNo
		AND (@pYear = -1 OR b.[Year] = @pYear)
		AND (@pSeason = -1 OR b.[Season] = @pSeason)
		AND a.[Enable] = @TRUE
END
GO
