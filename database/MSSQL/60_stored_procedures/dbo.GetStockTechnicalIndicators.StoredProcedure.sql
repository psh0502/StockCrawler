/****** Object:  StoredProcedure [dbo].[GetStockTechnicalIndicators] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2021-11-15
-- Description:	Get stock technical indicator values
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockTechnicalIndicators]
@pStockNo VARCHAR(10),
@pDateBegin DATE,
@pDateEnd DATE,
@pType VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
		a.StockName, b.*
	FROM
		Stock a(NOLOCK)
		INNER JOIN StockTechnicalIndicators b(NOLOCK) ON a.StockNo = b.StockNo
	WHERE
		b.StockNo = @pStockNo
		AND b.StockDT BETWEEN @pDateBegin AND @pDateEnd
		AND (@pType IS NULL OR @pType = '' OR b.[Type] = @pType)
END
GO