/****** Object:  Table [dbo].[StockAnalysisData]    Script Date: 2017/7/3 �U�� 09:12:15 ******/
DROP TABLE IF EXISTS [dbo].[StockAnalysisData]
GO

/****** Object:  Table [dbo].[StockAnalysisData]    Script Date: 2017/7/3 �U�� 09:12:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[StockAnalysisData](
	[StockNo] VARCHAR(10) NOT NULL,
	[Year] SMALLINT NOT NULL,
	[Price] NVARCHAR(50) NOT NULL,
	[StockCashDivi] DECIMAL(18, 4) NOT NULL,
	[DiviRatio] NVARCHAR(50) NOT NULL,
	[DiviType] NVARCHAR(50) NOT NULL,
	IsPromisingEPS BIT NOT NULL,
	IsGrowingUpEPS BIT NOT NULL,
	IsAlwaysIncomeEPS BIT NOT NULL,
	IsAlwaysPayDivi BIT NOT NULL,
	IsStableDivi BIT NOT NULL,
	IsAlwaysRestoreDivi BIT NOT NULL,
	IsStableOutsideIncome BIT NOT NULL,
	IsStableTotalAmount BIT NOT NULL,
	IsGrowingUpRevenue BIT NOT NULL,
	HasDivi BIT NOT NULL,
	IsRealMode BIT NOT NULL,
	Price5 DECIMAL(18, 4) NOT NULL,
	Price6 DECIMAL(18, 4) NOT NULL,
	Price7 DECIMAL(18, 4) NOT NULL,
	CurrPrice DECIMAL(18, 4) NOT NULL,
	[LastModifiedAt] DATETIME NOT NULL DEFAULT(GETDATE())
 CONSTRAINT [PK_StockAnalysisData] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
