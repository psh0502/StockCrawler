/****** Object:  Table [dbo].[StockReportBalance]    Script Date: 2020/8/22 上午 08:30:38 ******/
DROP TABLE IF EXISTS [dbo].[StockReportBalance]
GO

/****** Object:  Table [dbo].[StockReportBalance]    Script Date: 2020/8/22 上午 08:30:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- 損益表
CREATE TABLE [dbo].[StockReportBalance](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Season] [smallint] NOT NULL,
	--資產--
	[CashAndEquivalents][MONEY] NOT NULL, --現金及約當現金
	[ShortInvestments] [MONEY] NOT NULL, -- 短期投資
	[BillsReceivable] [MONEY] NOT NULL, -- 應收帳款及票據
	[Stock] [MONEY] NOT NULL, -- 存貨
	[OtherCurrentAssets][MONEY] NOT NULL, -- 其餘流動資產
	[CurrentAssets][MONEY] NOT NULL,-- 流動資產
	[LongInvestment][MONEY] NOT NULL,-- 長期投資
	[FixedAssets][MONEY] NOT NULL,-- 固定資產
	[OtherAssets][MONEY] NOT NULL, -- 其餘資產
	[TotalAssets][MONEY] NOT NULL, -- 總資產
	-- 負債--
	[ShortLoan][MONEY] NOT NULL, -- 短期借款
	[ShortBillsPayable][MONEY] NOT NULL, -- 應付短期票券
	[AccountsAndBillsPayable][MONEY] NOT NULL, -- 應付帳款及票據
	[AdvenceReceipt][MONEY] NOT NULL, -- 預收款項
	[LongLiabilitiesWithinOneYear][MONEY] NOT NULL, -- 一年內到期長期負債
	[OtherCurrentLiabilities][MONEY] NOT NULL, -- 其餘流動負債
	[CurrentLiabilities][MONEY] NOT NULL, -- 流動負債
	[LongLiabilities][MONEY] NOT NULL, -- 長期負債
	[OtherLiabilities][MONEY] NOT NULL, -- 其餘負債
	[TotalLiability][MONEY] NOT NULL, -- 總負債
	[NetWorth][MONEY] NOT NULL, -- 淨值
	[NAV][MONEY] NOT NULL, -- 每股淨值

	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_StockReportBalance] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[Year] DESC,
	[Season] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StockReportBalance] ADD  CONSTRAINT [DF_StockReportBalance_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[StockReportBalance] ADD  CONSTRAINT [DF_StockReportBalance_LastModifiedAt]  DEFAULT (getdate()) FOR [LastModifiedAt]
GO


