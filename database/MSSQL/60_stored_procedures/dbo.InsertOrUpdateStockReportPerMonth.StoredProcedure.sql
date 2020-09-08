/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockReportPerMonth] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-09-07
-- Description: Settle other report number per month
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockReportPerMonth]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pMonth SMALLINT,
@pPE MONEY
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockReportPerMonth](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Month] = @pMonth)
		UPDATE [StockReportPerMonth]
		SET
			[PE] = ISNULL(@pPE, [PE]),
			[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Month] = @pMonth

	ELSE
		INSERT INTO [dbo].[StockReportPerMonth]
           ([StockNo]
           ,[Year]
           ,[Month]
           ,[PE])
		VALUES
           (@pStockNo
           ,@pYear
           ,@pMonth
           ,ISNULL(@pPE, 0))
END
GO
