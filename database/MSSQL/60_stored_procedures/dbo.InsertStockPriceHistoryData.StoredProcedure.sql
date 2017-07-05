/****** Object:  StoredProcedure [dbo].[InsertStockPriceHistoryData]    Script Date: 07/15/2013 20:52:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertStockPriceHistoryData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertStockPriceHistoryData]
GO

/****** Object:  StoredProcedure [dbo].[InsertStockPriceHistoryData] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2017-07-05
-- Description: insert stock daily trade information
-- Revision:
-- =============================================
CREATE PROCEDURE [dbo].[InsertStockPriceHistoryData]
@pStockID INT, 
@pStockDT DATETIME, 
@pOpenPrice DECIMAL(10, 4),
@pHighPrice DECIMAL(10, 4),
@pLowPrice DECIMAL(10, 4),
@pClosePrice DECIMAL(10, 4),
@pVolume BIGINT,
@pAdjClosePrice DECIMAL(10, 4)
AS
BEGIN
	INSERT INTO [StockPriceHistory](
		[StockID],
        [StockDT],
        [OpenPrice], 
        [HighPrice],
        [LowPrice],
        [ClosePrice],
        [Volume],
        [AdjClosePrice]) 
	VALUES(
		@pStockID, 
        @pStockDT, 
        @pOpenPrice,
        @pHighPrice,
        @pLowPrice,
        @pClosePrice,
        @pVolume,
        @pAdjClosePrice);
END
GO
