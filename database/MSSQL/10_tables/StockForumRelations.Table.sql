/****** Object:  Table [dbo].[StockForumRelations]    Script Date: 2017/7/3 ¤U¤È 09:12:15 ******/
DROP TABLE IF EXISTS [dbo].[StockForumRelations]
GO

/****** Object:  Table [dbo].[StockForumRelations]    Script Date: 2017/7/3 ¤U¤È 09:12:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[StockForumRelations](
	[StockNo]VARCHAR(10) NOT NULL,
	[ID] BIGINT NOT NULL,
	[CreatedAt] DATETIME NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_StockForumRelations] PRIMARY KEY CLUSTERED 
(
	[StockNo]ASC, [ID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
