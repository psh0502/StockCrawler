/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockReportIncome] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-08-19
-- Description: Patch company income report
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockReportIncome]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pSeason SMALLINT,
@pRevenue MONEY,
@pGrossProfit MONEY,
@pSalesExpense MONEY,
@pManagementCost MONEY,
@pRDExpense MONEY,
@pOperatingExpenses MONEY,
@pBusinessInterest MONEY,
@pNetProfitTaxFree MONEY,
@pNetProfitTaxed MONEY,
@pEPS MONEY,
@pSEPS MONEY,
@pRelaseStockCount BIGINT
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockReportIncome](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason)
		UPDATE [StockReportIncome]
		SET
			[Revenue] = @pRevenue,
			[GrossProfit] = @pGrossProfit,
			[SalesExpense] = @pSalesExpense,
			[ManagementCost] = @pManagementCost,
			[RDExpense] = @pRDExpense,
			[OperatingExpenses] = @pOperatingExpenses,
			[BusinessInterest] = @pBusinessInterest,
			[NetProfitTaxFree] = @pNetProfitTaxFree,
			[NetProfitTaxed] = @pNetProfitTaxed,
			[EPS] = @pEPS,
			[SEPS] = @pSEPS,
			[ReleaseStockCount] = @pRelaseStockCount,
			[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason

	ELSE
		INSERT INTO [dbo].[StockReportIncome]
           ([StockNo]
           ,[Year]
           ,[Season]
           ,[Revenue]
           ,[GrossProfit]
           ,[SalesExpense]
           ,[ManagementCost]
           ,[RDExpense]
           ,[OperatingExpenses]
           ,[BusinessInterest]
           ,[NetProfitTaxFree]
           ,[NetProfitTaxed]
		   ,[EPS]
		   ,[SEPS]
		   ,[ReleaseStockCount])
     VALUES
           (@pStockNo
           ,@pYear
           ,@pSeason
           ,@pRevenue
           ,@pGrossProfit
           ,@pSalesExpense
           ,@pManagementCost
           ,@pRDExpense
           ,@pOperatingExpenses
           ,@pBusinessInterest
           ,@pNetProfitTaxFree
           ,@pNetProfitTaxed
		   ,@pEPS
		   ,@pSEPS
		   ,@pRelaseStockCount)
END
GO
