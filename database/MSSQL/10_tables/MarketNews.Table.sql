/****** Object:  Table [dbo].[MarketNews]    Script Date: 2017/7/3 ¤U¤È 09:12:15 ******/
DROP TABLE IF EXISTS [dbo].[MarketNews]
GO

/****** Object:  Table [dbo].[MarketNews]    Script Date: 2017/7/3 ¤U¤È 09:12:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MarketNews](
	[NewsDate] DATE NOT NULL DEFAULT(GETDATE()),
	[Subject] [nvarchar](100) NOT NULL,
	[Url] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE())
 CONSTRAINT [PK_MarketNews] PRIMARY KEY CLUSTERED 
(
	[NewsDate] DESC,
	[Subject] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
