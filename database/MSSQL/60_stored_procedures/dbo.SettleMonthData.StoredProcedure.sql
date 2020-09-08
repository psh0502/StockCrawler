/****** Object:  StoredProcedure [dbo].[SettleMonthData] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-09-08
-- Description: Settle month data
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SettleMonthData]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pMonth SMALLINT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @vAvgEPS MONEY,
		@v20DayAvgClosedPrice MONEY,
		@vPE MONEY,
		@vCalulateDate DATE,
		@vSeason SMALLINT

	SET @vSeason = (@pMonth / 4)
	IF(@vSeason = 0) BEGIN
		SET @pYear -= 1
		SET @vSeason = 4
	END

	SET @vCalulateDate = CONVERT(DATE, CONVERT(VARCHAR, @pYear + 1911))
	SET @vCalulateDate = DATEADD(DAY, -1, DATEADD(MONTH, @pMonth, @vCalulateDate))

	SELECT @vAvgEPS = AVG(EPS) FROM(
		SELECT TOP 4 EPS FROM [dbo].[StockReportPerSeason](NOLOCK) 
		WHERE StockNo = @pStockNo 
			AND [Year] <= @pYear AND (([Year] = @pYear AND Season <= @vSeason) OR [Year] < @pYear)
		ORDER BY [Year], [Season] DESC) t

	SELECT @v20DayAvgClosedPrice = AVG(ClosePrice) FROM(
		SELECT TOP 20 ClosePrice FROM [dbo].[StockPriceHistory](NOLOCK) 
		WHERE StockNo = @pStockNo 
			AND [StockDT] <= @vCalulateDate
		ORDER BY [StockDT] DESC) t

	SET @vPE = @v20DayAvgClosedPrice / @vAvgEPS
	EXEC InsertOrUpdateStockReportPerMonth @pStockNo = @pStockNo, @pYear = @pYear, @pMonth = @pMonth, @pPE = @vPE
END
GO
