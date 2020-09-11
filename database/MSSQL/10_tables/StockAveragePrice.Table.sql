/****** Object:  Table [dbo].[StockAveragePrice]    Script Date: 2017/7/3 ¤U¤È 10:06:29 ******/
DROP TABLE IF EXISTS [dbo].[StockAveragePrice]
GO

/****** Object:  Table [dbo].[StockAveragePrice]    Script Date: 2017/7/3 ¤U¤È 10:06:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StockAveragePrice](
	[StockNo] [varchar](10) NOT NULL,
	[StockDT] [date] NOT NULL,
	[Period] SMALLINT NOT NULL,
	[ClosePrice] [decimal](10, 4) NOT NULL DEFAULT (0),
	[CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_StockAveragePrice] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC,
	[StockDT] DESC,
	[Period] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
