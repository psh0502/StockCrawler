/****** Object:  StoredProcedure [dbo].[GetStockAnalysisData] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2021-05-25
-- Description:	Get StockAnalysisData
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockAnalysisData]
@pStockNo VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [dbo].[StockAnalysisData](NOLOCK)
	WHERE 
		StockNo = @pStockNo 
END
GO