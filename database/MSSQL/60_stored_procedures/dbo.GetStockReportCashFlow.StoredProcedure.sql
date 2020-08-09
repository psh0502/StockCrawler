/****** Object:  StoredProcedure [dbo].[GetStockReportCashFlow]    Script Date: 07/15/2013 20:52:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStockReportCashFlow]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStockReportCashFlow]
GO

/****** Object:  StoredProcedure [dbo].[GetStockReportCashFlow] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-08-09
-- Description: Get stocks cashflow report
-- Revision:
-- =============================================
CREATE PROCEDURE [dbo].[GetStockReportCashFlow]
@pStockNo VARCHAR(10),
@pYear SMALLINT,
@pSeason SMALLINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT a.StockName, b.*
	FROM [Stock] a(NOLOCK)
		INNER JOIN [dbo].[StockReportCashFlow] b(NOLOCK) ON a.StockNo = b.StockNo
    WHERE
		a.StockNo = @pStockNo
		AND (@pYear = -1 OR b.[Year] = @pYear)
		AND (@pSeason = -1 OR b.[Season] = @pSeason)
		AND a.[Enable] = @TRUE
END
GO
