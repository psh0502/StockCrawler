/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockReportMonthlyNetProfitTaxed] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-08-25
-- Description: Patch company monthly net profit report
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockReportMonthlyNetProfitTaxed]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pMonth SMALLINT,
@pNetProfitTaxed MONEY,
@pLastYearNetProfitTaxed MONEY,
@pDelta MONEY,
@pDeltaPercent DECIMAL(18, 4),
@pThisYearTillThisMonth MONEY,
@pLastYearTillThisMonth MONEY,
@pTillThisMonthDelta MONEY,
@pTillThisMonthDeltaPercent DECIMAL(18, 4),
@pRemark NVARCHAR(1000),
@pPE MONEY
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockReportMonthlyNetProfitTaxed](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Month] = @pMonth)
		UPDATE [StockReportMonthlyNetProfitTaxed]
		SET
			[NetProfitTaxed] = @pNetProfitTaxed
			,[LastYearNetProfitTaxed] = @pLastYearNetProfitTaxed
			,[Delta] = @pDelta
			,[DeltaPercent] = @pDeltaPercent
			,[ThisYearTillThisMonth] = @pThisYearTillThisMonth
			,[LastYearTillThisMonth] = @pLastYearTillThisMonth
			,[TillThisMonthDelta] = @pTillThisMonthDelta
			,[TillThisMonthDeltaPercent] = @pTillThisMonthDeltaPercent
			,[Remark] = @pRemark
			,[PE] = @pPE
			,[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Month] = @pMonth

	ELSE
		INSERT INTO [dbo].[StockReportMonthlyNetProfitTaxed]
           ([StockNo]
           ,[Year]
           ,[Month]
           ,[NetProfitTaxed]
           ,[LastYearNetProfitTaxed]
           ,[Delta]
           ,[DeltaPercent]
           ,[ThisYearTillThisMonth]
           ,[LastYearTillThisMonth]
           ,[TillThisMonthDelta]
           ,[TillThisMonthDeltaPercent]
           ,[Remark]
		   ,[PE])
		VALUES
           (@pStockNo
           ,@pYear
           ,@pMonth
           ,@pNetProfitTaxed
           ,@pLastYearNetProfitTaxed
           ,@pDelta
           ,@pDeltaPercent
           ,@pThisYearTillThisMonth
           ,@pLastYearTillThisMonth
           ,@pTillThisMonthDelta
           ,@pTillThisMonthDeltaPercent
           ,@pRemark
		   ,@pPE)
END
GO
