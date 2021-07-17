/****** Object:  StoredProcedure [dbo].[GetStockMarketNews] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-11-24
-- Description: Get Stock Market News
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockMarketNews]
@pTop INT,
@pStockNo VARCHAR(10),
@pSource VARCHAR(10),
@pStartDate DATE,
@pEndDate DATE
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1;
	SELECT TOP (@pTop) a.* 
	FROM [dbo].[StockMarketNews] a(NOLOCK)
		INNER JOIN [dbo].[Stock] s(NOLOCK) ON s.StockNo = a.StockNo
	WHERE
		s.[Enable] = @TRUE
		AND (@pStockNo IS NULL OR @pStockNo = '' OR a.StockNo = @pStockNo)
		AND (@pSource IS NULL OR @pSource = '' OR [Source] = @pSource)
		AND NewsDate BETWEEN @pStartDate AND @pEndDate
	ORDER BY 
		NewsDate DESC
		, a.[StockNo]
		, [Source]
		, [Subject] ASC
END
GO
