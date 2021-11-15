/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockTechnicalIndicators] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tom Tang
-- Create date: 2021-11-15
-- Description:	insert or update stock technical values
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockTechnicalIndicators]
@pStockNo VARCHAR(10),
@pStockDT DATE,
@pType VARCHAR(10),
@pValue DECIMAL(18, 4)
AS
BEGIN
	SET NOCOUNT ON
	IF EXISTS(SELECT [StockNo] FROM [StockTechnicalIndicators](NOLOCK) WHERE [StockNo] = @pStockNo AND [StockDT] = @pStockDT AND [Type] = @pType)
		UPDATE [StockTechnicalIndicators]
		SET
			[Value] = ISNULL(@pValue, [Value])
		WHERE [StockNo] = @pStockNo AND [StockDT] = @pStockDT AND [Type] = @pType

	ELSE
		INSERT INTO [StockTechnicalIndicators]
			([StockNo]
			,[StockDT]
			,[Type]
			,[Value])
		VALUES
			(@pStockNo
			,@pStockDT
			,@pType
			,@pValue)
END
GO