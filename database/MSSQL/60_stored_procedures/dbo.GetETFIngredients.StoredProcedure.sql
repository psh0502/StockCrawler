/****** Object:  StoredProcedure [dbo].[GetETFIngredients] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-11-25
-- Description: Get ETF ingredients
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetETFIngredients]
@pETFNo VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT *
	FROM
		[ETFIngredients] a(NOLOCK)
    WHERE
		a.ETFNo = @pETFNo
END
GO
