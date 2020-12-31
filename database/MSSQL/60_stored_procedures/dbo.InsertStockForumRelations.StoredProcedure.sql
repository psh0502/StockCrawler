/****** Object:  StoredProcedure [dbo].[InsertStockForumRelations] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-12-31
-- Description: InsertStockForumRelations
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertStockForumRelations]
@pStockNo VARCHAR(10),
@pID BIGINT
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO [dbo].[StockForumRelations]
		([StockNo]
		,[ID])
	VALUES
		(@pStockNo
		,@pID)
END
GO
