/****** Object:  Table [dbo].[ETFIngredients]    Script Date: 2021/11/25 ******/
DROP TABLE IF EXISTS [dbo].[ETFIngredients]
GO

/****** Object:  Table [dbo].[ETFIngredients]    Script Date: 2021/11/25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ETFIngredients](
	[ETFNo][varchar](10) NOT NULL,
	[StockNo] [varchar](10) NOT NULL,
	[Quantity] BIGINT NOT NULL,
	[Weight]DECIMAL(18, 4) NOT NULL,
	[CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
	[LastModifiedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_ETFIngredients] PRIMARY KEY CLUSTERED 
(
	[ETFNo], [StockNo]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
