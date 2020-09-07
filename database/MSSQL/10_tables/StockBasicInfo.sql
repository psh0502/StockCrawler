/****** Object:  Table [dbo].[StockBasicInfo]    Script Date: 2017/7/3 ¤U¤È 09:12:15 ******/
DROP TABLE IF EXISTS [dbo].[StockBasicInfo]
GO

/****** Object:  Table [dbo].[StockBasicInfo]    Script Date: 2017/7/3 ¤U¤È 09:12:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[StockBasicInfo](
	[StockNo] [varchar](10) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[CompanyName][nvarchar](100) NOT NULL,
	[CompanyID][varchar](10) NOT NULL,
	[BuildDate][date] NOT NULL,
	[PublishDate][date] NOT NULL,
	[Capital][money] NOT NULL,
	[MarketValue][money] NOT NULL,
	[ReleaseStockCount][bigint] NOT NULL,
	[Chairman][nvarchar](50) NOT NULL,
	[CEO][nvarchar](50) NOT NULL,
	[Url][varchar](100) NOT NULL,
	[Business][nvarchar](1000) NOT NULL,
	[CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
	[LastModifiedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_StockBasicInfo] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
