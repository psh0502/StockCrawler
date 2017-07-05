/****** Object:  StoredProcedure [dbo].[GetStocks]    Script Date: 07/15/2013 20:52:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStocks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStocks]
GO

/****** Object:  StoredProcedure [dbo].[GetStocks] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2017-07-04
-- Description: Get all stocks
-- Revision:
-- =============================================
CREATE PROCEDURE [dbo].[GetStocks]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @TRUE BIT = 1
	SELECT * 
	FROM [Stock]
    WHERE [Enable] = @TRUE;
END
GO
