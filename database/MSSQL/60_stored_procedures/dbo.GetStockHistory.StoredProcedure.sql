/****** Object:  StoredProcedure [dbo].[GetStockHistory] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2017-07-15
-- Description:	Get stock history records
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockHistory]
@pStockNo VARCHAR(10),
@pDateBegin DATE,
@pDateEnd DATE,
@pTop INT,
@pCurrentPage INT,
@pPageSize INT,
@oPageCount INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE
		@vFROM_IDX INT,
	    @vTO_IDX INT,
	    @vRowCount INT

	SELECT  @vRowCount = COUNT(1)
	FROM   [dbo].[StockPriceHistory] sph(NOLOCK)
	WHERE
		sph.StockNo = @pStockNo
		AND sph.StockDT BETWEEN @pDateBegin AND @pDateEnd

	IF (@vRowCount > @pTop) SET @vRowCount = @pTop
	
	IF(@pPageSize IS NULL ) SET @pPageSize = 5;
	
	SET @oPageCount =  (@vRowCount / @pPageSize) + (CASE WHEN (@vRowCount % @pPageSize) = 0 THEN 0 ELSE 1 END)
    	
	IF (@pCurrentPage IS NULL) SET @pCurrentPage = @oPageCount;
	SET @vFROM_IDX  =  ((@pCurrentPage - 1) * @pPageSize) + 1;
	SET @vTO_IDX    =   (@pCurrentPage * @pPageSize);	

	SELECT *
	FROM
	(
		SELECT
			TOP (@pTop) *,
			ROW_NUMBER() OVER (ORDER BY sph.StockDT DESC ) [RNO]
		FROM
			[dbo].[StockPriceHistory] sph(NOLOCK)
		WHERE
			sph.StockNo = @pStockNo
			AND sph.StockDT BETWEEN @pDateBegin AND @pDateEnd
	) ResultTable
	WHERE RNO BETWEEN @vFROM_IDX AND @vTO_IDX
	ORDER BY StockDT DESC
END
GO