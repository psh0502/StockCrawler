/****** Object:  Table [dbo].[StockFinancialReport]    Script Date: 2020/8/9 上午 08:30:38 ******/
DROP TABLE IF EXISTS [dbo].[StockFinancialReport]
GO

/****** Object:  Table [dbo].[StockFinancialReport]    Script Date: 2020/8/9 上午 08:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- 簡易財務報表
CREATE TABLE [dbo].[StockFinancialReport](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Season] [smallint] NOT NULL,
	[TotalAssets][MONEY] NOT NULL, --資產總計
	[TotalLiability] [MONEY] NOT NULL, -- 負債
	[NetWorth] [MONEY] NOT NULL, -- 權益總計
	[NAV] [MONEY] NOT NULL, -- 每股淨值
	[Revenue] [MONEY] NOT NULL, -- 營業收入
	[BusinessInterest] [MONEY] NOT NULL,-- 營業利益
	[NetProfitTaxFree] [MONEY] NOT NULL,-- 稅前淨利
	[EPS][MONEY] NOT NULL, -- 每股盈餘
	[BusinessCashflow] [MONEY] NOT NULL,-- 營業活動之淨現金流入
	[InvestmentCashflow] [MONEY] NOT NULL,-- 投資活動之淨現金流入
	[FinancingCashflow] [MONEY] NOT NULL,-- 籌資活動之淨現金流入
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockFinancialReport] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] DESC,
	[Season] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockFinancialReport] ADD  CONSTRAINT [DF_StockFinancialReport_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockFinancialReport] ADD  CONSTRAINT [DF_StockFinancialReport_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


