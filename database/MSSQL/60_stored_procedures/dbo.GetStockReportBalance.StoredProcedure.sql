/****** Object:  StoredProcedure [dbo].[GetStockReportBalance] Script Date: 2020-08-22 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-08-22
-- Description: Get stocks balance report
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockReportBalance]
@pTop INT,
@pStockNo VARCHAR(10),
@pYear SMALLINT,
@pSeason SMALLINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT TOP(@pTop) a.StockName, b.*
	FROM [dbo].[Stock] a(NOLOCK) 
		INNER JOIN [dbo].[StockReportBalance](NOLOCK) b ON a.StockNo = b.StockNo
	WHERE
		a.StockNo = @pStockNo
		AND (@pYear = -1 OR b.[Year] = @pYear)
		AND (@pSeason = -1 OR b.[Season] = @pSeason)
		AND a.[Enable] = @TRUE
END
GO
