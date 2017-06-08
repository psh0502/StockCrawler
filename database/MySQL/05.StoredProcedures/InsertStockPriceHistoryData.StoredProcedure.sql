DROP procedure IF EXISTS `InsertStockPriceHistoryData`;
DELIMITER $$

CREATE PROCEDURE `InsertStockPriceHistoryData`(
		pStockID INT(11), 
        pStockDT DATETIME, 
        pOpenPrice DECIMAL(10, 4),
        pHighPrice DECIMAL(10, 4),
        pLowPrice DECIMAL(10, 4),
        pClosePrice DECIMAL(10, 4),
        pVolume INT(11),
        pAdjClosePrice DECIMAL(10, 4)
)
BEGIN
	INSERT INTO `StockPriceHistory`(
		`StockID`, 
        `StockDT`, 
        `OpenPrice`, 
        `HighPrice`, 
        `LowPrice`, 
        `ClosePrice`, 
        `Volume`, 
        `AdjClosePrice`) 
	VALUES(
		pStockID, 
        pStockDT, 
        pOpenPrice,
        pHighPrice,
        pLowPrice,
        pClosePrice,
        pVolume,
        pAdjClosePrice);
END