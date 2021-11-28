/****** Object:  Table [dbo].[ETFBasicInfo]    Script Date: 2021/11/25 ******/
DROP TABLE IF EXISTS [dbo].[ETFBasicInfo]
GO

/****** Object:  Table [dbo].[ETFBasicInfo]    Script Date: 2021/11/25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ETFBasicInfo](
	[StockNo] [varchar](10) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[CompanyName][nvarchar](100) NOT NULL,
	[BuildDate][date] NOT NULL,
	[BuildPrice][decimal](10, 6) NOT NULL,
	[PublishDate][date] NOT NULL,
	[PublishPrice][decimal](18, 4) NOT NULL,
	[KeepingBank] [nvarchar](20) NOT NULL,
	[CEO][nvarchar](50) NOT NULL,
	[Url][varchar](100) NOT NULL,
	[Distribution] [bit] NOT NULL DEFAULT(1),
	[ManagementFee] [decimal](10, 6) NOT NULL,
	[KeepFee] [decimal](10, 6) NOT NULL,
	[Business][nvarchar](1000) NOT NULL,
	[TotalAssetNAV] MONEY NOT NULL,
	[NAV] MONEY NOT NULL,
	[TotalPublish] BIGINT NOT NULL,
	[CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
	[LastModifiedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_ETFBasicInfo] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
