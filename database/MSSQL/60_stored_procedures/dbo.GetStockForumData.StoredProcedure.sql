/****** Object:  StoredProcedure [dbo].[GetStockForumData] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2020-12-31
-- Description:	Get stock forum data
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetStockForumData]
@pID BIGINT,
@pStockNo VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON
	SELECT c.StockName, a.*
	FROM [dbo].[StockForums] a(NOLOCK)
		INNER JOIN [dbo].[StockForumRelations] b(NOLOCK) ON a.ID = b.ID
		INNER JOIN [dbo].[Stock] c(NOLOCK) ON b.StockNo = c.StockNo
	WHERE 
		(@pID IS NULL OR a.[ID] = @pID)
		AND (@pStockNo IS NULL OR @pStockNo = '' OR c.StockNo = @pStockNo)
END
GO