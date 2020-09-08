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
		@vNetWorth MONEY,
		@vNetValue MONEY, 
		@vSeasonNetProfit MONEY,
		@vEPS MONEY
	SELECT @vTotalReleaseStockCount = ReleaseStockCount FROM [dbo].[StockBasicInfo](NOLOCK) WHERE StockNo = @pStockNo
	IF (@vTotalReleaseStockCount > 0) BEGIN
		SELECT @vNetWorth = [NetWorth] * 1000
		FROM [dbo].[StockReportBalance](NOLOCK) 
		WHERE StockNo = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason

		SET @vNetValue = @vNetWorth / @vTotalReleaseStockCount

		SELECT @vSeasonNetProfit = [NetProfitTaxed] * 1000
		FROM [dbo].[StockReportIncome](NOLOCK)
		WHERE StockNo = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason

		SET @vEPS = @vSeasonNetProfit / @vTotalReleaseStockCount

		EXEC [dbo].[InsertOrUpdateStockReportPerSeason] @pStockNo = @pStockNo, @pYear = @pYear, @pSeason = @pSeason, 
			@pEPS = @vEPS, @pNetValue = @vNetValue
	END
END
GO
