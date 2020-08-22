/****** Object:  StoredProcedure [dbo].[DisableAllStocks] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2017-07-05
-- Description: disable all stock status
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[DisableAllStocks]
AS
BEGIN
	DECLARE @FALSE BIT = 0
	UPDATE [Stock] SET [Enable] = @FALSE, LastModifiedAt = GETDATE()
END
GO
