/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockInterestIssuedInfo] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-03-15
-- Description: Patch interest issued info
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockInterestIssuedInfo]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pSeason SMALLINT,
@pProfitCashIssued MONEY,
@pProfitStockIssued MONEY,
@pSsrCashIssued MONEY,
@pSsrStockIssued MONEY,
@pCapitalReserveCashIssued MONEY,
@pCapitalReserveStockIssued MONEY
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockInterestIssuedInfo](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason)
		UPDATE [StockInterestIssuedInfo]
		SET
			[ProfitCashIssued] = @pProfitCashIssued,
			[ProfitStockIssued] = @pProfitStockIssued,
			[SsrCashIssued] = @pSsrCashIssued,
			[SsrStockIssued] = @pSsrStockIssued,
			[CapitalReserveCashIssued] = @pCapitalReserveCashIssued,
			[CapitalReserveStockIssued] = @pCapitalReserveStockIssued,
			[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason
	ELSE
		INSERT INTO [dbo].[StockInterestIssuedInfo]
			([StockNo]
			,[Year]
			,[Season]
			,[ProfitCashIssued]
			,[ProfitStockIssued]
			,[SsrCashIssued]
			,[SsrStockIssued]
			,[CapitalReserveCashIssued]
			,[CapitalReserveStockIssued])
		VALUES
			(@pStockNo
			,@pYear
			,@pSeason
			,@pProfitCashIssued
			,@pProfitStockIssued
			,@pSsrCashIssued
			,@pSsrStockIssued
			,@pCapitalReserveCashIssued
			,@pCapitalReserveStockIssued)
END
GO
