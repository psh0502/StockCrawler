DROP PROCEDURE IF EXISTS `DeleteStockPriceHistoryData`;
DELIMITER $$

CREATE PROCEDURE `DeleteStockPriceHistoryData`(
pStockID int(11), pTradeDate DATETIME)
BEGIN
	DELETE FROM `stockpricehistory`
    WHERE
		(pStockID IS NULL OR pStockID < 0 OR StockID = pStockID)
		AND (pTradeDate IS NULL OR `StockDT` = pTradeDate);
END