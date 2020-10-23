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
GO

UPDATE StockPriceHistory
SET Volume = 
(CASE StockNo 
	WHEN '0009' THEN T.TotalVolume - T.FinancialTotalAmount 
	WHEN '0010' THEN T.TotalVolume - T.ElectronicTotalAmount 
END)
FROM
	StockPriceHistory a(NOLOCK)
	INNER JOIN
	(
		SELECT b.StockDT, TotalVolume = b.Volume, FinancialTotalAmount = c.Volume, ElectronicTotalAmount = SUM(d.Volume)
		FROM StockPriceHistory b(NOLOCK)
			INNER JOIN StockPriceHistory c(NOLOCK) ON c.StockDt = b.StockDT AND c.[Period] = b.[Period] AND c.StockNo = '0040'
			INNER JOIN StockPriceHistory d(NOLOCK) ON d.StockDt = b.StockDT AND d.[Period] = b.[Period] AND d.StockNo BETWEEN '0029' AND '0036'
		WHERE b.[Period] = 1 AND b.StockNo = '0000'
		GROUP BY b.StockDT, b.Volume, c.Volume) T ON a.StockDT = T.StockDT
WHERE [Period] = 1 AND StockNo IN('0009', '0010')
GO
