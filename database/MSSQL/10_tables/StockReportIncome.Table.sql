/****** Object:  Table [dbo].[StockReportIncome]    Script Date: 2020/8/9 上午 08:30:38 ******/
DROP TABLE IF EXISTS [dbo].[StockReportIncome]
GO

/****** Object:  Table [dbo].[StockReportIncome]    Script Date: 2020/8/9 上午 08:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- 合併綜合損益表
CREATE TABLE [dbo].[StockReportIncome](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Season] [smallint] NOT NULL,
	[Revenue][MONEY] NOT NULL, --營收
	[GrossProfit] [MONEY] NOT NULL, -- 毛利
	[SalesExpense] [MONEY] NOT NULL, -- 銷售費用
	[ManagementCost] [MONEY] NOT NULL, -- 管理費用
	[RDExpense][MONEY] NOT NULL, -- 研發費用
	[OperatingExpenses][MONEY] NOT NULL,-- 營業費用
	[BusinessInterest][MONEY] NOT NULL,-- 營業利益
	[NetProfitTaxFree][MONEY] NOT NULL,-- 稅前淨利
	[NetProfitTaxed][MONEY] NOT NULL, -- 稅後淨利
	[EPS][MONEY] NOT NULL, -- 每股盈餘
	[SEPS][MONEY] NOT NULL, -- 本季每股盈餘
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockReportIncome] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] DESC,
	[Season] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockReportIncome] ADD  CONSTRAINT [DF_StockReportIncome_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockReportIncome] ADD  CONSTRAINT [DF_StockReportIncome_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


