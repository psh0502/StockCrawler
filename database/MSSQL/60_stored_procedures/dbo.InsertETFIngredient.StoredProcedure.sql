/****** Object:  StoredProcedure [dbo].[InsertETFIngredient] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-11-25
-- Description: Insert ETF information
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertETFIngredient]
@pETFNo VARCHAR(10), 
@pStockNo NVARCHAR(10),
@pQuantity BIGINT,
@pWeight DECIMAL(18, 4)
AS
BEGIN
	INSERT INTO [dbo].[ETFIngredients]
		([ETFNo]
		,[StockNo]
		,[Quantity]
		,[Weight])
	VALUES
		(@pETFNo
		,@pStockNo
		,@pQuantity
		,@pWeight)
END
GO
