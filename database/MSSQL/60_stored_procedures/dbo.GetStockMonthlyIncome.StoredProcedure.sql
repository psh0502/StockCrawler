/****** Object:  StoredProcedure [dbo].[GetStockMonthlyIncome] Script Date: 2021-11-08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-11-08
-- Description: Get stocks monthly income
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockMonthlyIncome]
@pTop INT,
@pStockNo VARCHAR(10),
@pYear SMALLINT,
@pMonth SMALLINT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT TOP(@pTop) a.StockName, b.*
	FROM [Stock] a(NOLOCK)
		INNER JOIN [dbo].[StockMonthlyIncome] b(NOLOCK) ON a.StockNo = b.StockNo
    WHERE
		a.StockNo = @pStockNo
		AND (@pYear = -1 OR b.[Year] = @pYear)
		AND (@pMonth = -1 OR b.[Month] = @pMonth)
		AND a.[Enable] = @TRUE
END
GO
