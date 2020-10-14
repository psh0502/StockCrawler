/****** Object:  StoredProcedure [dbo].[GetCategoryMapping] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-10-14
-- Description: Get Category Mapping
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetCategoryMapping]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * 
	FROM [dbo].[CategoryMapping](NOLOCK)
END
GO
