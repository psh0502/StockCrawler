/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockMonthlyIncome] Script Date: 2021-11-08 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-11-08
-- Description: Patch monthly income
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockMonthlyIncome]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pMonth SMALLINT,
@pIncome MONEY,
@pPreIncome MONEY,
@pDeltaPercent DECIMAL(18, 4),
@pCumMonthIncome MONEY,
@pPreCumMonthIncome MONEY,
@pDeltaCumMonthIncomePercent MONEY
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockMonthlyIncome](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Month] = @pMonth)
		UPDATE [StockMonthlyIncome]
		SET
			[Income] = @pIncome,
			[PreIncome] = @pPreIncome,
			[DeltaPercent] = @pDeltaPercent,
			[CumMonthIncome] = @pCumMonthIncome,
			[PreCumMonthIncome] = @pPreCumMonthIncome,
			[DeltaCumMonthIncomePercent] = @pDeltaCumMonthIncomePercent,
			[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Month] = @pMonth
	ELSE
		INSERT INTO [dbo].[StockMonthlyIncome]
			([StockNo]
			,[Year]
			,[Month]
			,[Income]
			,[PreIncome]
			,[DeltaPercent]
			,[CumMonthIncome]
			,[PreCumMonthIncome]
			,[DeltaCumMonthIncomePercent])
		VALUES
			(@pStockNo
			,@pYear
			,@pMonth
			,@pIncome
			,@pPreIncome
			,@pDeltaPercent
			,@pCumMonthIncome
			,@pPreCumMonthIncome
			,@pDeltaCumMonthIncomePercent)
END
GO
