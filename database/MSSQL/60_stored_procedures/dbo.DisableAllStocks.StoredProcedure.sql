/****** Object:  StoredProcedure [dbo].[DisableAllStocks]    Script Date: 07/15/2013 20:52:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DisableAllStocks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DisableAllStocks]
GO

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
CREATE PROCEDURE [dbo].[DisableAllStocks]
AS
BEGIN
	DECLARE @FALSE BIT = 0
	UPDATE [Stock] SET [Enable] = @FALSE, LastModifiedAt = GETDATE()
END
GO
