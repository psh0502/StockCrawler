/****** Object:  StoredProcedure [dbo].[GetLazyStockData] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2020-12-30
-- Description:	Get lazy stock data
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetLazyStockData]
@pStockNo VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [dbo].[LazyStockData](NOLOCK)
	WHERE 
		StockNo = @pStockNo 
END
GO