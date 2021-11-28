/****** Object:  StoredProcedure [dbo].[InsertOrUpdateETFBasicInfo] Script Date: 07/15/2013 20:52:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Tom Tang
-- Create date: 2021-11-25
-- Description: Patch ETF information
-- Revision:
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdateETFBasicInfo]
@pStockNo VARCHAR(10), 
@pCategory NVARCHAR(50),
@pCompanyName NVARCHAR(100),
@pBuildDate DATE,
@pBuildPrice MONEY,
@pPublishDate DATE,
@pPublishPrice MONEY,
@pKeepingBank NVARCHAR(20),
@pCEO NVARCHAR(50),
@pUrl VARCHAR(100),
@pDistribution BIT,
@pManagementFee MONEY,
@pKeepFee MONEY,
@pBusiness NVARCHAR(1000),
@pTotalAssetNAV MONEY,
@pNAV MONEY,
@pTotalPublish BIGINT
AS
BEGIN
	IF EXISTS(SELECT [StockNo] FROM [ETFBasicInfo](NOLOCK) WHERE [StockNo] = @pStockNo)
		UPDATE [ETFBasicInfo]
		SET
			[Category] = ISNULL(@pCategory, [Category])
			,[CompanyName] = ISNULL(@pCompanyName, [CompanyName])
			,[BuildDate] = ISNULL(@pBuildDate, [BuildDate])
			,[BuildPrice] = ISNULL(@pBuildPrice, [BuildPrice])
			,[PublishDate] = ISNULL(@pPublishDate, [PublishDate])
			,[PublishPrice] = ISNULL(@pPublishPrice, [PublishPrice])
			,[KeepingBank] = ISNULL(@pKeepingBank, [KeepingBank])
			,[CEO] = ISNULL(@pCEO, [CEO])
			,[Url] = ISNULL(@pUrl, [Url])
			,[Distribution] = ISNULL(@pDistribution, [Distribution])
			,[ManagementFee] = ISNULL(@pManagementFee, [ManagementFee])
			,[KeepFee] = ISNULL(@pKeepFee, [KeepFee])
			,[Business] = ISNULL(@pBusiness, [Business])
			,[TotalAssetNAV] = ISNULL(@pTotalAssetNAV, [TotalAssetNAV])
			,[NAV] = ISNULL(@pNAV, [NAV])
			,[TotalPublish] = ISNULL(@pTotalPublish, [TotalPublish])
			,[LastModifiedAt] = GETDATE()
		WHERE [StockNo] = @pStockNo

	ELSE BEGIN
		INSERT INTO [dbo].[ETFBasicInfo]
			([StockNo]
			,[Category]
			,[CompanyName]
			,[BuildDate]
			,[BuildPrice]
			,[PublishDate]
			,[PublishPrice]
			,[CEO]
			,[Url]
			,[Distribution]
			,[ManagementFee]
			,[KeepFee]
			,[Business]
			,[TotalAssetNAV]
			,[NAV]
			,[TotalPublish])
		 VALUES
			(@pStockNo
			,@pCategory
			,@pCompanyName
			,@pBuildDate
			,@pBuildPrice
			,@pPublishDate
			,@pPublishPrice
			,@pCEO
			,@pUrl
			,@pDistribution
			,@pManagementFee
			,@pKeepFee
			,@pBusiness
			,@pTotalAssetNAV
			,@pNAV
			,@pTotalPublish)
	END
END
GO
