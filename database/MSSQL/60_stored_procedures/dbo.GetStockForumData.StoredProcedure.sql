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
@pID BIGINT
AS
BEGIN
	SET NOCOUNT ON
	SELECT *
	FROM [dbo].[StockForums](NOLOCK)
	WHERE 
		[ID] = @pID
END
GO