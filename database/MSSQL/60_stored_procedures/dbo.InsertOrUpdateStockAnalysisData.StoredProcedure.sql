/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockAnalysisData] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-05-25
-- Description: Patch StockAnalysisData information
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockAnalysisData]
@pStockNo VARCHAR(10), 
@pPrice NVARCHAR(50),
@pStockCashDivi DECIMAL(18, 4),
@pDiviRatio NVARCHAR(50),
@pDiviType NVARCHAR(50),
@pIsPromisingEPS BIT,
@pIsGrowingUpEPS BIT,
@pIsAlwaysIncomeEPS BIT,
@pIsAlwaysPayDivi BIT,
@pIsStableDivi BIT,
@pIsAlwaysRestoreDivi BIT,
@pIsStableOutsideIncome BIT,
@pIsStableTotalAmount BIT,
@pIsGrowingUpRevenue BIT,
@pHasDivi BIT,
@pIsRealMode BIT,
@pPrice5 DECIMAL(18, 4),
@pPrice6 DECIMAL(18, 4),
@pPrice7 DECIMAL(18, 4),
@pCurrPrice DECIMAL(18, 4)
AS
BEGIN
	SET NOCOUNT ON
	IF EXISTS(SELECT [StockNo] FROM [dbo].[StockAnalysisData](NOLOCK) WHERE [StockNo] = @pStockNo)
		UPDATE [dbo].[StockAnalysisData]
		SET
			[Price] = ISNULL(@pPrice, [Price])
			,[StockCashDivi] = ISNULL(@pStockCashDivi, [StockCashDivi])
			,[DiviRatio] = ISNULL(@pDiviRatio, [DiviRatio])
			,[DiviType] = ISNULL(@pDiviType, [DiviType])
			,[IsPromisingEPS] = ISNULL(@pIsPromisingEPS, [IsPromisingEPS])
			,[IsGrowingUpEPS] = ISNULL(@pIsGrowingUpEPS, [IsGrowingUpEPS])
			,[IsAlwaysIncomeEPS] = ISNULL(@pIsAlwaysIncomeEPS, [IsAlwaysIncomeEPS])
			,[IsAlwaysPayDivi] = ISNULL(@pIsAlwaysPayDivi, [IsAlwaysPayDivi])
			,[IsStableDivi] = ISNULL(@pIsStableDivi, [IsStableDivi])
			,[IsAlwaysRestoreDivi] = ISNULL(@pIsAlwaysRestoreDivi, [IsAlwaysRestoreDivi])
			,[IsStableOutsideIncome] = ISNULL(@pIsStableOutsideIncome, [IsStableOutsideIncome])
			,[IsStableTotalAmount] = ISNULL(@pIsStableTotalAmount, [IsStableTotalAmount])
			,[IsGrowingUpRevenue] = ISNULL(@pIsGrowingUpRevenue, [IsGrowingUpRevenue])
			,[HasDivi] = ISNULL(@pHasDivi, [HasDivi])
			,[IsRealMode] = ISNULL(@pIsRealMode, [IsRealMode])
			,[Price5] = ISNULL(@pPrice5, [Price5])
			,[Price6] = ISNULL(@pPrice6, [Price6])
			,[Price7] = ISNULL(@pPrice7, [Price7])
			,[CurrPrice] = ISNULL(@pCurrPrice, [CurrPrice])
			,[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo

	ELSE BEGIN
		INSERT INTO [dbo].[StockAnalysisData]
           ([StockNo]
           ,[Price]
           ,[StockCashDivi]
           ,[DiviRatio]
           ,[DiviType]
           ,[IsPromisingEPS]
           ,[IsGrowingUpEPS]
           ,[IsAlwaysIncomeEPS]
           ,[IsAlwaysPayDivi]
           ,[IsStableDivi]
           ,[IsAlwaysRestoreDivi]
           ,[IsStableOutsideIncome]
           ,[IsStableTotalAmount]
           ,[IsGrowingUpRevenue]
           ,[HasDivi]
           ,[IsRealMode]
           ,[Price5]
           ,[Price6]
           ,[Price7]
           ,[CurrPrice])
		VALUES
           (@pStockNo
           ,@pPrice
           ,@pStockCashDivi
           ,@pDiviRatio
           ,@pDiviType
           ,@pIsPromisingEPS
           ,@pIsGrowingUpEPS
           ,@pIsAlwaysIncomeEPS
           ,@pIsAlwaysPayDivi
           ,@pIsStableDivi
           ,@pIsAlwaysRestoreDivi
           ,@pIsStableOutsideIncome
           ,@pIsStableTotalAmount
           ,@pIsGrowingUpRevenue
           ,@pHasDivi
           ,@pIsRealMode
           ,@pPrice5
           ,@pPrice6
           ,@pPrice7
           ,@pCurrPrice)
	END
END
GO
