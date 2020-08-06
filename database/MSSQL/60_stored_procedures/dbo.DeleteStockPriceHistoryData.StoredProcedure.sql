/****** Object:  StoredProcedure [dbo].[DeleteStockPriceHistoryData]    Script Date: 07/15/2013 20:52:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteStockPriceHistoryData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteStockPriceHistoryData]
GO

/****** Object:  StoredProcedure [dbo].[DeleteStockPriceHistoryData] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2017-07-04
-- Description: Delete all historica data by the specified stock or trade date
-- Revision:
-- =============================================
CREATE PROCEDURE [dbo].[DeleteStockPriceHistoryData]
@pStockNo VARCHAR(10), 
@pTradeDate DATE
AS
BEGIN
	DELETE FROM [StockPriceHistory]
    WHERE
		(@pStockNo IS NULL OR @pStockNo = '' OR StockNo = @pStockNo)
		AND (@pTradeDate IS NULL OR [StockDT] = @pTradeDate);
END
GO
