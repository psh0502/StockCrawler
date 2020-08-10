/****** Object:  Table [dbo].[StockReportCashFlow]    Script Date: 2020/8/9 上午 08:30:38 ******/
DROP TABLE [dbo].[StockReportCashFlow]
GO

/****** Object:  Table [dbo].[StockReportCashFlow]    Script Date: 2020/8/9 上午 08:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StockReportCashFlow](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Season] [smallint] NOT NULL,
	[Depreciation][MONEY] NOT NULL, --折舊費用
	[AmortizationFee] [MONEY] NOT NULL, -- 攤銷
	[BusinessCashflow] [MONEY] NOT NULL, -- 營業現金流, 營業活動之淨現金流入（流出）
	[InvestmentCashflow] [MONEY] NOT NULL, -- 投資現金流, 投資活動之淨現金流入（流出）
	[FinancingCashflow][MONEY] NOT NULL, -- 融資現金流, 籌資活動之淨現金流入（流出）
	[CapitalExpenditures][MONEY] NOT NULL,-- 資本支出, (取得不動產、廠房及設備 + 處分不動產、廠房及設備)
	[FreeCashflow][MONEY] NOT NULL,-- 自由現金流 = (營業現金流 - 資本支出 - 股利支出)
	[NetCashflow][MONEY] NOT NULL,-- 淨現金流 = 營業現金流 - 投資現金流 + 融資現金流
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockReportCashFlow] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] ASC,
	[Season] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockReportCashFlow] ADD  CONSTRAINT [DF_StockReportCashFlow_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockReportCashFlow] ADD  CONSTRAINT [DF_StockReportCashFlow_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


