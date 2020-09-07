/****** Object:  Table [dbo].[StockReportPerSeason]    Script Date: 2020/8/9 上午 08:30:38 ******/
DROP TABLE IF EXISTS [dbo].[StockReportPerSeason]
GO

/****** Object:  Table [dbo].[StockReportPerSeason]    Script Date: 2020/8/9 上午 08:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- 損益表
CREATE TABLE [dbo].[StockReportPerSeason](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Season] [smallint] NOT NULL,
	[EPS] [MONEY] NOT NULL, -- 每股盈餘：公式：單季EPS = 單季稅後淨利 / 已發行股數
	[NetValue] [MONEY] NOT NULL, -- 每股淨值：公式：每股淨值 = 淨值(股東權益) / 在外流通股數
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockReportPerSeason] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] DESC,
	[Season] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockReportPerSeason] ADD  CONSTRAINT [DF_StockReportPerSeason_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockReportPerSeason] ADD  CONSTRAINT [DF_StockReportPerSeason_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


