/****** Object:  Table [dbo].[Stock]    Script Date: 2017/7/3 ¤U¤È 09:12:15 ******/
DROP TABLE IF EXISTS [dbo].[Stock]
GO

/****** Object:  Table [dbo].[Stock]    Script Date: 2017/7/3 ¤U¤È 09:12:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Stock](
	[StockNo] [varchar](10) NOT NULL,
	[StockName] [nvarchar](50) NOT NULL,
	[Enable] [bit] NOT NULL DEFAULT ((0)),
	[CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
	[LastModifiedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[StockNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
