/****** Object:  Table [dbo].[StockMonthlyIncome]    Script Date: 2021/11/08 ******/
DROP TABLE IF EXISTS [dbo].[StockMonthlyIncome]
GO

/****** Object:  Table [dbo].[StockMonthlyIncome]    Script Date: 2021/11/08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- �C���禬
CREATE TABLE [dbo].[StockMonthlyIncome](
	[StockNo] [varchar](10) NOT NULL,
	[Year] [smallint] NOT NULL,
	[Month] [smallint] NOT NULL,
	[Income][MONEY] NOT NULL, --����禬
	[PreIncome] [MONEY] NOT NULL, -- �h�~����禬
	[DeltaPercent] DECIMAL(18, 4) NOT NULL, -- �h�~�P��W��(%)
	[CumMonthIncome] [MONEY] NOT NULL, -- ���֭p�禬
	[PreCumMonthIncome] [MONEY] NOT NULL, -- �h�~�֭p�禬
	[DeltaCumMonthIncomePercent] DECIMAL(18, 4) NOT NULL,-- �e������W��(%)
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


