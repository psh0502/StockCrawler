UPDATE Stock
SET CategoryNo = c.CategoryNo
FROM [dbo].[Stock] a(NOLOCK)
INNER JOIN [dbo].[StockBasicInfo] b(NOLOCK) ON a.StockNo = b.StockNo
INNER JOIN [dbo].[CategoryMapping] c(NOLOCK) ON b.Category = c.Category
GO
