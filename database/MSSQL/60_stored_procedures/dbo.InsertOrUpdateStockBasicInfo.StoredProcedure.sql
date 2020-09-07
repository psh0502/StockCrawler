/****** Object:  StoredProcedure [dbo].[InsertOrUpdateStockBasicInfo] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2020-09-07
-- Description: Patch company information
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateStockBasicInfo]
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
			[Category] = ISNULL(@pCategory, [Category])
			,[CompanyName] = ISNULL(@pCompanyName, [CompanyName])
			,[CompanyID] = ISNULL(@pCompanyID, [CompanyID])
			,[BuildDate] = ISNULL(@pBuildDate, [BuildDate])
			,[PublishDate] = ISNULL(@pPublishDate, [PublishDate])
			,[Capital] = ISNULL(@pCapital, [Capital])
			,[MarketValue] = ISNULL(@pMarketValue, [MarketValue])
			,[ReleaseStockCount] = ISNULL(@pReleaseStockCount, [ReleaseStockCount])
			,[Chairman] = ISNULL(@pChairman, [Chairman])
			,[CEO] = ISNULL(@pCEO, [CEO])
			,[Url] = ISNULL(@pUrl, [Url])
			,[Business] = ISNULL(@pBusiness, [Business])
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
			,[Business])
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
