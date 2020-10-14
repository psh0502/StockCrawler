UPDATE StockPriceHistory
SET Volume = T.TotalVolume
FROM
	StockPriceHistory a(NOLOCK)
	INNER JOIN
	(
		SELECT b.StockDT, c.CategoryNO, TotalVolume = SUM(b.Volume) 
		FROM StockBasicInfo a(NOLOCK) 
			INNER JOIN StockPriceHistory b(NOLOCK) ON a.StockNo = b.StockNo
			INNER JOIN CategoryMapping c(NOLOCK) ON a.Category = c.Category
		WHERE b.[Period] = 1
		GROUP BY b.StockDT, c.CategoryNO) T ON a.StockDT = T.StockDT AND a.StockNo = T.CategoryNO
WHERE [Period] = 1