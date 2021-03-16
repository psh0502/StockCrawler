/****** Object:  Table [dbo].[StockInterestIssuedInfo]    Script Date: 2020/8/9 上午 08:30:38 ******/
DROP TABLE IF EXISTS [dbo].[StockInterestIssuedInfo]
GO

/****** Object:  Table [dbo].[StockInterestIssuedInfo]    Script Date: 2020/8/9 上午 08:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- 簡易財務報表
CREATE TABLE [dbo].[StockInterestIssuedInfo](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Season] [smallint] NOT NULL,
	[DecisionDate][DATE] NOT NULL, --董事會決議（擬議）日期
	[ProfitCashIssued] [MONEY] NOT NULL, -- 盈餘分配之現金股利
	[ProfitStockIssued] [MONEY] NOT NULL, -- 盈餘分配之股票股利
	[SsrCashIssued] [MONEY] NOT NULL, -- 法定盈餘公積發放之現金(元/股)
	[SsrStockIssued] [MONEY] NOT NULL, -- 法定盈餘公積轉增資配股(元/股)
	[CapitalReserveCashIssued] [MONEY] NOT NULL,-- 資本公積發放之現金(元/股)
	[CapitalReserveStockIssued] [MONEY] NOT NULL,-- 資本公積轉增資配股(元/股)
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockInterestIssuedInfo] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] DESC,
	[Season] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockInterestIssuedInfo] ADD  CONSTRAINT [DF_StockInterestIssuedInfo_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockInterestIssuedInfo] ADD  CONSTRAINT [DF_StockInterestIssuedInfo_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


