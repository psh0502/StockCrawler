/****** Object:  StoredProcedure [dbo].[GetETFBasicInfo] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-11-25
-- Description: Get one ETF
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetETFBasicInfo]
@pStockNo VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT a.StockName, b.*
	FROM [Stock] a(NOLOCK)
		INNER JOIN [ETFBasicInfo] b(NOLOCK) ON a.StockNo = b.StockNo
    WHERE
		a.StockNo = @pStockNo
		AND a.[Enable] = @TRUE
END
GO
