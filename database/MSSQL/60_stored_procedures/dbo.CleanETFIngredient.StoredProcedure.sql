/****** Object:  StoredProcedure [dbo].[ClearETFIngredient] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-11-25
-- Description: Clear ETF information
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[ClearETFIngredient]
@pETFNo VARCHAR(10)
AS
BEGIN
	DELETE[dbo].[ETFIngredients]
	WHERE ETFNo = @pETFNo
END
GO
