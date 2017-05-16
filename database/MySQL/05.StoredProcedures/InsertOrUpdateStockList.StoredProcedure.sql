DROP procedure IF EXISTS `DisableAllStocks`;
DELIMITER $$

CREATE PROCEDURE `InsertOrUpdateStockList`(
stockNo varchar(10), stockName varchar(50))
BEGIN
	IF EXISTS(SELECT `StockID` FROM `stock` WHERE `StockNo` = stockNo) THEN
		UPDATE `stock` SET `Enabled` = TRUE WHERE `StockNo` = stockNo;
	ELSE
		INSERT INTO `stock`
		(`StockNo`,
		`StockName`)
		VALUES
		(stockNo,
		stockName);

	END IF;
END