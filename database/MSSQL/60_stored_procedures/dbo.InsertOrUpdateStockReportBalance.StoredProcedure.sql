/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockReportBalance] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-08-19
-- Description: Patch company balance report
-- Revision:
-- 2020-9-12, Tom: add NAV
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockReportBalance]
@pStockNo VARCHAR(10), 
@pYear SMALLINT,
@pSeason SMALLINT,
@pCashAndEquivalents MONEY,
@pShortInvestments MONEY,
@pBillsReceivable MONEY,
@pStock MONEY,
@pOtherCurrentAssets MONEY,
@pCurrentAssets MONEY,
@pLongInvestment MONEY,
@pFixedAssets MONEY,
@pOtherAssets MONEY,
@pTotalAssets MONEY,
@pShortLoan MONEY,
@pShortBillsPayable MONEY,
@pAccountsAndBillsPayable MONEY,
@pAdvenceReceipt MONEY,
@pLongLiabilitiesWithinOneYear MONEY,
@pOtherCurrentLiabilities MONEY,
@pCurrentLiabilities MONEY,
@pLongLiabilities MONEY,
@pOtherLiabilities MONEY,
@pTotalLiability MONEY,
@pNetWorth MONEY,
@pNAV MONEY
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockReportBalance](NOLOCK) WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason)
		UPDATE [StockReportBalance]
		SET
           CashAndEquivalents			   = @pCashAndEquivalents
           ,[ShortInvestments]			   = @pShortInvestments
           ,[BillsReceivable]			   = @pBillsReceivable 
           ,[Stock]						   = @pStock 
           ,[OtherCurrentAssets]		   = @pOtherCurrentAssets 
           ,[CurrentAssets]				   = @pCurrentAssets 
           ,[LongInvestment]			   = @pLongInvestment 
           ,[FixedAssets]				   = @pFixedAssets 
           ,[OtherAssets]				   = @pOtherAssets 
           ,[TotalAssets]				   = @pTotalAssets 
           ,[ShortLoan]					   = @pShortLoan 
           ,[ShortBillsPayable]			   = @pShortBillsPayable 
           ,[AccountsAndBillsPayable]	   = @pAccountsAndBillsPayable 
           ,[AdvenceReceipt]			   = @pAdvenceReceipt 
           ,[LongLiabilitiesWithinOneYear] = @pLongLiabilitiesWithinOneYear 
           ,[OtherCurrentLiabilities]	   = @pOtherCurrentLiabilities 
           ,[CurrentLiabilities]		   = @pCurrentLiabilities 
           ,[LongLiabilities]			   = @pLongLiabilities 
           ,[OtherLiabilities]			   = @pOtherLiabilities 
           ,[TotalLiability]			   = @pTotalLiability 
           ,[NetWorth]					   = @pNetWorth
		   ,[NAV]						   = @pNAV
		   ,[LastModifiedAt] = GETDATE() 
		WHERE [StockNo] = @pStockNo AND [Year] = @pYear AND [Season] = @pSeason

	ELSE
		INSERT INTO [dbo].[StockReportBalance]
           ([StockNo]
           ,[Year]
           ,[Season]
           ,[CashAndEquivalents]
           ,[ShortInvestments]
           ,[BillsReceivable]
           ,[Stock]
           ,[OtherCurrentAssets]
           ,[CurrentAssets]
           ,[LongInvestment]
           ,[FixedAssets]
           ,[OtherAssets]
           ,[TotalAssets]
           ,[ShortLoan]
           ,[ShortBillsPayable]
           ,[AccountsAndBillsPayable]
           ,[AdvenceReceipt]
           ,[LongLiabilitiesWithinOneYear]
           ,[OtherCurrentLiabilities]
           ,[CurrentLiabilities]
           ,[LongLiabilities]
           ,[OtherLiabilities]
           ,[TotalLiability]
           ,[NetWorth]
		   ,[NAV])
		VALUES
           (@pStockNo
           ,@pYear
           ,@pSeason
           ,@pCashAndEquivalents
           ,@pShortInvestments
           ,@pBillsReceivable
           ,@pStock
           ,@pOtherCurrentAssets
           ,@pCurrentAssets
           ,@pLongInvestment
           ,@pFixedAssets
           ,@pOtherAssets
           ,@pTotalAssets
           ,@pShortLoan
           ,@pShortBillsPayable
           ,@pAccountsAndBillsPayable
           ,@pAdvenceReceipt
           ,@pLongLiabilitiesWithinOneYear
           ,@pOtherCurrentLiabilities
           ,@pCurrentLiabilities
           ,@pLongLiabilities
           ,@pOtherLiabilities
           ,@pTotalLiability
           ,@pNetWorth
		   ,@pNAV)
END
GO
