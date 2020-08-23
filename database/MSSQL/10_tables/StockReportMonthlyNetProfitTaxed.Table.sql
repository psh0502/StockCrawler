/****** Object:  Table [dbo].[StockReportMonthlyNetProfitTaxed]    Script Date: 2020/8/23 上午 08:30:38 ******/
DROP TABLE IF EXISTS [dbo].[StockReportMonthlyNetProfitTaxed]
GO

/****** Object:  Table [dbo].[StockReportMonthlyNetProfitTaxed]    Script Date: 2020/8/23 上午 08:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StockReportMonthlyNetProfitTaxed](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Month] [smallint] NOT NULL,
    NetProfitTaxed MONEY NOT NULL,    -- 本月
    LastYearNetProfitTaxed MONEY NOT NULL, -- 去年同期
    Delta MONEY NOT NULL, -- 增減金額
    DeltaPercent DECIMAL(18, 4) NOT NULL, -- 增減百分比
    ThisYearTillThisMonth MONEY NOT NULL, -- 本年累計
    LastYearTillThisMonth MONEY NOT NULL, -- 去年累計
    TillThisMonthDelta MONEY NOT NULL,    -- 增減金額
    TillThisMonthDeltaPercent DECIMAL(18, 4) NOT NULL,    -- 增減百分比
    Remark NVARCHAR(1000) NOT NULL,   -- 備註/營收變化原因說明
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockReportMonthlyNetProfitTaxed] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] DESC,
	[Month] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockReportMonthlyNetProfitTaxed] ADD  CONSTRAINT [DF_StockReportMonthlyNetProfitTaxed_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockReportMonthlyNetProfitTaxed] ADD  CONSTRAINT [DF_StockReportMonthlyNetProfitTaxed_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


