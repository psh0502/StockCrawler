/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockReportCashFlow] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-08-09
-- Description: Patch company cashflow report
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockReportCashFlow]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pSeason SMALLINT,
@pDepreciation MONEY,
@pAmortizationFee MONEY,
@pBusinessCashflow MONEY,
@pInvestmentCashflow MONEY,
@pFinancingCashflow MONEY,
@pCapitalExpenditures MONEY,
@pFreeCashflow MONEY,
@pNetCashflow MONEY
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockReportCashFlow](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason)
		UPDATE [StockReportCashFlow]
		SET
			[Depreciation] = @pDepreciation
			,[AmortizationFee] = @pAmortizationFee
			,[BusinessCashflow] = @pBusinessCashflow
			,[InvestmentCashflow] = @pInvestmentCashflow
			,[FinancingCashflow] = @pFinancingCashflow
			,[CapitalExpenditures] = @pCapitalExpenditures
			,[FreeCashflow] = @pFreeCashflow
			,[NetCashflow] = @pNetCashflow
			,[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason

	ELSE
	INSERT INTO [dbo].[StockReportCashFlow]
           ([StockNo]
           ,[Year]
           ,[Season]
           ,[Depreciation]
           ,[AmortizationFee]
           ,[BusinessCashflow]
           ,[InvestmentCashflow]
           ,[FinancingCashflow]
           ,[CapitalExpenditures]
           ,[FreeCashflow]
           ,[NetCashflow])
     VALUES
           (@pStockNo
           ,@pYear
           ,@pSeason
           ,@pDepreciation
           ,@pAmortizationFee
           ,@pBusinessCashflow
           ,@pInvestmentCashflow
           ,@pFinancingCashflow
           ,@pCapitalExpenditures
           ,@pFreeCashflow
           ,@pNetCashflow)
END
GO
