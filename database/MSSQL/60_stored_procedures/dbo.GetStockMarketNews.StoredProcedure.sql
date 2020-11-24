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
	SELECT TOP (@pTop) * 
	FROM [dbo].[StockMarketNews](NOLOCK)
	WHERE
		(@pStockNo IS NULL OR @pStockNo = '' OR StockNo = @pStockNo)
		AND(@pSource IS NULL OR @pSource = '' OR [Source] = @pSource)
		AND NewsDate BETWEEN @pStartDate AND @pEndDate
	ORDER BY 
		NewsDate DESC
		, [StockNo]
		, [Source]
		, [Subject] ASC
END
GO
