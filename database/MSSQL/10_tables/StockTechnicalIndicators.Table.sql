/****** Object:  Table [dbo].[StockTechnicalIndicators]    Script Date: 2021/11/15 ******/
DROP TABLE IF EXISTS [dbo].[StockTechnicalIndicators]
GO

/****** Object:  Table [dbo].[StockTechnicalIndicators]    Script Date: 2021/11/15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StockTechnicalIndicators](
	[StockNo] [varchar](10) NOT NULL,
	[StockDT] [date] NOT NULL,
	[Type] VARCHAR(10) NOT NULL,
	[Value] [decimal](10, 4) NOT NULL DEFAULT (0),
	[CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_StockTechnicalIndicators] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[StockDT] DESC,
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
