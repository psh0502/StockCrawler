/****** Object:  StoredProcedure [dbo].[SettleSeasonData] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-09-08
-- Description: Settle season data
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SettleSeasonData]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pSeason SMALLINT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @vTotalReleaseStockCount BIGINT, 
		@vTotalAssets MONEY,
		@vNetValue MONEY, 
		@vSeasonNetProfit MONEY,
		@vEPS MONEY
	SELECT @vTotalReleaseStockCount = 0 FROM [dbo].[StockBasicInfo](NOLOCK) WHERE StockNo = @pStockNo
	IF (@vTotalReleaseStockCount > 0) BEGIN
		SELECT @vTotalAssets = TotalAssets * 1000
		FROM [dbo].[StockReportBalance](NOLOCK) 
		WHERE StockNo = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason

		IF(@vTotalAssets > 0)BEGIN
			SET @vNetValue = @vTotalAssets / @vTotalReleaseStockCount
		END

		SELECT @vSeasonNetProfit = [NetProfitTaxed]
		FROM [dbo].[StockReportIncome](NOLOCK)
		WHERE StockNo = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason

		IF(@vSeasonNetProfit > 0)BEGIN
			SET @vEPS = @vSeasonNetProfit / @vTotalReleaseStockCount
		END

		EXEC [dbo].[InsertOrUpdateStockReportPerSeason] @pStockNo = @pStockNo, @pYear = @pYear, @pSeason = @pSeason, 
			@pEPS = @vEPS, @pNetValue = @vNetValue
	END
END
GO
