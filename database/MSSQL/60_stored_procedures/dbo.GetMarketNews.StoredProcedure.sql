/****** Object:  StoredProcedure [dbo].[GetMarketNews] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-10-28
-- Description: Get Market News
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetMarketNews]
@pTop INT,
@pStartDate DATE,
@pEndDate DATE
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP (@pTop) * 
	FROM [dbo].[MarketNews](NOLOCK)
	WHERE
		NewsDate BETWEEN @pStartDate AND @pEndDate
	ORDER BY 
		NewsDate DESC
		, [Subject] ASC
END
GO
