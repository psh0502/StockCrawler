/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockReportPerSeason] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-09-07
-- Description: Settle other report number per season
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockReportPerSeason]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pSeason SMALLINT,
@pEPS MONEY,
@pNetValue MONEY
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockReportPerSeason](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason)
		UPDATE [StockReportPerSeason]
		SET
			[EPS] = ISNULL(@pEPS, [EPS]),
			[NetValue] = ISNULL(@pNetValue, [NetValue]),
			[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason

	ELSE
		INSERT INTO [dbo].[StockReportPerSeason]
           ([StockNo]
           ,[Year]
           ,[Season]
           ,[EPS]
           ,[NetValue])
     VALUES
           (@pStockNo
           ,@pYear
           ,@pSeason
           ,@pEPS
           ,@pNetValue)
END
GO
