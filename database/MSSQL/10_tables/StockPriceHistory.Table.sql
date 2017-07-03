/****** Object:  Table [dbo].[StockPriceHistory]    Script Date: 2017/7/3 ¤U¤È 10:06:29 ******/
DROP TABLE [dbo].[StockPriceHistory]
GO

/****** Object:  Table [dbo].[StockPriceHistory]    Script Date: 2017/7/3 ¤U¤È 10:06:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StockPriceHistory](
	[StockID] [bigint] NOT NULL,
	[StockDT] [datetime] NOT NULL,
	[OpenPrice] [decimal](10, 4) NOT NULL DEFAULT (0),
	[HighPrice] [decimal](10, 4) NOT NULL DEFAULT (0),
	[LowPrice] [decimal](10, 4) NOT NULL DEFAULT (0),
	[ClosePrice] [decimal](10, 4) NOT NULL DEFAULT (0),
	[AdjClosePrice] [decimal](10, 4) NOT NULL DEFAULT (0),
	[Volume] [bigint] NOT NULL DEFAULT (0),
	[DateCreated] [datetime] NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_StockPriceHistory] PRIMARY KEY CLUSTERED 
(
	[StockID] ASC,
	[StockDT] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
