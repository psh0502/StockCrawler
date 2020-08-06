/****** Object:  StoredProcedure [dbo].[GetStockBasicInfo]    Script Date: 07/15/2013 20:52:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStockBasicInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStockBasicInfo]
GO

/****** Object:  StoredProcedure [dbo].[GetStockBasicInfo] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2017-07-04
-- Description: Get all stocks
-- Revision:
-- =============================================
CREATE PROCEDURE [dbo].[GetStockBasicInfo]
@pStockNo VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT a.StockNo, a.StockName, b.*
	FROM [Stock] a(NOLOCK)
		LEFT JOIN [StockBasicInfo] b(NOLOCK) ON a.StockNo = b.StockNo
    WHERE
		a.StockNo = @pStockNo
		AND a.[Enable] = @TRUE
END
GO
