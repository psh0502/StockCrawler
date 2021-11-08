/****** Object:  Table [dbo].[StockMonthlyIncome]    Script Date: 2021/11/08 ******/
DROP TABLE IF EXISTS [dbo].[StockMonthlyIncome]
GO

/****** Object:  Table [dbo].[StockMonthlyIncome]    Script Date: 2021/11/08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- 每月營收
CREATE TABLE [dbo].[StockMonthlyIncome](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Month] [smallint] NOT NULL,
	[Income][MONEY] NOT NULL, --當月營收
	[PreIncome] [MONEY] NOT NULL, -- 去年當月營收
	[DeltaPercent] DECIMAL(18, 4) NOT NULL, -- 去年同月增減(%)
	[CumMonthIncome] [MONEY] NOT NULL, -- 當月累計營收
	[PreCumMonthIncome] [MONEY] NOT NULL, -- 去年累計營收
	[DeltaCumMonthIncomePercent] DECIMAL(18, 4) NOT NULL,-- 前期比較增減(%)
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockMonthlyIncome] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] DESC,
	[Month] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockMonthlyIncome] ADD  CONSTRAINT [DF_StockMonthlyIncome_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockMonthlyIncome] ADD  CONSTRAINT [DF_StockMonthlyIncome_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


