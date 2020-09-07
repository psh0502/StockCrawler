/****** Object:  Table [dbo].[StockReportPerMonth]    Script Date: 2020/8/9 上午 08:30:38 ******/
DROP TABLE IF EXISTS [dbo].[StockReportPerMonth]
GO

/****** Object:  Table [dbo].[StockReportPerMonth]    Script Date: 2020/8/9 上午 08:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- 損益表
CREATE TABLE [dbo].[StockReportPerMonth](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Month] [smallint] NOT NULL,
	[PE] [MONEY] NOT NULL, -- 本益比：公式：本益比 = 月均價 / 近4季EPS總和
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockReportPerMonth] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] DESC,
	[Month] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockReportPerMonth] ADD  CONSTRAINT [DF_StockReportPerMonth_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockReportPerMonth] ADD  CONSTRAINT [DF_StockReportPerMonth_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


