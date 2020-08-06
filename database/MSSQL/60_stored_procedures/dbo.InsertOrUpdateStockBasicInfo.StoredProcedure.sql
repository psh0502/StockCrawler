/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockBasicInfo]    Script Date: 07/15/2013 20:52:04 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertOrUpdateStockBasicInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertOrUpdateStockBasicInfo]
GO

/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockBasicInfo] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-08-06
-- Description: Patch company information
-- Revision:
-- =============================================
CREATE PROCEDURE [dbo].[InsertOrUpdateStockBasicInfo]
@pStockNo VARCHAR(10), 
@pCategory NVARCHAR(50),
@pCompanyName NVARCHAR(100),
@pCompanyID VARCHAR(10),
@pBuildDate DATE,
@pPublishDate DATE,
@pCapital MONEY,
@pMarketValue MONEY,
@pReleaseStockCount BIGINT,
@pChairman NVARCHAR(50),
@pCEO NVARCHAR(50),
@pUrl VARCHAR(100),
@pBusiness NVARCHAR(1000)
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [StockBasicInfo](NOLOCK) WHERE [StockNo] = @pStockNo)
		UPDATE [StockBasicInfo]
		SET
			[Category] = @pCategory
			,[CompanyName] = @pCompanyName
			,[CompanyID] = @pCompanyID
			,[BuildDate] = @pBuildDate
			,[PublishDate] = @pPublishDate
			,[Capital] = @pCapital
			,[MarketValue] = @pMarketValue
			,[ReleaseStockCount] = @pReleaseStockCount
			,[Chairman] = @pChairman
			,[CEO] = @pCEO
			,[Url] = @pUrl
			,[Businiess] = @pBusiness
			,[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo

	ELSE
		INSERT INTO [dbo].[StockBasicInfo]
			([StockNo]
			,[Category]
			,[CompanyName]
			,[CompanyID]
			,[BuildDate]
			,[PublishDate]
			,[Capital]
			,[MarketValue]
			,[ReleaseStockCount]
			,[Chairman]
			,[CEO]
			,[Url]
			,[Businiess])
		 VALUES
			(@pStockNo
			,@pCategory
			,@pCompanyName
			,@pCompanyID
			,@pBuildDate
			,@pPublishDate
			,@pCapital
			,@pMarketValue
			,@pReleaseStockCount
			,@pChairman
			,@pCEO
			,@pUrl
			,@pBusiness)
END
GO
