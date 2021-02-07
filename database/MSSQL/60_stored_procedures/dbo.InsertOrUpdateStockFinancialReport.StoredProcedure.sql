/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockFinancialReport] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-02-07
-- Description: Patch financial report
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockFinancialReport]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pSeason SMALLINT,
@pTotalAssets MONEY,
@pTotalLiability MONEY,
@pNetWorth MONEY,
@pNAV MONEY,
@pRevenue MONEY,
@pBusinessInterest MONEY,
@pNetProfitTaxFree MONEY,
@pEPS MONEY,
@pBusinessCashflow MONEY,
@pInvestmentCashflow MONEY,
@pFinancingCashflow MONEY
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockFinancialReport](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason)
		UPDATE [StockFinancialReport]
		SET
			[TotalAssets] = @pTotalAssets,
			[TotalLiability] = @pTotalLiability,
			[NetWorth] = @pNetWorth,
			[NAV] = @pNAV,
			[Revenue] = @pRevenue,
			[BusinessInterest] = @pBusinessInterest,
			[NetProfitTaxFree] = @pNetProfitTaxFree,
			[EPS] = @pEPS,
			[BusinessCashflow] = @pBusinessCashflow,
			[InvestmentCashflow] = @pInvestmentCashflow,
			[FinancingCashflow] = @pFinancingCashflow,
			[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason
	ELSE
		INSERT INTO [dbo].[StockFinancialReport]
			([StockNo]
			,[Year]
			,[Season]
			,[TotalAssets]
			,[TotalLiability]
			,[NetWorth]
			,[NAV]
			,[Revenue]
			,[BusinessInterest]
			,[NetProfitTaxFree]
			,[EPS]
			,[BusinessCashflow]
			,[InvestmentCashflow]
			,[FinancingCashflow])
		VALUES
			(@pStockNo
			,@pYear
			,@pSeason
			,@pTotalAssets
			,@pTotalLiability
			,@pNetWorth
			,@pNAV
			,@pRevenue
			,@pBusinessInterest
			,@pNetProfitTaxFree
			,@pEPS
			,@pBusinessCashflow
			,@pInvestmentCashflow
			,@pFinancingCashflow)
END
GO
